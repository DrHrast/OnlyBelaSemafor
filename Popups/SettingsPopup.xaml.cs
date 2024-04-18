using CommunityToolkit.Maui.Views;

namespace OnlyBelaSemafor;

public partial class SettingsPopup : Popup
{
    private void CloseButton_Clicked(object sender, EventArgs e)
    {
		this.Close();
    }

	public SettingsPopup()
	{
		InitializeComponent();
	}

    private void SetTeamsButton_Clicked(object sender, EventArgs e)
    {
        // Clear existing content
        settingsDiv.Content = null;

        // Create content for setting teams (example: labels and entries)
        var label1 = new Label { Text = "Prvi Tim:", Margin = new Thickness(5) };
        var entry1 = new Entry { Placeholder = "Ime prvog tima", Margin = new Thickness(5) };
        var label2 = new Label { Text = "Drugi tim:", Margin = new Thickness(5) };
        var entry2 = new Entry { Placeholder = "Ime drugog tima", Margin = new Thickness(5) };
        var nameButton = new Button { Text = "Spremi promjene", Margin = new Thickness(10) };

        // Create a layout to hold the content
        var layout = new StackLayout();
        layout.Children.Add(label1);
        layout.Children.Add(entry1);
        layout.Children.Add(label2);
        layout.Children.Add(entry2);
        layout.Children.Add(nameButton);

        // Set the content of the settingsDiv
        settingsDiv.Content = layout;
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
        var nameButton = new Button { Text = "Spremi promjene", Margin = new Thickness(10)};

        // Create a layout to hold the content
        var layout = new StackLayout();
        layout.Children.Add(radioButton1);
        layout.Children.Add(radioButton2);
        layout.Children.Add(radioButton3);
        layout.Children.Add(radioButton4);
        layout.Children.Add(radioButton5);
        layout.Children.Add(nameButton);

        // Set the content of the settingsDiv
        settingsDiv.Content = layout;
    }

    private void DeleteLastResultButton_Clicked(object sender, EventArgs e)
    {
        // For now, do nothing when the button is clicked
        // You can add functionality to delete the last result here in the future
    }

}