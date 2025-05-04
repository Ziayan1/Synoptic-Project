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
        ShowDay(); // show the day
        ShowRecommendation(); // show reccomended intake
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // get calories from other pages
        totalCalories = UserData.TotalCalories;
        UpdateStatus(); // update progress bar and the label
    }

    private void ShowDay()
    {
        var today = DateTime.Now.DayOfWeek;
        dayLabel.Text = $"Tracking for: {today}";
    }

    private void OnAddCaloriesClicked(object sender, EventArgs e)
    {
        // check input is a number for validation
        if (double.TryParse(calorieEntry.Text, out double addedCalories))
        {
            totalCalories += addedCalories;
            UserData.TotalCalories += addedCalories; // save data globally

            calorieEntry.Text = ""; // reset box
            UpdateStatus(); // refresh the progress
        }
    }

    private void OnLogWaterClicked(object sender, EventArgs e)
    {
        // Check if number for validation
        if (double.TryParse(waterEntry.Text, out double addedWater))
        {
            totalWater += addedWater;
            waterEntry.Text = "";       // reset box
            UpdateStatus();  // refresh the progress
        }
    }

    private void ShowRecommendation()
    {
        double bmi = UserData.Bmi ?? 22;
        string gender = UserData.Gender ?? "Male";

        // Different data for male
        baseCalorie = gender == "Male" ? 2200 : 1900;
        baseWater = 2000;

        // Change depending on BMI
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

        // UPDATE calorie progress bar and the label
        calorieProgress.Progress = Math.Min(totalCalories / baseCalorie, 1);
        calorieTotalLabel.Text = $"Total: {totalCalories} kcal";

        // Feedback messages 
        string calorieFeedback;
        if (bmi > 30 && totalCalories > baseCalorie)
            calorieFeedback = "You are above limit for your calorie intake — Please try to reduce calories gradually.";
        else if (totalCalories < 1200)
            calorieFeedback = "You are below your calorie intake. Some light meal or snack will help.";
        else if (totalCalories <= baseCalorie)
            calorieFeedback = "You are doing really well. Keep it going!";
        else
            calorieFeedback = "You are over the limit — Don't worry, tommorow is a fresh start!";

        feedbackLabel.Text = calorieFeedback;
        feedbackLabel.IsVisible = true;

        // update water bar and label
        waterProgress.Progress = Math.Min(totalWater / baseWater, 1);
        waterTotalLabel.Text = $"Total: {totalWater} ml";

        // feedback water
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
        // reset all values
        totalCalories = 0;
        totalWater = 0;
        UserData.TotalCalories = 0;
        UpdateStatus();
    }

    private void OnBackToHomeClicked(object sender, EventArgs e)
    {
        // Return home
        Application.Current.MainPage = new HomePage();
    }
}
