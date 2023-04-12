using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repositories.UserOperationClaimRepository.Validation.FluentValidation
{
    public class UserOperationClaimValidator : AbstractValidator<UserOperationClaim>
    {
        public UserOperationClaimValidator()
        {
            RuleFor(x => x.UserId).Must(IsIdValid).WithMessage("Yetki ataması için kullanıcı seçimi yapılmalıdır");
            RuleFor(x => x.OperationClaimId).Must(IsIdValid).WithMessage("Yetki ataması için yetki seçilmelidir");
        }
        private bool IsIdValid(int id)
        {
            if (id > 0 && id != null)
            {
                return true;
            }
            return false;
        }
    }
}
