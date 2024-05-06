using SQLite;

namespace OnlyBelaSemafor.Models;

public class ScoreModel
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int Team1 { get; set; }
    public int Team2 { get; set; }
}