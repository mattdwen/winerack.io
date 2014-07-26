using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace winerack.Models.Social.Tumblr {

	public class Meta {
		public int status { get; set; }

		public string msg { get; set; }
	}

	public class BaseResponse {
		public Meta meta { get; set; }
	}

	public class UserResponse : BaseResponse {
		public UserResponseReponse response { get; set; }
	}

	public class UserResponseReponse {
		public User user { get; set; }
	}

	public class User {

		public string name { get; set; }

		public List<Blog> blogs { get; set; }
	}

	public class Blog {
		public string name { get; set; }
	}
}