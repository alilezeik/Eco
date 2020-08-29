namespace Eco
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json.Serialization;

    public class CreateAccountRequest
    {
        [Required]
        public string UserName
        {
            get;
            set;
        }

        [Required]
        public string Password
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

        [Required]
        public string PhoneNumber
        {
            get;
            set;
        }

        //[Required]
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

        [JsonIgnore]
        public int? StartupId
        {
            get;
            set;
        }
    }
}
