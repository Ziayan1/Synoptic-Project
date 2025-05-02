using Microsoft.Maui.Storage;

namespace ImprovingPhysicalHealthApp;

public partial class DietPlanPage : ContentPage
{
    private Dictionary<string, List<string>> mainCourses = new();
    private Dictionary<string, List<string>> sides = new();
    private Dictionary<string, List<string>> drinks = new();

    private string currentGoal = "";
    private string currentDiet = "";

    public DietPlanPage()
    {
        InitializeComponent();
        LoadFoodOptions();
        LoadSavedPreferences();
        ShowDay();
    }

    private void ShowDay()
    {
        string day = DateTime.Now.DayOfWeek.ToString();
        calorieTotalLabel.Text += $"\nMeal Plan for Today ({day})";
    }

    private void LoadFoodOptions()
    {
        // ✅ Expanded & filtered food lists
        mainCourses["Lose Weight"] = new List<string>
        {
            "Grilled Chicken - 350 kcal", "Tofu Bowl - 300 kcal", "Salmon Salad - 320 kcal",
            "Lentil Stew - 290 kcal", "Vegetable Stir Fry - 270 kcal"
        };

        mainCourses["Maintain"] = new List<string>
        {
            "Chicken Wrap - 450 kcal", "Egg Fried Rice - 430 kcal", "Veggie Pasta - 400 kcal",
            "Chickpea Curry - 410 kcal", "Paneer Bowl - 420 kcal"
        };

        mainCourses["Build Muscle"] = new List<string>
        {
            "Steak & Rice - 600 kcal", "Protein Burrito - 580 kcal", "Paneer Curry - 550 kcal",
            "Black Bean Burger - 530 kcal", "Vegan Chili - 500 kcal"
        };

        sides["Lose Weight"] = new List<string>
        {
            "Steamed Veggies - 80 kcal", "Fruit Bowl - 90 kcal", "Side Salad - 100 kcal",
            "Hummus & Carrot Sticks - 95 kcal"
        };

        sides["Maintain"] = new List<string>
        {
            "Roasted Veggies - 130 kcal", "Sweetcorn - 120 kcal", "Greek Yogurt - 140 kcal",
            "Bruschetta - 150 kcal"
        };

        sides["Build Muscle"] = new List<string>
        {
            "Boiled Eggs - 160 kcal", "Cheesy Broccoli - 180 kcal", "Mashed Potatoes - 200 kcal",
            "Mixed Nuts - 190 kcal"
        };

        drinks["Lose Weight"] = new List<string>
        {
            "Lemon Water - 10 kcal", "Green Tea - 0 kcal", "Fruit Smoothie - 120 kcal"
        };

        drinks["Maintain"] = new List<string>
        {
            "Lassi - 150 kcal", "Fresh Juice - 140 kcal", "Milkshake - 180 kcal"
        };

        drinks["Build Muscle"] = new List<string>
        {
            "Protein Shake - 220 kcal", "Banana Shake - 200 kcal", "Chocolate Milk - 230 kcal"
        };

        dietPicker.ItemsSource = new List<string> { "None", "Halal", "Vegetarian", "Vegan" };
    }

    private void OnGoalOrDietChanged(object sender, EventArgs e)
    {
        currentGoal = goalPicker.SelectedItem?.ToString() ?? "";
        currentDiet = dietPicker.SelectedItem?.ToString() ?? "";

        if (!string.IsNullOrEmpty(currentGoal) && !string.IsNullOrEmpty(currentDiet))
        {
            ApplyFoodFilter();
            mainCoursePicker.IsEnabled = true;
            sidePicker.IsEnabled = true;
            drinkPicker.IsEnabled = true;
        }
        else
        {
            mainCoursePicker.IsEnabled = false;
            sidePicker.IsEnabled = false;
            drinkPicker.IsEnabled = false;
        }
    }

    private void ApplyFoodFilter()
    {
        List<string> mains = mainCourses[currentGoal];
        List<string> filteredMains = FilterByDiet(mains, currentDiet);
        mainCoursePicker.ItemsSource = filteredMains;

        List<string> sideList = sides[currentGoal];
        List<string> filteredSides = FilterByDiet(sideList, currentDiet);
        sidePicker.ItemsSource = filteredSides;

        List<string> drinkList = drinks[currentGoal];
        List<string> filteredDrinks = FilterByDiet(drinkList, currentDiet);
        drinkPicker.ItemsSource = filteredDrinks;
    }

