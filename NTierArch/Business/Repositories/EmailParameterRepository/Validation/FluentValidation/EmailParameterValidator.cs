using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repositories.EmailParameterRepository.Validation.FluentValidation
{
    internal class EmailParameterValidator : AbstractValidator<EmailParameter>
    {
        public EmailParameterValidator()
        {
            RuleFor(x => x.Smtp).NotEmpty().WithMessage("SMTP adresi boş bırakılamaz");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email adresi boş bırakılamaz");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Şifre boş bırakılamaz");
            RuleFor(x => x.Port).NotEmpty().WithMessage("Port boş bırakılamaz");
        }
    }
}
