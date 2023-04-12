using Core.Utilities.Result.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repositories.EmailParameterRepository
{
    public interface IEmailParameterService
    {
        IResult Add(EmailParameter emailParameter);
        IResult Update(EmailParameter emailParameter);
        IResult Delete(EmailParameter emailParameter);
        Task<IDataResult<List<EmailParameter>>> GetList();
        IDataResult<EmailParameter> GetById(int id);
        IResult SendEmail(EmailParameter emailParameter, string body, string subject, string emails);
    }
}
