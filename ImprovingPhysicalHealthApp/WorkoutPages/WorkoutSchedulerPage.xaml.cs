using Microsoft.Maui.Storage;

namespace ImprovingPhysicalHealthApp;

public partial class WorkoutSchedulerPage : ContentPage
{
    //  day checkboxes
    private readonly List<CheckBox> dayCheckboxes = new();

   
    private readonly string[] daysOfWeek = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"]; // Days of the week

    public WorkoutSchedulerPage()
    {
        InitializeComponent();
        BuildDayCheckboxes(); // Creates checkboxes for each day
        fitnessPicker.SelectedIndexChanged += OnFitnessLevelChanged; 
        RestoreSelections(); 
    }

    // Makes the checkboxes for each day
    private void BuildDayCheckboxes()
    {
        foreach (string day in daysOfWeek)
        {
            var cb = new CheckBox { Color = Colors.DarkGreen };

            var label = new Label
            {
                Text = day,
                TextColor = Colors.Black,
                FontSize = 16,
                VerticalTextAlignment = TextAlignment.Center
            };

            var row = new HorizontalStackLayout
            {
                Spacing = 10,
                Children = { cb, label }
            };

            dayCheckboxes.Add(cb);
            daysLayout.Children.Add(row); // Add to the pag 
        }
    }

    // Runs when fitness level is picked
    private void OnFitnessLevelChanged(object sender, EventArgs e)
    {
        ShowSmartSuggestion(); // shows suggestions
        UpdateSummary(); // Shows summary
    }

    //  shows  suggestion depending on bmi level and and fitness 
    private void ShowSmartSuggestion()
    {
        string level = fitnessPicker.SelectedItem?.ToString();

        if (string.IsNullOrEmpty(level))
            return;

        int suggestedDays;
        string explanation = $"Selected level: {level}.";

        if (level == "Beginner")
        {
            suggestedDays = 3;
            explanation += " We recommend starting with 3 days to build consistency.";
        }
        else if (level == "Intermediate")
        {
            suggestedDays = 4;

            if (UserData.Bmi is double bmi)
            {
                if (bmi <= 25)
                {
                    suggestedDays = 5;
                    explanation += $" Your BMI is {bmi:F1}, so you're encouraged to train more frequently.";
                }
                else
                {
                    explanation += $" Your BMI is {bmi:F1}, so we suggest keeping it around 4 days.";
                }
            }
            else
            {
                explanation += " You can aim for 4–5 days depending on your energy.";
            }
        }
        else if (level == "Advanced")
        {
            suggestedDays = 6;

            if (UserData.Bmi is double bmi && bmi > 30)
            {
                suggestedDays = 5;
                explanation += $" Your BMI is {bmi:F1}, so you may want to scale back slightly to avoid overtraining.";
            }
            else
            {
                explanation += " You're experienced, so 6 days is ideal for progress.";
            }
        }
        else
        {
            suggestedDays = 3;
            explanation += " Defaulting to 3 days.";
        }

        suggestionLabel.Text = $"💡 Suggestion: Aim for {suggestedDays} days.\n{explanation}";
        suggestionLabel.IsVisible = true;
    }

    // This shows the options the user selected
    private void UpdateSummary()
    {
        var selectedDays = GetSelectedDayNames();
        string level = fitnessPicker.SelectedItem?.ToString() ?? "Not selected";

        if (selectedDays.Any())
        {
            summaryLabel.Text = $"✅ You selected: {string.Join(", ", selectedDays)}\n🏋️ Fitness Level: {level}";
        }
        else
        {
            summaryLabel.Text = $"🏋️ Fitness Level: {level}";
        }

        summaryLabel.IsVisible = true;
    }

    // looks for which days user picked
    private List<string> GetSelectedDayNames()
    {
        var selectedDays = new List<string>();

        foreach (var row in daysLayout.Children.OfType<HorizontalStackLayout>())
        {
            if (row.Children[0] is CheckBox cb && cb.IsChecked &&
                row.Children[1] is Label lbl)
            {
                selectedDays.Add(lbl.Text);
            }
        }

        return selectedDays;
    }

    // Rest when button is pressed
    private void OnResetClicked(object sender, EventArgs e)
    {
        foreach (var cb in dayCheckboxes)
            cb.IsChecked = false;

        fitnessPicker.SelectedIndex = -1;

        Preferences.Remove("WorkoutDays"); // Clears saved data
        Preferences.Remove("FitnessLevel");

        suggestionLabel.IsVisible = false;
        summaryLabel.IsVisible = false;
    }

    // saves data when save is clicked
    private void OnSaveClicked(object sender, EventArgs e)
    {
        var selectedDays = GetSelectedDayNames();
        string level = fitnessPicker.SelectedItem?.ToString() ?? "";

        Preferences.Set("WorkoutDays", string.Join(",", selectedDays));
        Preferences.Set("FitnessLevel", level);

        DisplayAlert("Saved", "Your workout plan has been saved.", "OK");
    }

    // loads the saved data
    private void RestoreSelections()
    {
        string level = Preferences.Get("FitnessLevel", "");
        if (!string.IsNullOrEmpty(level))
        {
            fitnessPicker.SelectedItem = level;
        }

        string savedDays = Preferences.Get("WorkoutDays", "");
        if (!string.IsNullOrEmpty(savedDays))
        {
            var selected = savedDays.Split(',');

            foreach (var row in daysLayout.Children.OfType<HorizontalStackLayout>())
            {
                if (row.Children[0] is CheckBox cb &&
                    row.Children[1] is Label lbl &&
                    selected.Contains(lbl.Text))
                {
                    cb.IsChecked = true;
                }
            }

            UpdateSummary();
            ShowSmartSuggestion();
        }
    }

    // return to home page
    private void OnBackToHomeClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new HomePage();
    }
}
