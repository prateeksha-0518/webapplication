using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpfcurd.Models
{
    public class Pagination
    {
        public List<Student> Students { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
    }
}
