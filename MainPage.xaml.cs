﻿using OnlyBelaSemafor.Models;

namespace OnlyBelaSemafor
{
    public partial class MainPage : ContentPage
    {

        //******************//
        //     VARIABLES    //
        //******************//

        //Enum of score steps
        private enum UpToPoint
        {
            Highest = 1301,
            High = 1001,
            Medium = 701,
            Low = 501,
            Lowest = 301
        }

        private const int GAME = 162;
        private int points;
        private string nameOfTheFirstTeam = "TeamOne";
        private string nameOfTheSecondTeam = "TeamTwo";
        private bool teamThatCalled;
        private bool isTeamCallChecked;

        //******************//
        //      METHODS     //
        //******************//
        private void TeamNameSetter()
        {
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
                    team1Score = string.IsNullOrEmpty(teamOneScoreEntry.Text) ? 0 : int.Parse(teamOneScoreEntry.Text),
                    team2Score = string.IsNullOrEmpty(teamTwoScoreEntry.Text) ? 0 : int.Parse(teamTwoScoreEntry.Text),
                    team1Bela = teamOneBelaCheck.IsChecked == true ? 20 : 0,
                    team2Bela = teamTwoBelaCheck.IsChecked == true ? 20 : 0,
                    team1Call = string.IsNullOrEmpty(teamOneScoreEntry.Text) ? 0 : int.Parse(teamOneScoreEntry.Text),
                    team2Call = string.IsNullOrEmpty(teamTwoCallEntry.Text) ? 0 : int.Parse(teamTwoCallEntry.Text)
                };

                //ResultOutput(result);
                ClearingInputs();
            }
        }

        //******************//
        //    NAVIGATION    //
        //******************//

        //******************//
        //       MAIN       //
        //******************//
        public MainPage()
        {
            InitializeComponent();
            points = ((int)UpToPoint.High);
            TeamNameSetter();
        }
    }
}