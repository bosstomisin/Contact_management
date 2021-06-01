using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sq007.DTO
{
    public class PaginationFilter
    {

        public int CurrentPage { get; set; }

        public PaginationFilter()
        {

            this.CurrentPage = 1;

        }
        public PaginationFilter(int _CurrentPage)
        {

            this.CurrentPage = _CurrentPage <= 0 ? 1 : _CurrentPage;
        }
    }
}
