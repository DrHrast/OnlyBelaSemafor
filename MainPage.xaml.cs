using OnlyBelaSemafor.Models;
using OnlyBelaSemafor.Services;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Internals;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace OnlyBelaSemafor
{
    public partial class MainPage : ContentPage
    {

        public class Result 
        {
            public int Team1 { get; set; }
            public int Team2 { get; set; }
        }

        //******************//
        //     VARIABLES    //
        //******************//
        private DatabaseManager databaseManager;
        private readonly GameModel game;
        private readonly AppSettings appSettings;
        public ObservableCollection<Result> Scores = [];
        private TeamDisplayModel teamOneDisplay = new();
        private TeamDisplayModel teamTwoDisplay = new();
        private const int GAME = 162;
        private int points;

        //******************//
        //      METHODS     //
        //******************//

        public void DeleteLastResult()
        {
            databaseManager.DeleteLastRowById(databaseManager.GetLastId());
            Scores.Remove(Scores.Last());
            //listOfGameResults.Remove(listOfGameResults.Last());
            Output();
        }
        public void NewGame()
        {
            LoadGameData();
            databaseManager.ClearDb();
            ClearingInputs();
            //listOfGameResults.Clear();
            Output();
        }
        public void QuitGame()
        {
            App.Current.Quit();
        }
        private bool IsDbEmpty()
        {
            return databaseManager.Size() == 0;
        }
        private void CheckDb()
        {
            if (!IsDbEmpty())
            {
                game.TeamOneName = databaseManager.GetLastTeamName(0);
                game.TeamTwoName = databaseManager.GetLastTeamName(1);
                points = databaseManager.GetIntScoreValue();
                deleteLastScoreButton.IsEnabled = true;
            }
        }        
        private void LoadGameData()
        {
            points = databaseManager.GetIntScoreValue();
        }
        private void LoadSettingsData()
        {
            appSettings.Theme = databaseManager.GetTheme();
        }
        private void LoadFrontend()
        {
            nameOfTeam1.Content = game.TeamOneName;
            nameOfTeam2.Content = game.TeamTwoName;
            App.Current.UserAppTheme = appSettings.Theme == "Dark" ? AppTheme.Dark : AppTheme.Light;

            //teamOneScoreEntry.Text = teamOneDisplay.Score.ToString();
            //teamOneCallEntry.Text = teamOneDisplay.Call.ToString();
            //teamOneBelaCheck.IsChecked = teamOneDisplay.IsBela;
            //nameOfTeam1.IsChecked = teamOneDisplay.IsCall;
            //cb_TeamOneStilja.IsChecked = teamOneDisplay.IsStilja;

            //teamTwoScoreEntry.Text = teamTwoDisplay.Score.ToString();
            //teamTwoCallEntry.Text = teamTwoDisplay.Call.ToString();
            //teamTwoBelaCheck.IsChecked = teamTwoDisplay.IsBela;
            //nameOfTeam2.IsChecked = teamTwoDisplay.IsCall;
            //cb_TeamTwoStilja.IsChecked = teamTwoDisplay.IsStilja; 
        }
        private void ClearEntryAndShowPlaceholder(Entry entry, string placeholder)
        {
            // Unsubscribe from the TextChanged event to prevent triggering it
            teamOneScoreEntry.TextChanged -= TeamOneScoreEntry_TextChanged;
            teamTwoScoreEntry.TextChanged -= TeamTwoScoreEntry_TextChanged;

            entry.Text = string.Empty;
            entry.Placeholder = placeholder;

            // Subscribe back to the TextChanged event
            teamOneScoreEntry.TextChanged += TeamOneScoreEntry_TextChanged;
            teamTwoScoreEntry.TextChanged += TeamTwoScoreEntry_TextChanged;
        }
        private void ClearingInputs()
        {
            ClearEntryAndShowPlaceholder(teamOneScoreEntry, "0");
            ClearEntryAndShowPlaceholder(teamTwoScoreEntry, "0");
            ClearEntryAndShowPlaceholder(teamOneCallEntry, "0");
            ClearEntryAndShowPlaceholder(teamTwoCallEntry, "0");
            teamOneBelaCheck.IsChecked = false;
            teamTwoBelaCheck.IsChecked = false;
            nameOfTeam1.IsChecked = false;
            nameOfTeam2.IsChecked = false;
            TeamOneStilja.IsVisible = false;
            TeamTwoStilja.IsVisible = false;
        }
        private void CalculateScore(ResultModel result)
        {
            //TODO: 5 Figue out where the score is lost for team two when it's 162 and 0 for team one
            //TODO: 5.1 Team that called doesn't work in favor of team two
            List<List<int>> listOfGameResults = [];
            var lastResult = databaseManager.GetLastTotal();
            if (lastResult is not null)
            {
                listOfGameResults.Add(lastResult);
            }
            ResultService resultService = new ResultService();
            var temp = new List<List<int>>();
            temp.Add(resultService.SumResults(result));
            bool oneHasBetterScore = temp[0][0] > temp[0][1];
            bool twoHasBetterScore = temp[0][0] < temp[0][1];
            var isTeamCallChecked = teamOneDisplay.IsCall || teamTwoDisplay.IsCall;
            if (isTeamCallChecked == true)
            {
                var teamThatCalled = teamOneDisplay.IsCall;
                if (listOfGameResults.Count == 0)
                {
                    if (teamThatCalled && oneHasBetterScore)
                    {
                        listOfGameResults.Add(temp[0]);
                    }
                    else if (teamThatCalled && !oneHasBetterScore)
                    {
                        listOfGameResults.Add([0, temp[0][0] + temp[0][1]]);
                    }
                    else if (!teamThatCalled && !twoHasBetterScore)
                    {
                        listOfGameResults.Add([temp[0][0] + temp[0][1], 0]);
                    }
                    else if (!teamThatCalled && twoHasBetterScore)
                    {
                        listOfGameResults.Add(temp[0]);
                    }
                }
                else
                {
                    int lastIndex = listOfGameResults.Count - 1;
                    if (teamThatCalled && oneHasBetterScore)
                    {
                        listOfGameResults.Add(
                            [temp[0][0] + listOfGameResults[lastIndex][0],
                                temp[0][1] + listOfGameResults[lastIndex][1]]
                        );
                    }
                    else if (teamThatCalled && !oneHasBetterScore)
                    {
                        listOfGameResults.Add(
                            [0 + listOfGameResults[lastIndex][0],
                                temp[0][0] + temp[0][1] + listOfGameResults[lastIndex][1]]);
                    }
                    else if (!teamThatCalled && !twoHasBetterScore)
                    {
                        listOfGameResults.Add(
                            [temp[0][0] + temp[0][1] + listOfGameResults[lastIndex][0],
                                0 + listOfGameResults[lastIndex][1]]);
                    }
                    else if (!teamThatCalled && twoHasBetterScore)
                    {
                        listOfGameResults.Add(
                            [temp[0][0] + listOfGameResults[lastIndex][0],
                                temp[0][1] + listOfGameResults[lastIndex][1]]
                        );
                    }
                }
            }
            else
            {
                if (listOfGameResults.Count == 0)
                {

                    listOfGameResults.Add(temp[0]);
                }
                else
                {
                    int lastIndex = listOfGameResults.Count - 1;
                    listOfGameResults.Add(
                        [temp[0][0] + listOfGameResults[lastIndex][0],
                            temp[0][1] + listOfGameResults[lastIndex][1]]
                    );
                }
            }

            int listsLastIndex = listOfGameResults.Count - 1;
            result.team1TotalScore = listOfGameResults[listsLastIndex][0];
            result.team2TotalScore = listOfGameResults[listsLastIndex][1];
            databaseManager.AddTeam(result);
            Output();
        }
        private void Output()
        {
            Scores.Clear();
            var lastResults = databaseManager.GetTeamsDesc();
            foreach (var result in lastResults)
            {
                Scores.Add( new Result{Team1 = result.team1TotalScore, Team2 = result.team2TotalScore});
            }
        }       
        private void CheckVictoryConditions()
        {
            //List<int> totalScores = new List<int>(databaseManager.GetLastTotal());
            if (Scores.First().Team1 >= points)
            {
                //Team one won
                this.ShowPopup(new VictoryPopup(this, game.TeamOneName));
            }
            else if (Scores.First().Team2 >= points)
            {
                //Team two won
                this.ShowPopup(new VictoryPopup(this, game.TeamTwoName));
            }
            else { return; }
        }
        private void SetStiljaVisibility()
        {
            TeamTwoStilja.IsVisible = teamOneScoreEntry.Text == "0";
            TeamOneStilja.IsVisible = teamTwoScoreEntry.Text == "0";
        }

        //******************//
        //      EVENTS      //
        //******************//
        
        private void TeamScoreEntry_TextChanged(Entry current, Entry other)
        {
            if (string.IsNullOrEmpty(current.Text))
            {
                ClearEntryAndShowPlaceholder(current, "0");
                ClearEntryAndShowPlaceholder(other, "0");
            }
            else if (int.TryParse(current.Text, out int team1Score) && int.Parse(current.Text) <= 162)
            {
                int team2Score = GAME - team1Score;
                other.Text = team2Score.ToString();
            }
            else
            {
                ClearEntryAndShowPlaceholder(other, "0");
                ClearEntryAndShowPlaceholder(current, "0");
            }
            SetStiljaVisibility();
        }
        private void TeamOneScoreEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            teamTwoScoreEntry.TextChanged -= TeamTwoScoreEntry_TextChanged;
            TeamScoreEntry_TextChanged(teamOneScoreEntry, teamTwoScoreEntry);
            teamTwoScoreEntry.TextChanged += TeamTwoScoreEntry_TextChanged;
        }
        private void TeamTwoScoreEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            teamOneScoreEntry.TextChanged -= TeamOneScoreEntry_TextChanged;
            TeamScoreEntry_TextChanged(teamTwoScoreEntry, teamOneScoreEntry);
            teamOneScoreEntry.TextChanged += TeamOneScoreEntry_TextChanged;
        }
        private void TeamOneBelaCheck_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (teamOneBelaCheck.IsChecked) teamTwoBelaCheck.IsChecked = false;
        }
        private void TeamTwoBelaCheck_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (teamTwoBelaCheck.IsChecked) teamOneBelaCheck.IsChecked = false;
        }
        private void TeamOneRadio_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
        }
        private void TeamTwoRadio_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
        }
        private void AddButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(teamOneScoreEntry.Text) && string.IsNullOrEmpty(teamTwoScoreEntry.Text))
            {
                ClearingInputs();
            }
            else
            {
                ResultModel result = new ResultModel()
                {
                    team1Name = game.TeamOneName,
                    team2Name = game.TeamTwoName,
                    team1Score = string.IsNullOrEmpty(teamOneScoreEntry.Text) ? 0 : int.Parse(teamOneScoreEntry.Text),
                    team2Score = string.IsNullOrEmpty(teamTwoScoreEntry.Text) ? 0 : int.Parse(teamTwoScoreEntry.Text),
                    team1Bela = teamOneBelaCheck.IsChecked == true ? 20 : 0,
                    team2Bela = teamTwoBelaCheck.IsChecked == true ? 20 : 0,
                    team1Call = string.IsNullOrEmpty(teamOneCallEntry.Text) ? 0 : int.Parse(teamOneCallEntry.Text),
                    team2Call = string.IsNullOrEmpty(teamTwoCallEntry.Text) ? 0 : int.Parse(teamTwoCallEntry.Text)
                };

                CalculateScore(result);
                CheckVictoryConditions();
                ClearingInputs();
            }
        }
        //DID_IT: 6 Sometimes on pressing new game button it doesn't save previous team names, nor it sets them on default
        private void PlusImageButton_Clicked(object sender, EventArgs e)
        {
            NewGame();

            // todo: implemenet ResetGame method to GameModel
            // game.ResetGame();
        }
        private void MinusImageButton_Clicked(object sender, EventArgs e)
        {
            DeleteLastResult();
        }
        private void Game_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            LoadGameData();
        }
        private void Score_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            deleteLastScoreButton.IsEnabled = Scores.Count > 0;
        }

        //******************//
        //    NAVIGATION    //
        //******************//
        private void HelpImageButton_Clicked(object sender, EventArgs e)
        {
            this.ShowPopup(new HelpPopup());
        }
        private void GearImageButton_Clicked(object sender, EventArgs e)
        {
            this.ShowPopup(new SettingsPopup());
        }

        //******************//
        //       MAIN       //
        //******************//
        public MainPage()
        {
            InitializeComponent();

            databaseManager = App.Current.Services.GetRequiredService<DatabaseManager>();
            game = App.Current.Services.GetRequiredService<GameModel>();
            appSettings = App.Current.Services.GetRequiredService<AppSettings>();
            game.PropertyChanged += Game_PropertyChanged;
            cv_scoreContent.ItemsSource = Scores;    
            
            BindingContext = this;

            LoadSettingsData();
            LoadGameData();
            LoadFrontend();
            CheckDb();
            game.ScoreTarget = points;
            Output();
            Scores.CollectionChanged += Score_CollectionChanged;
        }
    }
}