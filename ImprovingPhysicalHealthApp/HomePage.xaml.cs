namespace ImprovingPhysicalHealthApp;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
    }
    private void OnBmiClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new BmiPage();
    }

    private void OnCalorieClicked(object sender, EventArgs e) { }

    private void OnWorkoutClicked(object sender, EventArgs e) { }

    private void OnDietClicked(object sender, EventArgs e) { }

    private void OnLogoutClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new LoginPage();
    }
}
