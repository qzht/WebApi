using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Result<T> where T : class
    {
        public string success { get; set; }
        public string msg { get; set; }
        public IEnumerable<T> content { get; set; }
    }
}
