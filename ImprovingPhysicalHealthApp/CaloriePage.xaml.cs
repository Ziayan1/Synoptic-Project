namespace ImprovingPhysicalHealthApp;

public partial class CaloriePage : ContentPage
{
    double totalCalories = 0;
    double totalWater = 0;

    // These should ideally come from BMI page, stored/shared globally.
    double userBmi = 23.5;       // example fallback
    string userGender = "Male";  // example fallback

    public CaloriePage()
    {
        InitializeComponent();

        // You can fetch real BMI/gender from a shared storage later
        ShowRecommendedIntake();
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

    // Shows estimated daily calorie recommendation based on gender and BMI
    private void ShowRecommendedIntake()
    {
        int recommendation;

        if (userGender == "Male")
        {
            recommendation = userBmi < 18.5 ? 2500 : (userBmi < 25 ? 2200 : 2000);
        }
        else
        {
            recommendation = userBmi < 18.5 ? 2100 : (userBmi < 24 ? 1900 : 1700);
        }

        recommendedLabel.Text = $"Suggested daily intake: {recommendation} kcal";
        recommendedLabel.IsVisible = true;
    }

    private void UpdateStatus()
    {
        statusLabel.Text = $"Today you've logged {totalCalories} kcal and {totalWater} ml water.";
        statusLabel.IsVisible = true;

        string message;

        if (totalCalories < 1200)
            message = "You’re under your intake. A healthy snack could help.";
        else if (totalCalories < 2200)
            message = "You’re doing great. Keep going!";
        else
            message = "You’ve gone above your target. That’s okay — just be mindful tomorrow.";

        feedbackLabel.Text = message;
        feedbackLabel.IsVisible = true;
    }

    private void OnBackToHomeClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new HomePage();
    }
}
