using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public class RegisterViewModel
    {

        public int userId { get; set; }

        [Required(ErrorMessage = "Name can't be blank")]
        [Display(Name = "Tell us your name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Name can't be blank")]
        [Display(Name = "Tell us your name")]
        //public string Lastname { get; set; }
        public string LastnameF { get; set; }
        public string LastnameM { get; set; }


        public int? IdPadron { get; set; }
        public int roleID { get; set; }

        public string IP {  get; set; }


        [Required(ErrorMessage = "E-mail can't be blank")]
        [Display(Name = "Enter your E-mail address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password can't be blank")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Enter a password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Country code can't be blank")]
        [Display(Name = "Country code")]
        public string CountryCode { get; set; }

        [Required(ErrorMessage = "Phone number can't be blank")]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        public bool isActive { get; set; }  
        public bool removePassword { get; set; }

        public string Lastname { get; set; }

        // CONTROL
        public string error { get; set; }
        public bool success { get; set; }
    }
}
