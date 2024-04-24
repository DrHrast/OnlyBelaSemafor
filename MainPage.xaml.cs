using OnlyBelaSemafor.Models;
using OnlyBelaSemafor.Services;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Internals;

namespace OnlyBelaSemafor
{
    public partial class MainPage : ContentPage
    {

        //******************//
        //     VARIABLES    //
        //******************//
        private DatabaseManager databaseManager;

        //Enum of score steps
        /*private enum UpToPoint
        {
            Highest = 1301,
            High = 1001,
            Medium = 701,
            Low = 501,
            Lowest = 301
        }*/
        List<List<int>> listOfGameResults = new List<List<int>>();
        private const int GAME = 162;
        private int points;
        private string nameOfTheFirstTeam = "TeamOne";
        private string nameOfTheSecondTeam = "TeamTwo";
        private bool teamThatCalled;
        private bool isTeamCallChecked;

        //******************//
        //      METHODS     //
        //******************//

        /*public void SetGameScore(int key)
        //TODO:ERR Probably not needed and also Enum not needed
        {
            switch (key)
            {
                case 0:
                    points = ((int)UpToPoint.Highest);
                    break;
                case 1:
                    points = ((int)UpToPoint.High);
                    break;
                case 2:
                    points = ((int)UpToPoint.Medium);
                    break;
                case 3:
                    points = ((int)UpToPoint.Low);
                    break;
                case 4:
                    points = ((int)UpToPoint.Lowest);
                    break;
            }
            GameScoreSetter();
        }*/
        public void SetTeamNames(string nameOne, string nameTwo)
        {
            nameOfTheFirstTeam = string.IsNullOrEmpty(nameOne) ? nameOfTheFirstTeam : nameOne;
            nameOfTheSecondTeam = string.IsNullOrEmpty(nameTwo) ? nameOfTheSecondTeam : nameTwo;
            TeamNameSetter();
        }
        public void SaveNames()
        {
            nameOfTheFirstTeam = databaseManager.GetLastTeamName(0);
            nameOfTheSecondTeam = databaseManager.GetLastTeamName(1);
            SetTeamNames(nameOfTheFirstTeam, nameOfTheSecondTeam);
        }
        public string GetTeamNames(int id)
        {
            switch(id)
            {
                case 0:
                    return nameOfTheFirstTeam;
                case 1:
                    return nameOfTheSecondTeam;
                default:
                    return string.Empty;
            }
        }
        public void DeleteLastResult()
        {
            if(!IsDbEmpty())
            {
                databaseManager.DeleteLastRowById(databaseManager.GetLastId());
                CheckDb();
                Output();
            }
        }
        public void GameSettingsSetter(string key, string value)
        {
            //If score is higher than points value start new game with that points ceiling.
            //temp.Text = points.ToString();
            databaseManager.UpdateValueByKey(key, value);
            points = databaseManager.GetIntScoreValue();
        }
        public void NewGame()
        {
            SaveNames();
            databaseManager.ClearDb();
            ClearingInputs();
            listOfGameResults.Clear();
            Output();
        }
        public void QuitGame()
        {
            App.Current.Quit();
        }
        public bool GetDbMode()
        {
            return databaseManager.IsDarkModeOn();
        }
        private bool IsDbEmpty()
        {
            return databaseManager.Size() == 0;
        }
        private void CheckDb()
        {
            if (!IsDbEmpty())
            {
                listOfGameResults.Add(databaseManager.GetLastTotal());
            }
        }        
        private void TeamNameSetter()
        {
            //TODO: 1. Make a header
            nameOfTeam1.Content = nameOfTheFirstTeam;
            nameOfTeam2.Content = nameOfTheSecondTeam;
            //team1NameRez.Text = nameOfTheFirstTeam;
            //team2NameRez.Text = nameOfTheSecondTeam;
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
            teamThatCalled = false;
            isTeamCallChecked = false;
        }
        private void ResultOutput(ResultModel result)
        {

            //TODO: 5 Figue out where the score is lost for team two when it's 162 and 0 for team one
            //TODO: 5.1 Team that called doesn't work in favor of team two
            ResultService resultService = new ResultService();
            var temp = new List<List<int>>();
            temp.Add(resultService.SumResults(result));
            bool oneHasBetterScore = temp[0][0] > temp[0][1];
            bool twoHasBetterScore = temp[0][0] < temp[0][1];
            if (isTeamCallChecked == true)
            {
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
            var lastResults = databaseManager.GetTeamsDesc();

            var tableSection = new TableSection();

            foreach (var result in lastResults)
            {
                var grid = new Grid
                {
                    Padding = new Thickness(10),
                    ColumnDefinitions =
                        {
                            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                        }
                };
                //var team1Header = new Label { Text = nameOfTheFirstTeam, VerticalOptions = LayoutOptions.CenterAndExpand };
                //var team2Header = new Label { Text = nameOfTheSecondTeam, VerticalOptions = LayoutOptions.CenterAndExpand };

                //headerGrid.Children.Add(team1Header);
                //headerGrid.Children.Add(team2Header);

                //Grid.SetColumn(team1Header, 0);
                //Grid.SetColumn(team2Header, 1);

                //var headerCell = new ViewCell { View = headerGrid };
                //tableSection.Add(headerCell);

                var team1Label = new Label
                {
                    Text = result.team1TotalScore.ToString(),
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    FontSize = 20
                };
                var team2Label = new Label 
                { 
                    Text = result.team2TotalScore.ToString(), 
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    FontSize = 20
                };

                grid.Children.Add(team1Label);
                grid.Children.Add(team2Label);

                Grid.SetColumn(team1Label, 0);
                Grid.SetColumn(team2Label, 1);

                var viewCell = new ViewCell { View = grid };
                tableSection.Add(viewCell);
            }

            scoreContent.Root.Clear();
            scoreContent.Root.Add(tableSection);
        }       
        private void CheckVictoryConditions()
        {
            List<int> totalScores = new List<int>(databaseManager.GetLastTotal());
            if (totalScores[0] >= points)
            {
                //Team one won
                this.ShowPopup(new VictoryPopup(this, nameOfTheFirstTeam));
            }
            else if (totalScores[1] >= points)
            {
                //Team two won
                this.ShowPopup(new VictoryPopup(this, nameOfTheSecondTeam));
            }
            else { return; }
        }

        //******************//
        //      EVENTS      //
        //******************//
        private void TeamOneScoreEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(teamOneScoreEntry.Text))
            {
                //team1ScoreEntry.Text = "0";
                ClearEntryAndShowPlaceholder(teamOneScoreEntry, "0");
            }
            else if (int.TryParse(teamOneScoreEntry.Text, out int team1Score) && int.Parse(teamOneScoreEntry.Text) <= 162)
            {
                teamTwoScoreEntry.TextChanged -= TeamTwoScoreEntry_TextChanged;
                int team2Score = GAME - team1Score;
                teamTwoScoreEntry.Text = team2Score.ToString();
                teamTwoScoreEntry.TextChanged += TeamTwoScoreEntry_TextChanged;
            }
            else
            {
                ClearEntryAndShowPlaceholder(teamTwoScoreEntry, "0");
                ClearEntryAndShowPlaceholder(teamOneScoreEntry, "0");
            }
        }
        private void TeamTwoScoreEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(teamTwoScoreEntry.Text))
            {
                //team2ScoreEntry.Text = "0";
                ClearEntryAndShowPlaceholder(teamTwoScoreEntry, "0");
            }
            else if (int.TryParse(teamTwoScoreEntry.Text, out int team2Score) && int.Parse(teamTwoScoreEntry.Text) <= 162)
            {
                teamOneScoreEntry.TextChanged -= TeamOneScoreEntry_TextChanged;
                int team1Score = GAME - team2Score;
                teamOneScoreEntry.Text = team1Score.ToString();
                teamOneScoreEntry.TextChanged += TeamOneScoreEntry_TextChanged;
            }
            else
            {
                ClearEntryAndShowPlaceholder(teamOneScoreEntry, "0");
                ClearEntryAndShowPlaceholder(teamTwoScoreEntry, "0");
            }
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
            isTeamCallChecked = true;
            teamThatCalled = true;
        }
        private void TeamTwoRadio_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            isTeamCallChecked = true;
            teamThatCalled = false;
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
                    team1Name = nameOfTheFirstTeam,
                    team2Name = nameOfTheSecondTeam,
                    team1Score = string.IsNullOrEmpty(teamOneScoreEntry.Text) ? 0 : int.Parse(teamOneScoreEntry.Text),
                    team2Score = string.IsNullOrEmpty(teamTwoScoreEntry.Text) ? 0 : int.Parse(teamTwoScoreEntry.Text),
                    team1Bela = teamOneBelaCheck.IsChecked == true ? 20 : 0,
                    team2Bela = teamTwoBelaCheck.IsChecked == true ? 20 : 0,
                    team1Call = string.IsNullOrEmpty(teamOneCallEntry.Text) ? 0 : int.Parse(teamOneCallEntry.Text),
                    team2Call = string.IsNullOrEmpty(teamTwoCallEntry.Text) ? 0 : int.Parse(teamTwoCallEntry.Text)
                };

                ResultOutput(result);
                CheckVictoryConditions();
                ClearingInputs();
            }
        }
        //TODO: 6 Sometimes on pressing new game button it doesn't save previous team names, nor it sets them on default
        private void PlusImageButton_Clicked(object sender, EventArgs e)
        {
            NewGame();

            // todo: implemenet ResetGame method to GameModel
            // game.ResetGame();
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
            this.ShowPopup(new SettingsPopup(this));
        }

        //******************//
        //       MAIN       //
        //******************//
        public MainPage()
        {

            InitializeComponent();

            databaseManager = App.Current.Services.GetRequiredService<DatabaseManager>();
            game = App.Current.Services.GetRequiredService<GameModel>();
            game.PropertyChanged += Game_PropertyChanged;

            ////temp.Text = "1001";
            //var firstName = databaseManager.GetLastTeamName(0);
            //var secondName = databaseManager.GetLastTeamName(1);
            //points = databaseManager.GetIntScoreValue();
            //nameOfTheFirstTeam = string.IsNullOrEmpty(firstName) ? nameOfTheFirstTeam : firstName;
            //nameOfTheSecondTeam = string.IsNullOrEmpty(secondName) ? nameOfTheSecondTeam : secondName;
            //SetTeamNames(nameOfTheFirstTeam, nameOfTheSecondTeam);
            //CheckDb();
            //Output();

            LoadGameData();
        }

        private readonly GameModel game;

        private void Game_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            LoadGameData();
        }
        private void LoadGameData()
        {
            nameOfTeam1.Content = game.TeamOneName;
            nameOfTeam2.Content = game.TeamTwoName;
            points = game.ScoreTarget;
        }
    }
}