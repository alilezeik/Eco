namespace Eco
{
    using System;

    public class User
    {
        public string UserName
        {
            get;
            set;
        }

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

        public DateTimeOffset CreatedDate
        {
            get;
            set;
        }

        public DateTimeOffset ModifiedDate
        {
            get;
            set;
        }
    }
}
