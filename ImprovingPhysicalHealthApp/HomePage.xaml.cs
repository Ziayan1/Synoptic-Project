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
    private async void OnCalorieClicked(object sender, EventArgs e)
    {
        if (UserData.Bmi == null || string.IsNullOrEmpty(UserData.Gender))
        {
            await DisplayAlert("BMI Required", "Please complete the BMI calculator first.", "OK");
            return;
        }

        Application.Current.MainPage = new CaloriePage();
    }
    private void OnWorkoutSchedulerClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new WorkoutSchedulerPage();
    }


    private void OnDietClicked(object sender, EventArgs e) { }

    private void OnLogoutClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new LoginPage();
    }
}
