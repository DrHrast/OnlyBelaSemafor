using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlyBelaSemafor.Models
{
    public class TeamModel
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public int Call { get; set; }
        public int Bela { get; set; }
        public bool IsCalling { get; set; }
        public bool IsStilja { get; set; }
    }

}
