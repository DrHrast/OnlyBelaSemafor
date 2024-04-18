﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlyBelaSemafor.Models;
using SQLite;

namespace OnlyBelaSemafor
{
    public class DatabaseManager
    {
        readonly SQLiteConnection database;

        public DatabaseManager(string dbPath)
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

        public ResultModel GetTeam(int id)
        {
            return database.Get<ResultModel>(id);
        }

        public List<ResultModel> GetTeams()
        {
            return database.Table<ResultModel>().ToList();
        }
    }
}
