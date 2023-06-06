using CypherKeeper.AuthLayer.Models;
using CypherKeeper.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace CypherKeeper.AuthLayer.Utility
{
    public class Cryptography
    {
        public IConfiguration Configuration { get; set; }
        public IHttpContextAccessor HttpContextAccessor { get; set; }
        public string DecryptionKey { get; set; }
        public CommonFunctions CommonFunctions { get; set; }
        public SelectedServerModel CurrentServer { get; set; }
        
        public Cryptography(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            Configuration = configuration;
            HttpContextAccessor = httpContextAccessor;
            DecryptionKey = GetCurrentDecryptedKey();
            CommonFunctions = new CommonFunctions(configuration, httpContextAccessor);
            CurrentServer = CommonFunctions.GetCurrentServer();
        }

        #region Other Helper Funcations

        public string GetCurrentDecryptedKey()
        {
            try
            {
                return DecryptRSAEncryptedString(HttpContextAccessor.HttpContext.Request.Headers["DecryptKey"]);
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region RSA Encrypt Decrypt

        public string DecryptRSAEncryptedString(string cipherText)
        {
            var RSACryptographySection = Configuration.GetSection("RSACryptography");

            string privateKey = RSACryptographySection.GetValue<String>("PrivateKey");
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(privateKey);
            var encryptedBytes = Convert.FromBase64String(cipherText);
            var decryptedBytes = rsa.Decrypt(encryptedBytes,false);
            var decryptedData = Encoding.UTF8.GetString(decryptedBytes);

            return decryptedData;
        }

        public string EncryptRSAString(string data)
        {
            var RSACryptographySection = Configuration.GetSection("RSACryptography");

            string PublicKey = RSACryptographySection.GetValue<String>("PublicKey");
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(PublicKey);
            var dataBytes = Encoding.UTF8.GetBytes(data);
            var encryptedBytes = rsa.Encrypt(dataBytes, false);
            var encryptedData = Convert.ToBase64String(encryptedBytes);
            return encryptedData;
        }

        #endregion

        #region Basic Encrypt Decrypt

        public string Encrypt(string clearText, string EncryptionKey = null)
        {
            var jwtSection = Configuration.GetSection("Jwt");

            if(String.IsNullOrEmpty(EncryptionKey))
                EncryptionKey = jwtSection.GetValue<String>("Secret");

            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public string Decrypt(string cipherText, string EncryptionKey = null)
        {
            var jwtSection = Configuration.GetSection("Jwt");

            if (String.IsNullOrEmpty(EncryptionKey))
                EncryptionKey = jwtSection.GetValue<String>("Secret");

            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        #endregion

        #region tbGroupsModel

        public tbGroupsModel EncryptModel(tbGroupsModel model)
        {
            model.Name = Encrypt(model.Name, CurrentServer.Key);
            return model;
        }

        public tbGroupsModel DecryptModel(tbGroupsModel model)
        {
            model.Name = Encrypt(model.Name, CurrentServer.Key);
            return model;
        }

        #endregion
    }
}
