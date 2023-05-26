using System.Collections.Generic;

namespace AuthLayer.Model
{
    public class CompanyData
    {
        public List<string> CompaniesList { get; set; }
        public List<CompanyListValueModel> CompaniesListWithValue { get; set; }
        public string Version { get; set; }
        public string Feedback { get; set; }
        public string NoTourPrompt { get; set; }
        public string WebServiceURL { get; set; }
        public List<iniServersModel> servers { get; set; }
    }

    public class CompanyListValueModel
    {
        public string Company { get; set; }
        public string Data { get; set; }

        public CompanyListValueModel(string Company, string Data)
        {
            this.Company = Company;
            this.Data = Data;
        }
    }

    public class iniServersModel
    {
        public string Key { get; set; }
        public string Type { get; set; }
        public string Address { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public iniServersModel(string Key, string Type, string Address, string Username, string Password)
        {
            this.Key = Key;
            this.Type = Type;
            this.Address = Address;
            this.Username = Username;
            this.Password = Password;
        }
    }
}
