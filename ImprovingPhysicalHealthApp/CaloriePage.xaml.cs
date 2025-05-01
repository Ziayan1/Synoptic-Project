namespace ImprovingPhysicalHealthApp;

public partial class CaloriePage : ContentPage
{
    double totalCalories = 0;
    double totalWater = 0;

    public CaloriePage()
    {
        InitializeComponent();
        ShowRecommendation();
    }

    private void OnAddCaloriesClicked(object sender, EventArgs e)
    {
        if (double.TryParse(calorieEntry.Text, out double addedCalories))
        {
            totalCalories += addedCalories;
            calorieEntry.Text = string.Empty;
            UpdateStatus();
        }
    }

    private void OnLogWaterClicked(object sender, EventArgs e)
    {
        if (double.TryParse(waterEntry.Text, out double addedWater))
        {
            totalWater += addedWater;
            waterEntry.Text = string.Empty;
            UpdateStatus();
        }
    }

    private void ShowRecommendation()
    {
        double bmi = UserData.Bmi ?? 22; // fallback just in case
        string gender = UserData.Gender ?? "Male";

        int baseCalorie = gender == "Male" ? 2200 : 1900;

        if (bmi < 18.5)
            baseCalorie += 200;
        else if (bmi > 30)
            baseCalorie -= 300;

        recommendedLabel.Text = $"Recommended intake: approx. {baseCalorie} kcal/day";
        recommendedLabel.IsVisible = true;
    }

    private void UpdateStatus()
    {
        statusLabel.Text = $"Calories logged today: {totalCalories} kcal\nWater: {totalWater} ml";
        statusLabel.IsVisible = true;

        double bmi = UserData.Bmi ?? 22;

        string feedback;

        if (bmi > 30 && totalCalories > 2200)
            feedback = "You're above your intake and BMI is high — try reducing calories gradually.";
        else if (totalCalories < 1200)
            feedback = "You're under your intake. A light meal or snack could help.";
        else if (totalCalories <= 2200)
            feedback = "You're doing well. Keep it balanced!";
        else
            feedback = "You're a bit over — no worries, stay consistent tomorrow.";

        feedbackLabel.Text = feedback;
        feedbackLabel.IsVisible = true;
    }

    private void OnBackToHomeClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new HomePage();
    }
}
