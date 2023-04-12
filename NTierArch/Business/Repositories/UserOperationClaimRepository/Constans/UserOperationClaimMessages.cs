using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repositories.UserOperationClaimRepository.Constans
{
    public class UserOperationClaimMessages
    {
        public static string Added = "Yetki ataması başarılı";
        public static string Updated = "Yetki ataması başarıyla güncellendi";
        public static string Deleted = "Yetki ataması başarıyla silindi";     
        public static string OperationClaimSetExist = "Kullanıcıya bu yetki daha önceden atanmış";                    
        public static string OperationClaimNotExist = "Seçilen yetki bilgisi mevcut değil";                      
        public static string UserNotExist = "Seçilen kullanıcı bulunamadı";
    }
}
