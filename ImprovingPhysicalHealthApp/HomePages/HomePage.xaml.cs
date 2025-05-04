namespace ImprovingPhysicalHealthApp;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent(); 
    }

    // open BMI calculator page
    private void OnBmiClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new BmiPage();
    }

    // open calorie tracker only if BMI is done
    private async void OnCalorieClicked(object sender, EventArgs e)
    {
        if (UserData.Bmi == null || string.IsNullOrEmpty(UserData.Gender))
        {
            await DisplayAlert("BMI Required", "Please complete the BMI calculator first.", "OK");
            return;
        }

        Application.Current.MainPage = new CaloriePage();
    }

    // open workout scheduler page
    private void OnWorkoutClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new WorkoutSchedulerPage();
    }

    // open workouts page
    private void OnWorkoutsClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new WorkoutsPage();
    }

    // open diet plan only if BMI is done
    private async void OnDietClicked(object sender, EventArgs e)
    {
        if (UserData.Bmi == null)
        {
            await DisplayAlert("BMI Required", "Please complete your BMI check first.", "OK");
            return;
        }

        await Navigation.PushAsync(new DietPlanPage());
    }

    // logs user out 
    private void OnLogoutClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new LoginPage();
    }
}
