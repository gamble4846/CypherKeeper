﻿using CypherKeeper.AuthLayer.Models;
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
        public IConfiguration Configuration { get; }
        public Cryptography(IConfiguration configuration)
        {
            Configuration = configuration;
        }

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


        #region SettingsModel

        public SettingsModel EncryptData(SettingsModel model, string EncryptionKey)
        {
            foreach(var server in model.Servers)
            {
                server.DatabaseType = Encrypt(server.DatabaseType, EncryptionKey);
                server.ConnectionString = Encrypt(server.ConnectionString, EncryptionKey);
            }

            return model;
        }

        public SettingsModel DecryptData(SettingsModel model, string EncryptionKey)
        {
            foreach (var server in model.Servers)
            {
                server.DatabaseType = Decrypt(server.DatabaseType, EncryptionKey);
                server.ConnectionString = Decrypt(server.ConnectionString, EncryptionKey);
            }

            return model;
        }

        #endregion
    }
}