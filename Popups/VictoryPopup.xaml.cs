using CommunityToolkit.Maui.Views;

namespace OnlyBelaSemafor.Popups;

public partial class VictoryPopup : Popup
{
	public VictoryPopup(string victoriousTeam)
	{
        InitializeComponent();
        this.victoriousTeam = victoriousTeam;
        victoryLabel.Text = this.victoriousTeam;
    }

    private readonly string victoriousTeam;

    private void CloseButton_Clicked(object sender, EventArgs e)
    {
        App.Current.Quit();
    }

    private void NewGameButton_Clicked(object sender, EventArgs e)
    {
        Close();
    }

    private void ExitButton_Clicked(object sender, EventArgs e)
    {
        App.Current.Quit();
    }
}