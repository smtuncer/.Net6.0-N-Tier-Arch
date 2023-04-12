using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Business.Utilities.File
{
    public class FileManager : IFileService
    {
        public string FileSaveToServer(IFormFile file, string filePath)
        {
            var ext = file.FileName.Substring(file.FileName.LastIndexOf('.'));
            ext = ext.ToLower();
            string fileName = Guid.NewGuid().ToString() + ext;
            string path = filePath + fileName;
            using (var stream = System.IO.File.Create(path))
            {
                file.CopyTo(stream);
            }
            return fileName;
        }

        public string FileSaveToFtp(IFormFile file)
        {
            var ext = file.FileName.Substring(file.FileName.LastIndexOf('.'));
            ext = ext.ToLower();
            string fileName = Guid.NewGuid().ToString() + ext;
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("FTP ADRESİ YAZILACAK" + fileName);
            request.Credentials = new NetworkCredential("Kullanıcı Adı", "Şifre");
            request.Method = WebRequestMethods.Ftp.UploadFile;
            using (Stream ftpStream = request.GetRequestStream())
            {
                file.CopyTo(ftpStream);
            }
            return fileName;
        }

        public void FileDeleteToServer(string path)
        {
            try
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void FileDeleteToFtp(string path)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp adresi" + path);
                request.Credentials = new NetworkCredential("Kullanıcı adı", "şifre");
                request.Method = WebRequestMethods.Ftp.DeleteFile;
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
