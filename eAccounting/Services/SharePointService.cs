using System;
using System.Security;
using System.Threading.Tasks;
using eAccounting.Utils;

namespace eAccounting.Services
{
    public interface ISharePointService
    {
        SecureString GetSecureString(string plainString);

        Task<object> ConnectSharePoint();
    }

    public class SharePointService : ISharePointService
    {
        private string AADAppId { get; set; }

        private Uri SiteUrl { get; set; }

        private string Username { get; set; }

        private string Password { get; set; }

        public SharePointService(string aaDAppId, string siteUrl, string username, string password)
        {
            this.AADAppId = aaDAppId;
            this.SiteUrl = new Uri(siteUrl);
            this.Username = username;
            this.Password = password;
        }

        public SecureString GetSecureString(string plainString)
        {
            if (plainString == null) return null;

            SecureString secureString = new SecureString();
            foreach (char c in plainString.ToCharArray())
            {
                secureString.AppendChar(c);
            }
            return secureString;
        }

        public async Task<object> ConnectSharePoint()
        {
            var aadAppId = this.AADAppId;
            Uri site = this.SiteUrl;
            string user = this.Username;
            string password = this.Password;
            string web_title = "";
            SecureString securePassword = GetSecureString(password);

            // Note: The PnP Sites Core AuthenticationManager class also supports this
            using (var authenticationManager = new AuthenticationManager(aadAppId))
            using (var context = authenticationManager.GetContext(site, user, securePassword))
            {
                context.Load(context.Web, p => p.Title);
                await context.ExecuteQueryAsync();
                Console.WriteLine($"Title: {context.Web.Title}");
                web_title = context.Web.Title;
            }
            return new { web_title };
        }
    }
}
