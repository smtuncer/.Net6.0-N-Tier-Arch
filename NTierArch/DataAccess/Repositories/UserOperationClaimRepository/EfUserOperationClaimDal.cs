using Core.DataAccess.EntityFramework;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.UserOperationClaimRepository
{
    public class EfUserOperationClaimDal : EfEntitiyRepositoryBase<UserOperationClaim, ApplicationDbContext>, IUserOperationClaimDal
    {
    }
}
