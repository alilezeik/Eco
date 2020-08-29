namespace Eco
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class PageCollection<T> where T : class
    {
        public IEnumerable<T> Items { get; set; }

        public int TotalCount { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }
    }
}
