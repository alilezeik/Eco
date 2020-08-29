namespace Eco
{
    using System;
    using System.Collections.Generic;

    public class LoginResponse
    {
        public object Token
        {
            get;
            set;
        }

        public IEnumerable<string> Roles
        {
            get;
            set;
        }
    }
}
