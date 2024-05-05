using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Storage;

namespace OnlyBelaSemafor;

public partial class HelpPopup : Popup
{
    private string _txt = @"	Bela Semafor App User Instructions
Overview:
Bela Semafor is a scoring app designed for keeping track of scores in the Bela card game. It allows users to input scores for two teams, manage calls, and track the Bela points.

Getting Started:
1.  Upon launching the app, you will see the main screen divided into two sections: Header and Body.
2.  In the Header section, you'll find the app title and a menu button for accessing additional options.
3.  The Body section consists of input fields for entering data related to the game.

Input Data:
1.  Team Selection:
    -Each team has a radio button associated with it. Select the appropriate radio button to indicate the team you are entering scores for.
2.  Score Entry:
    -Enter the score achieved by the selected team in the provided entry field labeled ""Rezultat"". Only numeric values up to 162 are allowed.
3.  Call Entry:
    -If the team made any calls during the game, enter the number of calls in the provided entry field labeled ""Zvanja"".
4.  Bela Points:
    -If the team scored Bela points, mark the checkbox labeled ""Bela"".
5.  Additional Options:
    -Depending on the game rules, additional options like ""Stilja"" might be available. These options will be displayed if applicable.

Adding Scores:
1.  After entering the necessary data for both teams, click the ""DODAJ"" button to add the scores.

Menu Options:
1.  Help:
    -Click the question mark icon in the Header section to access help and instructions on how to use the app.
2.  Settings:
    -Click the gear icon in the Header section to access app settings and customize your experience.";

    private void CloseButton_Clicked(object sender, EventArgs e)
    {
        this.Close();
    }

    async Task<string> LoadMauiAsset()
    {
        using var stream = await FileSystem.OpenAppPackageFileAsync("UserInstructions.txt");
        using var reader = new StreamReader(stream);

       return reader.ReadToEnd();
    }

    private async void LoadHelpText(string fileName)
    {
        try
        {
            helpMessage.Text = await LoadMauiAsset();
        }
        catch
        {
            //helpMessage.Text = "Error 404: helpText not found.";
            helpMessage.Text = "404";
        }
    }

    public HelpPopup()
	{
        InitializeComponent();
        LoadHelpText("UserInstructions.txt");
	}
}