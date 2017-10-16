using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Project
    {
        public Guid ProID { get; set; }
        public string ProName { get; set; }
        public string ProSub { get; set; }
        [PetaPoco.ResultColumn]
        public DateTime CDate { get; set; }
        public int UserID { get; set; }
        public int IsFlag { get; set; }
        public int IsImport { get; set; }
        public int IsMessage { get; set; }

    }
}
