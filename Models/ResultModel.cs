namespace OnlyBelaSemafor.Models;

public class ResultModel
{
    private readonly TeamModel _teamOne;
    private readonly TeamModel _teamTwo;
	
    private int GameScore => _teamOne.Call + _teamTwo.Call + _teamOne.Bela + _teamTwo.Bela + 162;
	
    public int TeamOneGameScore { get; private set; }
    public int TeamTwoGameScore { get; private set; }
	
    public ResultModel(TeamModel teamOne, TeamModel teamTwo)
    {
        _teamOne = teamOne;
        _teamTwo = teamTwo;
		
        TeamOneGameScore = _teamOne.Score + _teamOne.Call + _teamOne.Bela;
        TeamTwoGameScore = _teamTwo.Score + _teamTwo.Call + _teamTwo.Bela;
    }
	
    public void CalculateResult()
    {
        CheckFall();
        CheckStilja();
    }
	
    private void CheckFall()
    {
        if (_teamOne.IsCalling && TeamOneGameScore > 0 && TeamOneGameScore < GameScore / 2)
        {
            TeamOneGameScore = 0;
            TeamTwoGameScore = GameScore;
        }
		
        if (_teamTwo.IsCalling && TeamOneGameScore > 0 && TeamTwoGameScore < GameScore / 2)
        {
            TeamOneGameScore = GameScore;
            TeamTwoGameScore = 0;                
        }
    }
    private void CheckStilja()
    {
        if (_teamOne.IsStilja)
        {
            TeamOneGameScore = GameScore + 90;
            TeamTwoGameScore = 0;                
        }
		
        if (_teamTwo.IsStilja)
        {
            TeamOneGameScore = 0;
            TeamTwoGameScore = GameScore + 90;
        }
    }	
}