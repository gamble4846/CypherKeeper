using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.IO;
using Microsoft.AspNetCore.Http;
using IniParser;
using IniParser.Model;
using System.Data;
using System.Reflection;
using AuthLayer.Model;

namespace AuthLayer.Utility
{
    public class CommonFunctions
    {
        public IConfiguration Configuration { get; }
        public string ContentRootPath { get; set; }
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public CommonFunctions(IConfiguration configuration, string ContentRootPath, IHttpContextAccessor httpContextAccessor)
        {
            Configuration = configuration;

            if (!String.IsNullOrEmpty(ContentRootPath))
            {
                var pathArray = ContentRootPath.Split("\\").ToList();
                pathArray.RemoveRange(pathArray.Count - 1, 1);
                this.ContentRootPath = String.Join("\\", pathArray) + "\\";
            }

            HttpContextAccessor = httpContextAccessor;
        }

        public CommonFunctions()
        {

        }

        public static bool VerifyPassword(string Password, string PasswordHash)
        {
            if (string.IsNullOrEmpty(Password) && string.IsNullOrEmpty(PasswordHash))
                // Blank password hash matches blank password
                return true;

            if (!PasswordHash.StartsWith("$MD5$"))
                throw new Exception("Unknown password hash algorithm");

            if (PasswordHash.Length != 5 + 32 + 32)
                // Hashed password should be "$MD5$" + 32 character of salt + 32 character hash
                throw new Exception("Invalid hashed password format");

            var Salt = PasswordHash.Substring(5, 32);
            var ActualHash = PasswordHash.Substring(37, 32);

            string ComputedHash;

            using (MD5CryptoServiceProvider MD5HashObject = new MD5CryptoServiceProvider())
            {
                UnicodeEncoding Encoder = new UnicodeEncoding(false, false);
                var Bytes = MD5HashObject.ComputeHash(Encoder.GetBytes(Salt + Password.ToLowerInvariant()));
                ComputedHash = BitConverter.ToString(Bytes).Replace("-", "");
            }

            return ActualHash.Equals(ComputedHash, StringComparison.InvariantCultureIgnoreCase);
        }

        public dynamic GetTokenData(string token)
        {
            try
            {
                token = token.Replace("Bearer ", "");
                var jwtSection = Configuration.GetSection("Jwt");
                var Secret = Encoding.ASCII.GetBytes(jwtSection.GetValue<String>("Secret"));
                var ValidIssuer = jwtSection.GetValue<String>("ValidIssuer");
                var ValidAudience = jwtSection.GetValue<String>("ValidAudience");
                var handler = new JwtSecurityTokenHandler();
                var validations = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Secret),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                var claims = handler.ValidateToken(token, validations, out var tokenSecure);
                var otherClaims = claims.Identities.ToList()[0].Claims.ToList();
                var userId = otherClaims.Find(x => x.Type == "Sid").Value;
                var CompanyData = Newtonsoft.Json.JsonConvert.DeserializeObject<CompanyListValueModel>(otherClaims.Find(x => x.Type == "SCompany").Value);
                var password = otherClaims.Find(x => x.Type == "SPass").Value;

                dynamic returnVar = new System.Dynamic.ExpandoObject();
                returnVar.userId = userId;
                returnVar.CompanyData = CompanyData;
                returnVar.password = password;

                return returnVar;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public CompanyData GetINIFilesData()
        {
            var CustomPathsSection = Configuration.GetSection("CustomPaths");
            var accFilePath = Path.Combine(ContentRootPath, CustomPathsSection.GetValue<String>("accINIFile"));
            var serversFilePath = Path.Combine(ContentRootPath, CustomPathsSection.GetValue<String>("serversINIFile"));
            var parser = new FileIniDataParser();
            IniData accData = parser.ReadFile(accFilePath);
            IniData serverData = parser.ReadFile(serversFilePath);

            var CompanyData = new CompanyData()
            {
                CompaniesList = accData["Company"].ToList().Select(x => x.KeyName).ToList(),
                CompaniesListWithValue = accData["Company"].ToList().Select(x => new CompanyListValueModel(x.KeyName, x.Value)).ToList(),
                Version = accData["Version"]["Version"],
                Feedback = accData["Options"]["Feedback"],
                NoTourPrompt = accData["Options"]["NoTourPrompt"],
                WebServiceURL = accData["Mobile"]["WebServiceURL"],
                servers = serverData["Database Server"].ToList().Select(x => new iniServersModel(x.KeyName, x.Value.Split(",")[0], x.Value.Split(",")[1], x.Value.Split(",")[2], x.Value.Split(",")[3])).ToList()
            };

            return CompanyData;
        }

        public string getConnectionStringFromDatabaseIndex(int index)
        {
            var companyData = GetINIFilesData();
            var currentCompany = companyData.CompaniesListWithValue[index];
            var currentServer = companyData.servers.Find(x => x.Key == currentCompany.Data.Split(",")[1]);
            var connectionString = "Server=" + currentServer.Address + ";Database=" + currentCompany.Data.Split(",")[2] + ";User Id=" + currentServer.Username + ";Password=" + currentServer.Password + ";" + "Max Pool Size=50000;Pooling=True;TrustServerCertificate=True;MultipleActiveResultSets=true;";
            return connectionString;
        }

        public int getDatabaseIndexFromServerKey(string serverKey)
        {
            var INIFilesData = GetINIFilesData();
            return INIFilesData.CompaniesListWithValue.FindIndex(x => (Convert.ToString(x.Data)).Split(',')[2] == serverKey);
        }

        public string GetNewConnectionString(string accessTokenParameter = "")
        {
            try
            { 
                var accessToken = "";
                if (string.IsNullOrEmpty(accessTokenParameter))
                {
                    accessToken = HttpContextAccessor.HttpContext.Request.Headers["Authorization"];
                }
                else
                {
                    accessToken = accessTokenParameter;
                }
                 
                var tokenData = GetTokenData(accessToken);
                if (tokenData == null)
                {
                    return null;
                }
                var connectionString = getConnectionStringFromDatabaseIndex(getDatabaseIndexFromServerKey((Convert.ToString(tokenData.CompanyData.Data)).Split(',')[2]));
                return connectionString;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }

            dataTable.Columns.Add("IndexCommonFunctions");


            for (int j = 0; j < items.Count(); j++)
            {
                var item = items[j];
                var values = new object[Props.Length + 1];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                values[Props.Length] = j;
                dataTable.Rows.Add(values);
            }

            //put a breakpoint here and check datatable
            return dataTable;
        }

        public string Encrypt(string clearText)
        {
            var jwtSection = Configuration.GetSection("Jwt");
            string EncryptionKey = jwtSection.GetValue<String>("Secret");
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

        public dynamic Decrypt(string cipherText)
        {
            var jwtSection = Configuration.GetSection("Jwt");
            string EncryptionKey = jwtSection.GetValue<String>("Secret");
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
            return Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(cipherText);
        }

        public static T CreateDeepCopy<T>(T obj)
        {
            var stringObj = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            var returnObect = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(stringObj);
            return returnObect;
        }
    }
}
