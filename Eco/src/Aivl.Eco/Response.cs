namespace Eco
{
    using System.Collections.Generic;

    public  class Response<T>
    {
        public T Result
        {
            get;
            set;
        }

        public IEnumerable<string> ErrorCodes
        {
            get;
            set;
        } = new List<string>();
    }
}
