using Business.Repositories.UserRepository.Constans;
using Business.Repositories.UserRepository.Validation.FluentValidation;
using Business.Utilities.File;
using Core.Aspects.Caching;
using Core.Aspects.Transaction;
using Core.Aspects.Validation;
using Core.Utilities.Hashing;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Repositories.UserRepository;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repositories.UserRepository
{
    public class UserManager : IUserService
    {
        private readonly IUserDal _userDal;
        private readonly IFileService _fileService;

        public UserManager(IUserDal userDal, IFileService fileService)
        {
            _userDal = userDal;
            _fileService = fileService;
        }

        [RemoveCacheAspect("IUserService.GetList")]
        public async Task Add(RegisterAuthDto registerAuthDto)
        {
            string fileName = _fileService.FileSaveToServer(registerAuthDto.Image, "./Content/img/");
            //string fileName = _fileService.FileSaveToFtp(registerAuthDto.Image);

            var user = CreateUser(registerAuthDto, fileName);
            await _userDal.Add(user);
        }
        private User CreateUser(RegisterAuthDto registerAuthDto, string fileName)
        {
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePassword(registerAuthDto.Password, out passwordHash, out passwordSalt);

            User user = new User();
            user.Id = 0;
            user.Email = registerAuthDto.Email;
            user.Name = registerAuthDto.Name;
            user.ImageUrl = fileName;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            return user;
        }
        public User GetByEmail(string email)
        {
            return _userDal.Get(p => p.Email == email);
        }
        [ValidationAspect(typeof(UserValidator))]
        [TransactionAspect()]
        public IResult Update(User user)
        {
            _userDal.Update(user);
            return new SuccessResult(UserMessages.UpdatedUser);
        }

        public IResult Delete(User user)
        {
            _userDal.Delete(user);
            return new SuccessResult(UserMessages.DeletedUser);
        }

        [CacheAspect(60)]
        public async Task< IDataResult<List<User>>> GetList()
        {
            return new SuccessDataResult<List<User>>(await _userDal.GetAll());
        }

        public IDataResult<User> GetById(int id)
        {
            return new SuccessDataResult<User>(_userDal.Get(x => x.Id == id));
        }

        [ValidationAspect(typeof(UserChangePasswordValidator))]
        public IResult ChangePassword(UserChangePasswordDto userChangePasswordDto)
        {
            var user = _userDal.Get(x => x.Id == userChangePasswordDto.UserId);
            bool result = HashingHelper.VerifyPasswordHash(userChangePasswordDto.CurrentPassword, user.PasswordHash, user.PasswordSalt);
            if (!result)
            {
                return new ErrorResult(UserMessages.WrongCurrentPassword);
            }
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePassword(userChangePasswordDto.NewPassword, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            _userDal.Update(user);
            return new SuccessResult(UserMessages.PasswordChanged);
        }

        public List<OperationClaim> GetUserOperationClaims(int userId)
        {
            return _userDal.GetUserOperationClaims(userId);
        }
    }
}
