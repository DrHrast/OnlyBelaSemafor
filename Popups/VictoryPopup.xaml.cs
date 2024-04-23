using CommunityToolkit.Maui.Views;

namespace OnlyBelaSemafor;

public partial class VictoryPopup : Popup
{
	public VictoryPopup(MainPage mainPage, string victoriousTeam)
	{
        InitializeComponent();
        this.mainPage = mainPage;
        this.victoriousTeam = victoriousTeam;
        victoryLabel.Text = this.victoriousTeam;
    }

    private readonly MainPage mainPage;
    private readonly string victoriousTeam;

    private void CloseButton_Clicked(object sender, EventArgs e)
    {
        this.Close();
        mainPage.NewGame();
    }

    private void NewGameButton_Clicked(object sender, EventArgs e)
    {
        this.Close();
        mainPage.NewGame();
    }

    private void ExitButton_Clicked(object sender, EventArgs e)
    {
        mainPage.NewGame();
        mainPage.QuitGame();
    }
}