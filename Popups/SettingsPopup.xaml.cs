using CommunityToolkit.Maui.Views;

namespace OnlyBelaSemafor;

public partial class SettingsPopup : Popup
{
    public SettingsPopup(MainPage mainPage)
    {
        InitializeComponent();
        this.mainPage = mainPage;
        isDarkModeSwitch.IsToggled = mainPage.GetDbMode();
    }

    string teamOneName;
    string teamTwoName;
    private string scoreValue;
    private string modeValue;
    private string scoreKey = "scoreValue";
    private string modeKey = "modeValue";
    private readonly MainPage mainPage;

    private void CloseWindow()
    {
        //TODO: 2. Implement database entry
        //TODO: 3. Implement control for reverting to default settings
        //TODO: 4. != null is idiotic and needs to be replaced with a respective value
        if (scoreValue != null)
        {
            mainPage.GameSettingsSetter(scoreKey, scoreValue);
        }
        if (modeValue != null)
        {
            mainPage.GameSettingsSetter(modeKey, modeValue);
        }
        this.Close();
    }

    private void CloseButton_Clicked(object sender, EventArgs e)
    {
        CloseWindow();
    }


    private void SetTeamsButton_Clicked(object sender, EventArgs e)
    {
        // Clear existing content
        settingsDiv.Content = null;
        settingsDiv.BorderColor = null;

        // Create content for setting teams (example: labels and entries)
        var label1 = new Label { Text = "Prvi Tim:", Margin = new Thickness(5) };
        var entry1 = new Entry { Placeholder = "Ime prvog tima", Margin = new Thickness(5), Keyboard = Keyboard.Text };
        var label2 = new Label { Text = "Drugi tim:", Margin = new Thickness(5) };
        var entry2 = new Entry { Placeholder = "Ime drugog tima", Margin = new Thickness(5), Keyboard = Keyboard.Text };
        var nameButton = new Button { Text = "Spremi promjene", Margin = new Thickness(10) };
        nameButton.Clicked += OnNameSaveButtonClicked;

        // Create a layout to hold the content
        var layout = new StackLayout();
        layout.Children.Add(label1);
        layout.Children.Add(entry1);
        layout.Children.Add(label2);
        layout.Children.Add(entry2);
        layout.Children.Add(nameButton);

        // Set the content of the settingsDiv
        settingsDiv.Content = layout;

        void OnNameSaveButtonClicked(object? sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(entry1.Text) && !string.IsNullOrEmpty(entry2.Text))
            {
                teamOneName = entry1.Text;
                teamTwoName = entry2.Text;
            }
            else if (!string.IsNullOrEmpty(entry1.Text))
            {
                teamOneName = entry1.Text;
            }
            else if (!string.IsNullOrEmpty(entry2.Text))
            {
                teamTwoName = entry2.Text;
            }
            mainPage.SetTeamNames(teamOneName, teamTwoName);
            this.Close();
        }
    }

    private void SetScoresButton_Clicked(object sender, EventArgs e)
    {
        // Clear existing content
        settingsDiv.Content = null;

        // Create content for setting scores (example: radio buttons)
        var radioButton1 = new RadioButton { Content = "1301" };
        var radioButton2 = new RadioButton { Content = "1001" };
        var radioButton3 = new RadioButton { Content = "701" };
        var radioButton4 = new RadioButton { Content = "501" };
        var radioButton5 = new RadioButton { Content = "301" };
        var scoreButton = new Button { Text = "Spremi promjene", Margin = new Thickness(10) };
        scoreButton.Clicked += ScoreButton_Clicked;

        // Create a layout to hold the content
        var layout = new StackLayout();
        layout.Children.Add(radioButton1);
        layout.Children.Add(radioButton2);
        layout.Children.Add(radioButton3);
        layout.Children.Add(radioButton4);
        layout.Children.Add(radioButton5);
        layout.Children.Add(scoreButton);

        // Set the content of the settingsDiv
        settingsDiv.Content = layout;

        void ScoreButton_Clicked(object? sender, EventArgs e)
        {
            if (radioButton1.IsChecked) scoreValue = "1301";
            else if (radioButton2.IsChecked) scoreValue = "1001";
            else if (radioButton3.IsChecked) scoreValue = "701";
            else if (radioButton4.IsChecked) scoreValue = "501";
            else if (radioButton5.IsChecked) scoreValue = "301";

            CloseWindow();
        }
    }

    private void DeleteLastResultButton_Clicked(object sender, EventArgs e)
    {
        mainPage.DeleteLastResult();
        CloseWindow();
    }

    private void isDarkModeSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        // Dark mode "tenebris" for ASCII value = 870
        // Light mode "lux" for ASCII value = 345
        if (isDarkModeSwitch.IsToggled)
        {
            modeValue = "Dark";
        }
        else
        {
            modeValue = "Light";
        }
    }
}