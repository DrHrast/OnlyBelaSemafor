using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlyBelaSemafor.Models;
using SQLite;

namespace OnlyBelaSemafor
{
    public class DatabaseManager
    {
        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "rezultsDb.db3");
        readonly SQLiteConnection database;

        public DatabaseManager()
        {
            database = new SQLiteConnection(dbPath);
            database.CreateTable<ResultModel>();
        }

        public void AddTeam(ResultModel result)
        {
            database.Insert(result);
        }

        public void UpdateTeam(ResultModel result)
        {
            database.Update(result);
        }

        public void DeleteTeam(int id)
        {
            database.Delete<ResultModel>(id);
        }

        public int Size()
        {
            var temp = GetTeams();
            return temp.Count();
        }

        public ResultModel GetTeam(int id)
        {
            return database.Get<ResultModel>(id);
        }

        public List<ResultModel> GetTeams()
        {
            return database.Table<ResultModel>().ToList();
        }

        public ResultModel LastRow()
        {
            return database.Table<ResultModel>().OrderByDescending(team => team.Id).FirstOrDefault();
        }

        public string GetLastTeamName(int id) 
        { 
            var lastTeam = LastRow(); 
            return id == 0 ? lastTeam?.team1Name ?? string.Empty : lastTeam?.team2Name ?? string.Empty; 
        }

        public List<int> GetLastTotal()
        {
            var lastTotal = LastRow();
            List<int> rez = new List<int>() {
                lastTotal.team1TotalScore,
                lastTotal.team2TotalScore
            };
            return rez;
        }

        public int GetLastId()
        {
            return LastRow().Id;
        }

        public void DeleteLastRowById(int id)
        {
            database.Delete<ResultModel>(id);
        }

        //DID_IT: This does not clear database
        public void ClearDb()
        {
            database.Execute("DELETE FROM ResultModel");
        }
    }
}
