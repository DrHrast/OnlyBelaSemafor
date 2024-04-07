using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlyBelaSemafor.Models
{
    internal class ResultModel
    {
        //DID_IT 2.1: Make team1 score, team 2 score
        //DID_IT 2.2: Make "call" variables for bouth teams
        //DID_IT 2.3: Make "bela" variables for bouth teams

        public int team1Score { get; set; }
        public int team2Score { get; set; }
        public int team1Call { get; set; }
        public int team2Call { get; set; }
        public int team1Bela { get; set; }
        public int team2Bela { get; set; }
        //public int team1Stilja { get; set;}
        //public int team2Stilja { get; set;}
    }
}
