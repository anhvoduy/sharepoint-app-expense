using System;
using System.Security;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;

namespace NetCoreSharePoint
{
    public class Program
    {
        public static SecureString GetSecureString(string plainString)
        {
            if (plainString == null)
                return null;

            SecureString secureString = new SecureString();
            foreach (char c in plainString.ToCharArray())
            {
                secureString.AppendChar(c);
            }
            return secureString;
        }

        public static async Task Main(string[] args)
        {
            Uri site = new Uri("https://domain.sharepoint.com/");
            string user = "xxxxxx@domain.onmicrosoft.com";
            string password = "xxxxxx";
            SecureString securePassword = GetSecureString(password);

            // Note: The PnP Sites Core AuthenticationManager class also supports this
            using (var authenticationManager = new AuthenticationManager())
            using (var context = authenticationManager.GetContext(site, user, securePassword))
            {
                context.Load(context.Web, p => p.Title);
                await context.ExecuteQueryAsync();
                Console.WriteLine($"Title: {context.Web.Title}");
            }
        }
    }
}
