using Core.Utilities.Result.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repositories.UserRepository
{
    public interface IUserService
    {
        Task Add(RegisterAuthDto registerAuthDto);
        IResult Update(User user);
        IResult ChangePassword(UserChangePasswordDto userChangePasswordDto);
        IResult Delete(User user);
        Task<IDataResult<List<User>>> GetList();
        User GetByEmail(string email);
        List<OperationClaim> GetUserOperationClaims(int userId);
        IDataResult<User> GetById(int id);
    }
}
