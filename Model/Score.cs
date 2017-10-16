using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
   public  class Score
    {
       public int ID { get; set; }
       public string XProjectId { get; set; }
       public string StuId { get; set; }
       public string subName { get; set; }
       public float score { get; set; }
       public float classAvg { get; set; }
       public string classPos { get; set; }
    }
}
