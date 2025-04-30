namespace ImprovingPhysicalHealthApp;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private bool ValidateLogin(string username, string password)
    {
        return username == "user" && password == "1234";
    }

    private void OnLoginClicked(object sender, EventArgs e)
    {
        var username = usernameEntry.Text?.Trim();
        var password = passwordEntry.Text;

        if (ValidateLogin(username, password))
        {
            Application.Current.MainPage = new AppShell();
        }
        else
        {
            messageLabel.Text = "Invalid login. Please try again.";
            messageLabel.IsVisible = true;
        }

    }
}
