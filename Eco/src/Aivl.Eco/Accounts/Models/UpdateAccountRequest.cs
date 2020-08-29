using System.ComponentModel.DataAnnotations;

namespace Eco
{
    public class UpdateAccountRequest
    {
        [Required]
        public string UserName
        {
            get;
            set;
        }

        [Required]
        public string Email
        {
            get;
            set;
        }

        public string PhoneNumber
        {
            get;
            set;
        }

        [Required]
        public string FullName
        {
            get;
            set;
        }

        public string ProfileLink
        {
            get;
            set;
        }
    }
}
