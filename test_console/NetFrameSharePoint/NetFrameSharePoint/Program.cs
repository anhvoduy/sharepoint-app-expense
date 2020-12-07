using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;

namespace NetFrameSharePoint
{
    class Program
    {
        static void Main(string[] args)
        {
            string siteUrl = "https://domain.sharepoint.com/";

            //Get the realm for the URL
            string realm = TokenHelper.GetRealmFromTargetUrl(new Uri(siteUrl));

            //Get the access token for the URL.  
            string accessToken = TokenHelper.GetAppOnlyAccessToken(TokenHelper.SharePointPrincipal, new Uri(siteUrl).Authority, realm).AccessToken;

            //Create a client context object based on the retrieved access token
            using (ClientContext cc = TokenHelper.GetClientContextWithAccessToken(siteUrl, accessToken))
            {
                cc.Load(cc.Web, p => p.Title, p => p.Url);
                cc.ExecuteQuery();
                Console.WriteLine(cc.Web.Title);
                Console.WriteLine(cc.Web.Url);
                Console.ReadLine();
            }
        }
    }
}