    private List<string> FilterByDiet(List<string> items, string diet)
    {
        return items.Where(item =>
        {
            string lower = item.ToLower();

            if (diet == "Vegetarian")
                return !(lower.Contains("chicken") || lower.Contains("steak") || lower.Contains("salmon"));

            if (diet == "Vegan")
                return !(lower.Contains("chicken") || lower.Contains("egg") || lower.Contains("paneer") || lower.Contains("steak") || lower.Contains("milk") || lower.Contains("yogurt") || lower.Contains("cheese") || lower.Contains("salmon") || lower.Contains("lassi"));

            return true;
        }).ToList();
    }

    private void OnAddToCaloriesClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(currentGoal) || string.IsNullOrEmpty(currentDiet))
        {
            DisplayAlert("Missing Info", "Please select both a goal and dietary preference.", "OK");
            return;
        }

        string main = mainCoursePicker.SelectedItem?.ToString() ?? "";
        string side = sidePicker.SelectedItem?.ToString() ?? "";
        string drink = drinkPicker.SelectedItem?.ToString() ?? "";

        Preferences.Set("DietGoal", currentGoal);
        Preferences.Set("DietPreference", currentDiet);
        Preferences.Set("SavedMain", main);
        Preferences.Set("SavedSide", side);
        Preferences.Set("SavedDrink", drink);

        int total = ExtractCalories(main) + ExtractCalories(side) + ExtractCalories(drink);

        // ✅ Add to global intake so Calorie Page can access it
        UserData.TotalCalories += total;

        suggestionsLabel.Text = $"Goal: {currentGoal}\nDiet: {currentDiet}\nMain: {main}\nSide: {side}\nDrink: {drink}";
        suggestionsLabel.IsVisible = true;

        string day = DateTime.Now.DayOfWeek.ToString();
        calorieTotalLabel.Text = $"Total Calories: {total} kcal\nMeal Plan for Today ({day})";
        calorieTotalLabel.IsVisible = true;

        DisplayAlert("Added!", $"{total} kcal added to your calorie intake.", "OK");
    }


    private int ExtractCalories(string item)
    {
        if (string.IsNullOrEmpty(item)) return 0;

        var match = System.Text.RegularExpressions.Regex.Match(item, @"(\d+)\s*kcal");
        return match.Success ? int.Parse(match.Groups[1].Value) : 0;
    }

    private void LoadSavedPreferences()
    {
        currentGoal = Preferences.Get("DietGoal", "");
        currentDiet = Preferences.Get("DietPreference", "");

        goalPicker.SelectedItem = currentGoal;
        dietPicker.SelectedItem = currentDiet;

        if (!string.IsNullOrEmpty(currentGoal) && !string.IsNullOrEmpty(currentDiet))
        {
            ApplyFoodFilter();
            mainCoursePicker.SelectedItem = Preferences.Get("SavedMain", "");
            sidePicker.SelectedItem = Preferences.Get("SavedSide", "");
            drinkPicker.SelectedItem = Preferences.Get("SavedDrink", "");

            if (!string.IsNullOrEmpty(mainCoursePicker.SelectedItem?.ToString()) ||
                !string.IsNullOrEmpty(sidePicker.SelectedItem?.ToString()) ||
                !string.IsNullOrEmpty(drinkPicker.SelectedItem?.ToString()))
            {
                OnAddToCaloriesClicked(null, null);
            }

            mainCoursePicker.IsEnabled = true;
            sidePicker.IsEnabled = true;
            drinkPicker.IsEnabled = true;
        }
    }

    private void OnResetClicked(object sender, EventArgs e)
    {
        goalPicker.SelectedIndex = -1;
        dietPicker.SelectedIndex = -1;
        mainCoursePicker.ItemsSource = null;
        sidePicker.ItemsSource = null;
        drinkPicker.ItemsSource = null;

        suggestionsLabel.IsVisible = false;
        calorieTotalLabel.IsVisible = false;

        Preferences.Clear();
    }

    private void OnBackToHomeClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new HomePage();
    }
}
