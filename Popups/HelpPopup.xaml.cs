using CommunityToolkit.Maui.Views;

namespace OnlyBelaSemafor;

public partial class HelpPopup : Popup
{
    private void CloseButton_Clicked(object sender, EventArgs e)
    {
        this.Close();
    }

    private void LoadHelpText()
    {
        string filePath = "..\\UserInstructions.txt";

        if (File.Exists(filePath))
        {
            string helpText = File.ReadAllText(filePath);
            helpMessage.Text = helpText;
        }
        else
        {
            helpMessage.Text = "Error 404: helpText not found.";
        }
    }

    public HelpPopup()
	{
        InitializeComponent();
        LoadHelpText();
	}
}