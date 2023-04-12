using Business.Aspects.Security;
using Business.Repositories.OperationClaimRepository.Constans;
using Business.Repositories.OperationClaimRepository.Validation.FluentValidation;
using Business.Repositories.UserOperationClaimRepository.Constans;
using Business.Repositories.UserRepository.Constans;
using Core.Aspects.Performance;
using Core.Aspects.Validation;
using Core.Utilities.Business;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Repositories.OperationClaimRepository;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Business.Repositories.OperationClaimRepository
{
    public class OperationClaimManager : IOperationClaimService
    {
        private readonly IOperationClaimDal _operationClaimDal;

        public OperationClaimManager(IOperationClaimDal operationClaimDal)
        {
            _operationClaimDal = operationClaimDal;
        }
        [ValidationAspect(typeof(OperationClaimValidator))]
        public IResult Add(OperationClaim operationClaim)
        {
            IResult result = BusinessRules.Run(IsNameExistForAdd(operationClaim.Name));
            if (result != null)
            {
                return result;
            }
            _operationClaimDal.Add(operationClaim);
            return new SuccessResult(OperationClaimMessages.Added);
        }

        public IResult Delete(OperationClaim operationClaim)
        {
            _operationClaimDal.Delete(operationClaim);
            return new SuccessResult(OperationClaimMessages.Deleted);
        }

        public IDataResult<OperationClaim> GetById(int id)
        {
            return new SuccessDataResult<OperationClaim>(_operationClaimDal.Get(x => x.Id == id));
        }
        [SecuredAspect()]
        [PerformanceAspect()]
        public async Task<IDataResult<List<OperationClaim>>> GetList()
        {
            _operationClaimDal.GetAll();
            return new SuccessDataResult<List<OperationClaim>>(await _operationClaimDal.GetAll());
        }
        [ValidationAspect(typeof(OperationClaimValidator))]
        public IResult Update(OperationClaim operationClaim)
        {
            IResult result = BusinessRules.Run(IsNameExistForUpdate(operationClaim));
            if (result != null)
            {
                return result;
            }
            _operationClaimDal.Update(operationClaim);
            return new SuccessResult(OperationClaimMessages.Updated);
        }

        private IResult IsNameExistForAdd(string name)
        {
            var result = _operationClaimDal.Get(x => x.Name == name);
            if (result != null)
            {
                return new ErrorResult(OperationClaimMessages.NameIsNotAvailble);
            }
            return new SuccessResult();
        }
        private IResult IsNameExistForUpdate(OperationClaim operationClaim)
        {
            var currentOperationClaim = _operationClaimDal.Get(x => x.Id == operationClaim.Id);
            if (currentOperationClaim.Name != operationClaim.Name)
            {
                var result = _operationClaimDal.Get(x => x.Name == operationClaim.Name);
                if (result != null)
                {
                    return new ErrorResult(OperationClaimMessages.NameIsNotAvailble);
                }
            }
            return new SuccessResult();
        }
    }
}
