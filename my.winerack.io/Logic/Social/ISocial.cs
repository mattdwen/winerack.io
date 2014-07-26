using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using winerack.Models;

namespace winerack.Logic.Social {

	interface ISocial {

		string GetAuthorizationUrl();

		Credentials ProcessAccessToken(string verifier, string userId);
	}
}
