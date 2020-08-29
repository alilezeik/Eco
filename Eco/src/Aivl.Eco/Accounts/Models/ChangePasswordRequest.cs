namespace Eco
{
    using System.ComponentModel.DataAnnotations;

    public class ChangePasswordRequest
    {
        [Required]
        public string OldPassword
        {
            get;
            set;
        }

        [Required]
        public string NewPassword
        {
            get;
            set;
        }
    }
}
