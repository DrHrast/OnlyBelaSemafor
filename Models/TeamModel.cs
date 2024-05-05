using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlyBelaSemafor.Models
{
    internal class TeamModel
    {
        [PrimaryKey, AutoIncrement]
        int id {  get; set; }
        string name { get; set; }
        int call {  get; set; }
        int bela { get; set; }
        int score { get; set; }
        int totalScore { get; set; }
    }
}
