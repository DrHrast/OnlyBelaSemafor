using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
        readonly GameModel _game;
        string defaultValueForScoreKey = "1001";
        string defaultValueForModeKey = "Light";

        public DatabaseManager()
        {
            database = new SQLiteConnection(dbPath);
            database.CreateTable<ResultModel>();
            database.CreateTable<SettingsModel>();

            var settingsCount = database.Table<SettingsModel>().Count();
            if (settingsCount == 0)
            {
                // If table is empty, insert the initial row
                database.Insert(new SettingsModel
                {
                    Key = "scoreValue",
                    Value = "1001"
                });

                database.Insert(new SettingsModel
                {
                    Key = "modeValue",
                    Value = "Light"
                });
            }
        }

        //******************//
        //      RESULT      //
        //******************//

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

        public bool IsEmpty()
        {
            return database.Table<ResultModel>().Count() == 0;
        }

        public ResultModel GetTeam(int id)
        {
            return database.Get<ResultModel>(id);
        }

        public List<ResultModel> GetTeams()
        {
            return database.Table<ResultModel>().ToList();
        }

        public List<ResultModel> GetTeamsDesc()
        {
            return database.Table<ResultModel>().OrderByDescending(t => t.Id).ToList();
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

        public void SetLastTeamName(int id)
        {
            var lastTeam = LastRow();
            if (id == 0)
            {
                lastTeam.team1Name = _game.TeamOneName.ToString();
            }
            else if (id == 1)
            {
                lastTeam.team2Name = _game.TeamTwoName.ToString();
            }
        }

        public List<int> GetLastTotal()
        {
            var lastTotal = LastRow();
            if (lastTotal == null) { return null; }
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
            database.DropTable<ResultModel>();
            database.CreateTable<ResultModel>();
        }

        //******************//
        //     SETTINGS     //
        //******************//

        //public void UpdateValueByKey(string key, string newValue)
        //{
        //    //var entry = database.Table<SettingsModel>().FirstOrDefault(x => x.Key.Equals(key)).Value = newValue;

        //    //if (entry != null)
        //    //{
        //    //    database.Update(entry);
        //    //}
        //}

        public SettingsModel GetModelByKey(string key)
        {
            return database.Table<SettingsModel>().FirstOrDefault(s => s.Key == key);
        }

        public int GetIntScoreValue()
        {
            string key = "scoreValue";
            return int.Parse(database.Table<SettingsModel>().FirstOrDefault(s => s.Key == key).Value);
        }

        public bool IsDarkModeOn()
        {
            string key = "modeValue";
            return database.Table<SettingsModel>().FirstOrDefault(s => s.Key == key).Value == "Light" ? false : true;
        }

        public void UpdateScoreValue(string score)
        {
            var prevValue = database.Table<SettingsModel>().FirstOrDefault(x => x.Key == "scoreValue");
            if (prevValue != null)
            {
                prevValue.Value = score;
                database.Update(prevValue);
            }
        }

        public void UpdateModeValue(string mode)
        {
            var prevValue = database.Table<SettingsModel>().FirstOrDefault(x => x.Key == "modeValue");
            if (prevValue != null)
            {
                prevValue.Value = mode;
                database.Update(prevValue);
            }
        }

        public void RevertToDefaultSettings()
        {
            UpdateScoreValue(defaultValueForScoreKey);
            UpdateModeValue(defaultValueForModeKey);
        }
    }
}
