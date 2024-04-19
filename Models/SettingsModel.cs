using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlyBelaSemafor.Models
{
    public class SettingsModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int victoryScore { get; set; } 
        public bool darkLayoutMode { get; set; }
    }
}
