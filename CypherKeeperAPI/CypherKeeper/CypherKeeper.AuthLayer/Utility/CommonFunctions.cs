using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using CypherKeeper.AuthLayer.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Security.Principal;
using CypherKeeper.Model;
using OtpNet;
//using System.Runtime.Intrinsics.Arm;

namespace CypherKeeper.AuthLayer.Utility
{
    public class CommonFunctions
    {
        public IConfiguration Configuration { get; }
        public string ContentRootPath { get; set; }
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public CommonFunctions(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            Configuration = configuration;
            HttpContextAccessor = httpContextAccessor;
        }

        public SettingsModel GetSettings()
        {
            try
            {
                var currentUser = GetCurrentUser();
                return currentUser.Settings;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string GetTokenFromHeader()
        {
            try
            {
                return HttpContextAccessor.HttpContext.Request.Headers["Authorization"];
            }
            catch (Exception)
            {
                return null;
            }
        }

        public MongoDBValues GetMongoDBValues()
        {
            var mongoSection = Configuration.GetSection("MongoDB");

            var model = new MongoDBValues();
            model.ConnectionString = mongoSection.GetValue<string>("ConnectionString");
            model.Database = mongoSection.GetValue<string>("Database");

            return model;
        }

        public string CreateJWTToken(List<ClaimModel> Claims)
        {
            var jwtSection = Configuration.GetSection("Jwt");
            var Secret = jwtSection.GetValue<string>("Secret");
            var ValidIssuer = jwtSection.GetValue<string>("ValidIssuer");
            var ValidAudience = jwtSection.GetValue<string>("ValidAudience");


            var claims = new List<Claim>();

            foreach (var claim in Claims)
            {
                claims.Add(new Claim(claim.ClaimName, JsonConvert.SerializeObject(claim.Data)));
            }

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(issuer: ValidIssuer, audience: ValidAudience, claims: claims, expires: DateTime.Now.AddYears(1), signingCredentials: signinCredentials);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

            return tokenString;
        }

        public string CreateJWTToken(List<Claim> Claims)
        {
            var jwtSection = Configuration.GetSection("Jwt");
            var Secret = jwtSection.GetValue<string>("Secret");
            var ValidIssuer = jwtSection.GetValue<string>("ValidIssuer");
            var ValidAudience = jwtSection.GetValue<string>("ValidAudience");

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(issuer: ValidIssuer, audience: ValidAudience, claims: Claims, expires: DateTime.Now.AddYears(1), signingCredentials: signinCredentials);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

            return tokenString;
        }

        public List<Claim> GetClaimsFromToken(string JWTToken)
        {
            try
            {
                JWTToken = JWTToken.Replace("Bearer ", "");
                var jwtSection = Configuration.GetSection("Jwt");
                var Secret = Encoding.ASCII.GetBytes(jwtSection.GetValue<string>("Secret"));
                var ValidIssuer = jwtSection.GetValue<string>("ValidIssuer");
                var ValidAudience = jwtSection.GetValue<string>("ValidAudience");
                var handler = new JwtSecurityTokenHandler();
                var validations = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Secret),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                var claims = handler.ValidateToken(JWTToken, validations, out var tokenSecure);
                var otherClaims = claims.Identities.ToList()[0].Claims.ToList();

                return otherClaims;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public SelectedServerModel GetCurrentServer()
        {
            try
            {
                var accessToken = GetTokenFromHeader();
                if (string.IsNullOrEmpty(accessToken)) { return null; }

                var claims = GetClaimsFromToken(accessToken);
                if (claims == null) { return null; }

                var ServerDataClaim = claims.Find(x => x.Type == "ServerData");
                if (ServerDataClaim == null) { return null; }
                var unEncrypedServerDataValue = Decrypt(ServerDataClaim.Value);

                var ServerData = JsonConvert.DeserializeObject<SelectedServerModel>(unEncrypedServerDataValue);
                if (ServerData == null) { return null; }

                return ServerData;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public tbAccessModel GetCurrentUser()
        {
            try
            {
                var accessToken = GetTokenFromHeader();
                if (string.IsNullOrEmpty(accessToken)) { return null; }

                var claims = GetClaimsFromToken(accessToken);
                if (claims == null) { return null; }

                var LoginDataClaim = claims.Find(x => x.Type == "LoginData");
                if (LoginDataClaim == null) { return null; }

                var LoginData = JsonConvert.DeserializeObject<LoginModel>(LoginDataClaim.Value);
                if (LoginData == null) { return null; }
                LoginData.Password = Decrypt(LoginData.Password);

                var MongoValues = GetMongoDBValues();
                var Settings = MongoClientSettings.FromConnectionString(MongoValues.ConnectionString);
                Settings.ServerApi = new ServerApi(ServerApiVersion.V1);
                var Client = new MongoClient(Settings);
                var Database = Client.GetDatabase(MongoValues.Database);
                var Collection = "tbAccess";
                var collection = Database.GetCollection<tbAccessModel>(Collection);
                var filter = Builders<tbAccessModel>.Filter.Eq("Username", LoginData.Username);
                var documents = collection.Find(filter).ToList();
                if (documents.Count == 0) { return null; }

                var CurrentUserData = documents[0];
                var VerifyPassword = SecureHasher.Verify(LoginData.Password, CurrentUserData.Password);
                if (!VerifyPassword) { return null; }

                return CurrentUserData;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string GetCurrentPublicEncryptionKey()
        {
            try
            {
                return HttpContextAccessor.HttpContext.Request.Headers["PublicEncryptionKey"];
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static T CreateDeepCopy<T>(T obj)
        {
            var stringObj = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            var returnObect = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(stringObj);
            return returnObect;
        }

        public bool ValidateCurrentToken()
        {
            var jwtSection = Configuration.GetSection("Jwt");
            var Secret = jwtSection.GetValue<string>("Secret");
            var ValidIssuer = jwtSection.GetValue<string>("ValidIssuer");
            var ValidAudience = jwtSection.GetValue<string>("ValidAudience");

            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters()
            {
                ValidateLifetime = false, // Because there is no expiration in the generated token
                ValidateAudience = false, // Because there is no audiance in the generated token
                ValidateIssuer = false,   // Because there is no issuer in the generated token
                ValidIssuer = ValidIssuer,
                ValidAudience = ValidAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret)) // The same key as the one that generate the token
            };

            SecurityToken validatedToken;
            var oldToken = GetTokenFromHeader();
            oldToken = oldToken.Replace("Bearer ", "");
            IPrincipal principal = tokenHandler.ValidateToken(oldToken, validationParameters, out validatedToken);
            return true;
        }

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

        public string GetSQLCurrentDateTime()
        {
            var CurrentUTCDate = DateTime.UtcNow;
            var DateString = CurrentUTCDate.Year + "-" + CurrentUTCDate.Month + "-" + CurrentUTCDate.Day + " " + CurrentUTCDate.Hour + ":" + CurrentUTCDate.Minute + ":" + CurrentUTCDate.Second;
            return DateString;
        }

         
        public string DecryptRSAEncryptedString(string cipherText, string privateKey = null)
        {
            var RSACryptographySection = Configuration.GetSection("RSACryptography");

            if (String.IsNullOrEmpty(privateKey))
            {
                privateKey = RSACryptographySection.GetValue<String>("PrivateKey");
            }
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(privateKey);
            var encryptedBytes = Convert.FromBase64String(cipherText);
            var decryptedBytes = rsa.Decrypt(encryptedBytes, false);
            var decryptedData = Encoding.UTF8.GetString(decryptedBytes);

            return decryptedData;
        }

        public string EncryptRSAString(string data, string publicKey = null)
        {
            var RSACryptographySection = Configuration.GetSection("RSACryptography");

            if (String.IsNullOrEmpty(publicKey))
            {
                publicKey = RSACryptographySection.GetValue<String>("PublicKey");
            }

            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publicKey);
            var dataBytes = Encoding.UTF8.GetBytes(data);
            var encryptedBytes = rsa.Encrypt(dataBytes, false);
            var encryptedData = Convert.ToBase64String(encryptedBytes);
            return encryptedData;
        }


        public string Encrypt(string clearText, string EncryptionKey = null)
        {
            var jwtSection = Configuration.GetSection("Jwt");

            if (String.IsNullOrEmpty(EncryptionKey))
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


        public tbGroupsModel EncryptModel(tbGroupsModel model)
        {
            model.Name = Encrypt(model.Name, GetCurrentServer().Key);
            return model;
        }

        public tbGroupsModel DecryptModel(tbGroupsModel model)
        {
            model.Name = Decrypt(model.Name, GetCurrentServer().Key);
            return model;
        }


        public tbKeysHistoryModel EncryptModel(tbKeysHistoryModel model)
        {
            model.KeysJSON = Encrypt(model.KeysJSON, GetCurrentServer().Key);
            return model;
        }

        public tbKeysHistoryModel DecryptModel(tbKeysHistoryModel model)
        {
            model.KeysJSON = Decrypt(model.KeysJSON, GetCurrentServer().Key);
            return model;
        }


        public tbKeysModel EncryptModel(tbKeysModel model)
        {
            model.Name = Encrypt(model.Name, GetCurrentServer().Key);
            model.UserName = Encrypt(model.UserName, GetCurrentServer().Key);
            model.Password = Encrypt(model.Password, GetCurrentServer().Key);
            model.Notes = Encrypt(model.Notes, GetCurrentServer().Key);
            return model;
        }

        public tbKeysModel DecryptModel(tbKeysModel model)
        {
            model.Name = Decrypt(model.Name, GetCurrentServer().Key);
            model.UserName = Decrypt(model.UserName, GetCurrentServer().Key);
            model.Password = Decrypt(model.Password, GetCurrentServer().Key);
            model.Notes = Decrypt(model.Notes, GetCurrentServer().Key);
            return model;
        }


        public tbStringKeyFieldsModel EncryptModel(tbStringKeyFieldsModel model)
        {
            model.Name = Encrypt(model.Name, GetCurrentServer().Key);
            model.Value = Encrypt(model.Value, GetCurrentServer().Key);
            return model;
        }

        public tbStringKeyFieldsModel DecryptModel(tbStringKeyFieldsModel model)
        {
            model.Name = Decrypt(model.Name, GetCurrentServer().Key);
            model.Value = Decrypt(model.Value, GetCurrentServer().Key);
            return model;
        }

        public IEnumerable<string> SplitByLength(string str, int maxLength)
        {
            for (int index = 0; index < str.Length; index += maxLength)
            {
                yield return str.Substring(index, Math.Min(maxLength, str.Length - index));
            }
        }

        public List<string> EncryptFinalResponseString(string dataString)
        {
            var SplitedString = SplitByLength(dataString, 100);

            var publicKey = GetCurrentPublicEncryptionKey();

            if(String.IsNullOrEmpty(publicKey))
            {
                throw new Exception("Public Key Not Found");
            }

            try
            {
                byte[] data = Convert.FromBase64String(publicKey);
                publicKey = Encoding.UTF8.GetString(data);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message,ex);
            }

            var EncryptedResponseStrings = new List<string>();

            foreach(var str in SplitedString)
            {
                var rsa = new RSACryptoServiceProvider();
                rsa.ImportFromPem(publicKey.ToCharArray());
                var dataBytes = Encoding.UTF8.GetBytes(str);
                var encryptedBytes = rsa.Encrypt(dataBytes, false);
                var encryptedData = Convert.ToBase64String(encryptedBytes);

                EncryptedResponseStrings.Add(encryptedData);
            }

            return EncryptedResponseStrings;
        }

        public Totp GetTotp(Int32 Step, string Mode, int Size, string SecretKey)
        {
            var secretKey = Base32Encoding.ToBytes(SecretKey);
            TimeCorrection timeCorrection = null;
            var totp = new Totp(secretKey, Step, GetOTPMode_ENUM(Mode), Size, timeCorrection);
            return totp;
            //var totpCode = totp.ComputeTotp();
            //var remainingTime = totp.RemainingSeconds();


            //var hotp = new Hotp(secretKey, Mode, TOTPSize);
            //var hotpCode = hotp.ComputeHOTP(1);

        }

        public Hotp GetHotp(string Mode, int Size, string SecretKey)
        {
            var secretKey = Base32Encoding.ToBytes(SecretKey);
            var hotp = new Hotp(secretKey, GetOTPMode_ENUM(Mode), Size);
            return hotp;
            //var totpCode = totp.ComputeTotp();
            //var remainingTime = totp.RemainingSeconds();


            //var hotp = new Hotp(secretKey, Mode, TOTPSize);
            //var hotpCode = hotp.ComputeHOTP(1);

        }

        public OtpHashMode GetOTPMode_ENUM(string Mode)
        {
            switch (Mode)
            {
                case "Sha1":
                    return OtpHashMode.Sha1;
                case "Sha256":
                    return OtpHashMode.Sha256;
                case "Sha512":
                    return OtpHashMode.Sha512;
                default:
                    return OtpHashMode.Sha256;
            }
        }

        public tbTwoFactorAuthModel EncryptModel(tbTwoFactorAuthModel model)
        {
            model.Name = Encrypt(model.Name, GetCurrentServer().Key);
            model.SecretKey = Encrypt(model.SecretKey, GetCurrentServer().Key);
            return model;
        }

        public tbTwoFactorAuthModel DecryptModel(tbTwoFactorAuthModel model)
        {
            model.Name = Decrypt(model.Name, GetCurrentServer().Key);
            model.SecretKey = Decrypt(model.SecretKey, GetCurrentServer().Key);
            return model;
        }
    }
}
