using System.Collections.Generic;

namespace EShop.ViewModels.Users.WebApi
{
    public class UserToBuildJwtTokenViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
    }
}