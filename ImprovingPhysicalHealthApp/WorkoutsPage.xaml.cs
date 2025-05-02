namespace ImprovingPhysicalHealthApp;

public partial class WorkoutsPage : ContentPage
{
    private List<Workout> allWorkouts;

    public WorkoutsPage()
    {
        InitializeComponent();
        GenerateSmartSuggestion();
        LoadWorkouts();
        DisplayWorkouts("All");
    }

    private void GenerateSmartSuggestion()
    {
        double bmi = UserData.Bmi ?? 22;
        string fitnessLevel = UserData.FitnessLevel ?? "Beginner";

        if (bmi > 30)
        {
            suggestionLabel.Text = $"Based on your BMI ({bmi:F1}) and '{fitnessLevel}' level, we recommend starting with low-impact cardio and short sessions.";
        }
        else if (bmi < 18.5)
        {
            suggestionLabel.Text = $"You're underweight (BMI {bmi:F1}). Try light strength workouts and eat high-protein meals.";
        }
        else
        {
            suggestionLabel.Text = $"Your BMI is healthy ({bmi:F1}). You're good to go with a balanced plan for your fitness level: {fitnessLevel}.";
        }
    }

    private void LoadWorkouts()
    {
        allWorkouts = new List<Workout>
        {
            new Workout("Home Cardio", "20 min", 180, "Home"),
            new Workout("Bodyweight Strength", "25 min", 200, "Home"),
            new Workout("Treadmill Intervals", "30 min", 250, "Gym"),
            new Workout("Full Body Stretch", "15 min", 80, "Low Impact"),
            new Workout("Elliptical Session", "20 min", 160, "Low Impact"),
            new Workout("Weight Training", "40 min", 300, "Gym"),
            new Workout("Yoga Flow", "30 min", 120, "Low Impact"),
            new Workout("HIIT Blast", "25 min", 280, "Home"),
        };
    }

    private void DisplayWorkouts(string category)
    {
        workoutList.Children.Clear();

        var filtered = category == "All"
            ? allWorkouts
            : allWorkouts.Where(w => w.Category == category).ToList();

        foreach (var workout in filtered)
        {
            var card = new Frame
            {
                CornerRadius = 10,
                Padding = 15,
                BackgroundColor = Colors.White,
                Content = new VerticalStackLayout
                {
                    Spacing = 5,
                    Children =
                    {
                        new Label { Text = workout.Title, FontSize = 18, FontAttributes = FontAttributes.Bold, TextColor = Colors.Black },
                        new Label { Text = $"Duration: {workout.Duration}", FontSize = 14, TextColor = Colors.Gray },
                        new Label { Text = $"Burns ~{workout.Calories} kcal", FontSize = 14, TextColor = Colors.Gray }
                    }
                }
            };

            workoutList.Children.Add(card);
        }
    }

    private void OnFilterClicked(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            string category = button.Text;
            DisplayWorkouts(category);
        }
    }

    private void OnBackClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new HomePage();
    }
}

// Simple workout class
public class Workout
{
    public string Title { get; }
    public string Duration { get; }
    public int Calories { get; }
    public string Category { get; }

    public Workout(string title, string duration, int calories, string category)
    {
        Title = title;
        Duration = duration;
        Calories = calories;
        Category = category;
    }
}
