namespace ImprovingPhysicalHealthApp;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent(); 
    }

    // validateif username and password are correct
    private bool ValidateLogin(string username, string password)
    {
        return username == "user" && password == "1234";
    }

    private void OnLoginClicked(object sender, EventArgs e)
    {
        var username = usernameEntry.Text?.Trim(); // get username
        var password = passwordEntry.Text;         // get password

        if (ValidateLogin(username, password))
        {
            // login successful leads to homePage of the app
            Application.Current.MainPage = new HomePage();
        }
        else
        {
            // show error message
            messageLabel.Text = "Invalid login. Please try again.";
            messageLabel.IsVisible = true;
        }
    }
}
