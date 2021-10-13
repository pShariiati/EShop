using System.Collections.Generic;

namespace EShop.ViewModels.Users.WebApi
{
    public class AddUserViewModel
    {
        public string UserName { get; set; }

        public string FullName { get; set; }

        public string Password { get; set; }
        public List<string> Roles { get; set; }
    }
}