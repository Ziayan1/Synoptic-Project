namespace ImprovingPhysicalHealthApp;

public partial class WorkoutsPage : ContentPage
{
    public WorkoutsPage()
    {
        InitializeComponent();

        if (UserData.Bmi == null)
        {
            DisplayAlert("BMI Required", "Please complete the BMI calculator first.", "OK");
            Application.Current.MainPage = new HomePage();
            return;
        }

        SetGoalLabel();

        fitnessPicker.ItemsSource = new List<string> { "Beginner", "Intermediate", "Advanced" };
        fitnessPicker.SelectedIndexChanged += OnFitnessLevelChanged;
    }

    private void SetGoalLabel()
    {
        double bmi = UserData.Bmi ?? 22;

        string goal;
        if (bmi < 18.5)
            goal = "Your Goal: Gain Weight";
        else if (bmi > 25)
            goal = "Your Goal: Lose Weight";
        else
            goal = "Your Goal: Stay Healthy";

        goalLabel.Text = goal;
    }

    private void OnFitnessLevelChanged(object sender, EventArgs e)
    {
        if (fitnessPicker.SelectedIndex == -1)
            return;

        string level = fitnessPicker.SelectedItem.ToString();
        double bmi = UserData.Bmi ?? 22;

        var workouts = GetSuggestedWorkouts(bmi, level);

        workoutList.Children.Clear();
        suggestionTitle.IsVisible = true;

        foreach (string workout in workouts)
        {
            workoutList.Children.Add(new Button
            {
                Text = workout,
                BackgroundColor = Color.FromArgb("#2196F3"), // Blue buttons
                TextColor = Colors.White,
                CornerRadius = 15,
                FontSize = 18,
                HeightRequest = 80,
                Margin = new Thickness(0, 10),
                Padding = new Thickness(10),
                HorizontalOptions = LayoutOptions.FillAndExpand
            });
        }
    }

    private List<string> GetSuggestedWorkouts(double bmi, string level)
    {
        var results = new List<string>();

        if (bmi > 30)
        {
            if (level == "Beginner")
            {
                results.Add("Walking - 20 min");
                results.Add("Light Cycling - 15 min");
                results.Add("Chair Squats - 3 sets");
                results.Add("Arm Circles - 2 min");
            }
            else if (level == "Intermediate")
            {
                results.Add("Jogging - 20 min");
                results.Add("Jump Rope - 5 min");
                results.Add("Bodyweight Squats - 3 sets");
                results.Add("Push-ups - 2 sets");
            }
            else
            {
                results.Add("HIIT - 20 min");
                results.Add("Sprints - 10x100m");
                results.Add("Burpees - 3 sets");
                results.Add("Mountain Climbers - 3 sets");
            }
        }
        else if (bmi < 18.5)
        {
            results.Add("Strength Training - Upper Body");
            results.Add("Leg Press Machine");
            results.Add("Dumbbell Rows");
            results.Add("Protein Stretch Cool-down");

            if (level != "Beginner")
                results.Add("Weighted Squats - 3 sets");
        }
        else
        {
            results.Add("Brisk Walk - 30 min");
            results.Add("Yoga - 15 min");
            results.Add("Plank - 1 min");
            results.Add("Resistance Band Workout");

            if (level == "Advanced")
                results.Add("Deadlifts - 3 sets");
        }

        return results;
    }

    private void OnBackToHomeClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new HomePage();
    }
}
