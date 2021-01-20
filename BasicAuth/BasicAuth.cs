using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace BasicAuth
{
    /// <summary>
    /// This middleware performs basic authentication.
    /// For details on basic authentication see RFC 2617.
    /// Based on the work by DevBridge (https://github.com/devbridge/) and Mukesh Murugan (https://github.com/iammukeshm/webapi-basic-authentication-middleware-asp.net-core-3.1)
    ///
    /// The basic operational flow is:
    ///
    /// On AuthenticateRequest:
    ///     extract the basic authentication credentials
    ///     verify the credentials
    ///     if successfull, create and send authentication cookie
    ///
    /// On SendResponseHeaders:
    ///     if there is no authentication cookie in request, clear response, add unauthorized status code (401) and
    ///     add the basic authentication challenge to trigger basic authentication.
    /// </summary>

    public class BasicAuth
    {
        /// <summary>
        /// HTTP1.1 Authorization header
        /// </summary>
        public const string HttpAuthorizationHeader = "Authorization";

        /// <summary>
        /// HTTP1.1 Basic Challenge Scheme Name
        /// </summary>
        public const string HttpBasicSchemeName = "Basic"; //

        /// <summary>
        /// HTTP1.1 Credential username and password separator
        /// </summary>
        public const char HttpCredentialSeparator = ':';

        /// <summary>
        /// HTTP1.1 Basic Challenge Scheme Name
        /// </summary>
        public const string HttpWwwAuthenticateHeader = "WWW-Authenticate";


        /// <summary>
        /// The credentials that are allowed to access the site.
        /// </summary>
        private Dictionary<string, string> activeUsers;

        /// <summary>
        /// Required for asp.net core middleware to function right
        /// </summary>
        private readonly RequestDelegate next;
        private readonly string realm;

        /// <summary>
        /// Default Constructor (the RequestDelegate is required by the middleware API)
        /// </summary>
        public BasicAuth(RequestDelegate next, string realm, string basicAuthUserJson)
        {
            this.next = next;
            this.realm = realm;
            //deserialize the valid users from the user.json file
            var packageJson = File.ReadAllText(basicAuthUserJson);
            activeUsers = JsonSerializer.Deserialize<Dictionary<string, string>>(packageJson);
        }


        /// <summary>
        /// Class entry point called by the middleware pipeline
        /// </summary>
        public async Task Invoke(HttpContext context)
        {
            string authHeader = context.Request.Headers[HttpAuthorizationHeader];
            
            if (authHeader != null && authHeader.StartsWith(HttpBasicSchemeName))
            {
                // Get the encoded username and password
                var encodedUsernamePassword = authHeader.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries)[1]?.Trim();
                
                // Decode from Base64 to string
                var decodedUsernamePassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));
                
                // Split username and password
                var username = decodedUsernamePassword.Split(':', 2)[0];
                var password = decodedUsernamePassword.Split(':', 2)[1];

                // Check if login is correct
                if (IsAuthorized(username, password))
                {
                    //if it's good, pass them on to the next middleware step in the pipeline
                    await next.Invoke(context);
                    return;
                }
            }

            // Return authentication type (causes browser to show login dialog)
            context.Response.Headers[HttpWwwAuthenticateHeader] = HttpBasicSchemeName;
            // Add realm if it is not null
            if (!string.IsNullOrWhiteSpace(realm))
            {
                context.Response.Headers[HttpWwwAuthenticateHeader] += $" realm=\"{realm}\"";
            }
            // Return unauthorized
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }

        public bool IsAuthorized(string username, string password)
        {
            // Check that username and password are correct
            string lowerCaseUserName = username.ToLower();

            if (activeUsers.ContainsKey(username) && activeUsers[username] == password)
            {
                return true;
            }
            else
                return false;
        }
    }
}