using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;
using KanbanLight.Models;
using KanbanLight.Providers;

namespace KanbanLight
{
	public partial class Startup
	{
		// Anwendung für die Verwendung von "OAuthAuthorization" aktivieren. Anschließend können Sie Ihre Web-APIs sichern.
		static Startup()
		{
			PublicClientId = "web";

			OAuthOptions = new OAuthAuthorizationServerOptions
			{
				TokenEndpointPath = new PathString("/Token"),
				AuthorizeEndpointPath = new PathString("/Account/Authorize"),
				Provider = new ApplicationOAuthProvider(PublicClientId),
				AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
				AllowInsecureHttp = true
			};
		}

		public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

		public static string PublicClientId { get; private set; }

		// Weitere Informationen zum Konfigurieren der Authentifizierung finden Sie unter "http://go.microsoft.com/fwlink/?LinkId=301864".
		public void ConfigureAuth(IAppBuilder app)
		{
			// Konfigurieren des db-Kontexts und des Benutzer-Managers für die Verwendung einer einzelnen Instanz pro Anforderung.
			app.CreatePerOwinContext(ApplicationDbContext.Create);
			app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

			// Anwendung für die Verwendung eines Cookies zum Speichern von Informationen für den angemeldeten Benutzer aktivieren
			app.UseCookieAuthentication(new CookieAuthenticationOptions
			{
				AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
				LoginPath = new PathString("/Account/Login"),
				Provider = new CookieAuthenticationProvider
				{
					OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
						validateInterval: TimeSpan.FromMinutes(20),
						regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
				}
			});
			// Ein Cookie zum vorübergehenden Speichern von Informationen zu einem Benutzer verwenden, der sich mit dem Anmeldeanbieter eines Drittanbieters anmeldet
			app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

			// Anwendung für die Verwendung eines Trägertokens zum Authentifizieren von Benutzern aktivieren
			app.UseOAuthBearerTokens(OAuthOptions);

			// Auskommentierung der folgenden Zeilen aufheben, um die Anmeldung mit Anmeldeanbietern von Drittanbietern zu ermöglichen
			//app.UseMicrosoftAccountAuthentication(
			//    clientId: "",
			//    clientSecret: "");

			//app.UseTwitterAuthentication(
			//    consumerKey: "",
			//    consumerSecret: "");

			//app.UseFacebookAuthentication(
			//    appId: "",
			//    appSecret: "");

			//app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
			//{
			//    ClientId = "",
			//    ClientSecret = ""
			//});
		}
	}
}
