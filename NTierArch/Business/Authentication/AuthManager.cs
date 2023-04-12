using Business.Abstract;
using Business.Authentication.Validation.FluentValidation;
using Business.Repositories.UserRepository;
using Core.Aspects.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Business;
using Core.Utilities.Hashing;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using Core.Utilities.Security.JWT;
using Entities.Concrete;
using Entities.Dtos;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private readonly IUserService _userService;
        private readonly ITokenHandler _tokenHandler;

        public AuthManager(IUserService userService, ITokenHandler tokenHandler)
        {
            _userService = userService;
            _tokenHandler = tokenHandler;
        }
        public IDataResult<Token> Login(LoginAuthDto loginAuthDto)
        {
            var user = _userService.GetByEmail(loginAuthDto.Email);
            var result = HashingHelper.VerifyPasswordHash(loginAuthDto.Password, user.PasswordHash, user.PasswordSalt);
            List<OperationClaim> operationClaims = _userService.GetUserOperationClaims(user.Id);
            if (result)
            {
                Token token = new Token();
                token = _tokenHandler.CreateToken(user, operationClaims);
                return new SuccessDataResult<Token>(token);
            }
            return new ErrorDataResult<Token>("Kullanıcı maili ya da şifre bilgisi hatalı");
        }
        [ValidationAspect(typeof(AuthValidator))]
        public async Task<IResult> Register(RegisterAuthDto registerAuthDto)
        {
            IResult result = BusinessRules.Run(
                CheckIfEmailExist(registerAuthDto.Email),
                CheckIfImageExtensionsAllow(registerAuthDto.Image.FileName),
                CheckIfImageSizeIsLessThanOneMb(registerAuthDto.Image.Length)
                );

            if (result != null)
            {
                return result;
            }

            await _userService.Add(registerAuthDto);
            return new SuccessResult("Kullanıcı kaydı başarıyla tamamlandı");
        }

        private IResult CheckIfEmailExist(string email)
        {
            var list = _userService.GetByEmail(email);
            if (list != null)
            {
                return new ErrorResult("Mail adresi daha önce kullanılmış");
            }
            return new SuccessResult();
        }
        private IResult CheckIfImageSizeIsLessThanOneMb(long imgSize)
        {
            decimal imgMbSize = Convert.ToDecimal(imgSize * 0.000001);
            if (imgMbSize > 1)
            {
                return new ErrorResult("En fazla 1 mb resim yüklenebilir");
            }
            return new SuccessResult();
        }
        private IResult CheckIfImageExtensionsAllow(string fileName)
        {
            string ext = fileName.Substring(fileName.LastIndexOf('.'));
            string extension = ext.ToLower();
            List<string> AllowFileExtensions = new List<string> { ".jpeg", ".jpg", ".png", ".gif" };
            if (!AllowFileExtensions.Contains(extension))
            {
                return new ErrorResult("Resim .jpeg, .jpg, .gif ya da .png uzantılı olmalıdır");
            }
            return new SuccessResult();
        }
    }
}
