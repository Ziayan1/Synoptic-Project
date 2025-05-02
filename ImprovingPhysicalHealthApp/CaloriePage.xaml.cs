namespace ImprovingPhysicalHealthApp;

public partial class CaloriePage : ContentPage
{
    double totalCalories = 0;
    double totalWater = 0;
    int baseCalorie = 2200;
    int baseWater = 2000;

    public CaloriePage()
    {
        InitializeComponent();
        ShowDay();
        ShowRecommendation();
    }

    private void ShowDay()
    {
        var today = DateTime.Now.DayOfWeek;
        dayLabel.Text = $"Tracking for: {today}";
    }

    private void OnAddCaloriesClicked(object sender, EventArgs e)
    {
        if (double.TryParse(calorieEntry.Text, out double addedCalories))
        {
            totalCalories += addedCalories;

            // Save to global user data
            UserData.TotalCalories += addedCalories;

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
        double bmi = UserData.Bmi ?? 22;
        string gender = UserData.Gender ?? "Male";

        baseCalorie = gender == "Male" ? 2200 : 1900;
        baseWater = 2000;

        if (bmi < 18.5)
            baseCalorie += 200;
        else if (bmi > 30)
            baseCalorie -= 300;

        recommendedLabel.Text = $"Recommended intake: approx. {baseCalorie} kcal/day";
        recommendedLabel.IsVisible = true;

        recommendedWaterLabel.Text = $"Recommended water: {baseWater} ml/day";
        recommendedWaterLabel.IsVisible = true;
    }

    private void UpdateStatus()
    {
        double bmi = UserData.Bmi ?? 22;

        // Calorie progress
        calorieProgress.Progress = Math.Min(totalCalories / baseCalorie, 1);
        calorieTotalLabel.Text = $"Total: {totalCalories} kcal";

        string calorieFeedback;
        if (bmi > 30 && totalCalories > baseCalorie)
            calorieFeedback = "You're above your intake and BMI is high — try reducing calories gradually.";
        else if (totalCalories < 1200)
            calorieFeedback = "You're under your intake. A light meal or snack could help.";
        else if (totalCalories <= baseCalorie)
            calorieFeedback = "You're doing well. Keep it balanced!";
        else
            calorieFeedback = "You're a bit over — no worries, stay consistent tomorrow.";

        feedbackLabel.Text = calorieFeedback;
        feedbackLabel.IsVisible = true;

        // Water progress
        waterProgress.Progress = Math.Min(totalWater / baseWater, 1);
        waterTotalLabel.Text = $"Total: {totalWater} ml";

        string waterFeedback;
        if (totalWater < 1000)
            waterFeedback = "Try to drink more water to stay hydrated.";
        else if (totalWater < baseWater)
            waterFeedback = "Great! Keep sipping to reach your goal.";
        else
            waterFeedback = "You've reached your water goal today. Well done!";

        feedbackWaterLabel.Text = waterFeedback;
        feedbackWaterLabel.IsVisible = true;
    }

    private void OnResetClicked(object sender, EventArgs e)
    {
        totalCalories = 0;
        totalWater = 0;
        UpdateStatus();
    }

    private void OnBackToHomeClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new HomePage();
    }
}
