using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.SharePoint.Client;
using eAccounting.Models;
using eAccounting.Utils;

namespace eAccounting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {        
        private readonly IOptions<SharePointConfiguration> _sharepointConfiguration;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

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
            var aadAppId = _sharepointConfiguration.Value.AADAppId;
            Uri site = new Uri(_sharepointConfiguration.Value.SiteUrl);
            string user = _sharepointConfiguration.Value.Username;
            string password = _sharepointConfiguration.Value.Password;
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

        private readonly ILogger<StatusController> _logger;

        public StatusController(ILogger<StatusController> logger, IOptions<SharePointConfiguration> sharepointConfiguration)
        {
            _logger = logger;
            _sharepointConfiguration = sharepointConfiguration;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(Summaries);
        }

        // GET api/status/sharepoint
        [HttpGet("sharepoint")]
        public IActionResult GetSharePoint()
        {
            try
            {
                var result = ConnectSharePoint();
                return Ok(new { Code = "200", Message = "Success", Data = result });

            }
            catch (Exception ex)
            {
                return Ok(new { Code = "500", Message = "Error happening", Data = ex });
            }
        }
    }
}
