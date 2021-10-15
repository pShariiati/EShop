using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.ViewModels.TestWebApi
{
    public class ShowUserViewModel
    {
        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }

        [Display(Name = "نام و نام خانوادگی")]
        public string FullName { get; set; }

        [Display(Name = "عکس کاربر")]
        public string Avatar { get; set; }
    }
}
