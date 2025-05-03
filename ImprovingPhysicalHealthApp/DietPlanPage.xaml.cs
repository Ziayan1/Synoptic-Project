namespace ImprovingPhysicalHealthApp;

public partial class DietPlanPage : ContentPage
{
    private List<string> mealHistory = new List<string>();
    private string goal = "";

    public DietPlanPage()
    {
        InitializeComponent();

        if (UserData.Bmi == null)
        {
            DisplayAlert("BMI Required", "Please complete the BMI calculator first.", "OK");
            Application.Current.MainPage = new HomePage();
            return;
        }

        SetGoalFromBmi();
        LoadPickers();
        LoadMealHistory();
    }

    private void SetGoalFromBmi()
    {
        double bmi = UserData.Bmi ?? 22;

        if (bmi < 18.5)
            goal = "Weight Gain";
        else if (bmi > 25)
            goal = "Weight Loss";
        else
            goal = "Maintain Weight";

        goalLabel.Text = $"Goal: {goal}";
    }

    private void LoadPickers()
    {
        dietPicker.ItemsSource = new List<string> { "Halal", "Vegetarian", "Vegan", "None" };
        dietPicker.SelectedIndexChanged += (s, e) => RefreshFoodPickers();
    }

    private void LoadMealHistory()
    {
        string stored = Preferences.Get("MealHistory", "");
        if (!string.IsNullOrEmpty(stored))
        {
            mealHistory = stored.Split("||").ToList();
            mealHistoryLabel.Text = "Meal History:\n" + string.Join("\n", mealHistory.TakeLast(3));
            mealHistoryLabel.IsVisible = true;
        }
    }

    private void SaveMealHistory()
    {
        Preferences.Set("MealHistory", string.Join("||", mealHistory));
    }

    private void RefreshFoodPickers()
    {
        if (dietPicker.SelectedIndex == -1)
            return;

        string diet = dietPicker.SelectedItem.ToString();

        var mainOptions = new List<string>();
        var sideOptions = new List<string>();
        var drinkOptions = new List<string>();

        // Main courses by goal + dietary
        if (goal == "Weight Loss")
        {
            if (diet == "Vegan" || diet == "None")
            {
                mainOptions.Add("Tofu Salad - 280 kcal");
                mainOptions.Add("Chickpea Curry - 300 kcal");
                mainOptions.Add("Grilled Veg Bowl - 290 kcal");
                mainOptions.Add("Vegan Sushi - 310 kcal");
                mainOptions.Add("Quinoa & Beans - 330 kcal");
            }
            if (diet == "Vegetarian" || diet == "None")
            {
                mainOptions.Add("Grilled Veg Wrap - 300 kcal");
                mainOptions.Add("Paneer Tikka - 350 kcal");
                mainOptions.Add("Zucchini Lasagna - 340 kcal");
                mainOptions.Add("Greek Salad - 320 kcal");
                mainOptions.Add("Eggplant Parmesan - 330 kcal");
            }
            if (diet == "Halal" || diet == "None")
            {
                mainOptions.Add("Grilled Chicken Breast - 350 kcal");
                mainOptions.Add("Chicken Stir Fry - 360 kcal");
                mainOptions.Add("Baked Fish - 370 kcal");
                mainOptions.Add("Chicken & Veg Soup - 300 kcal");
                mainOptions.Add("Turkey Salad - 340 kcal");
            }
        }
        else if (goal == "Weight Gain")
        {
            if (diet == "Vegan" || diet == "None")
            {
                mainOptions.Add("Vegan Burrito - 600 kcal");
                mainOptions.Add("Lentil Stew - 620 kcal");
                mainOptions.Add("Stuffed Peppers - 590 kcal");
                mainOptions.Add("Tempeh Curry - 610 kcal");
                mainOptions.Add("Peanut Butter Bowl - 650 kcal");
            }
            if (diet == "Vegetarian" || diet == "None")
            {
                mainOptions.Add("Paneer Butter Masala - 620 kcal");
                mainOptions.Add("Vegetable Lasagna - 600 kcal");
                mainOptions.Add("Cheese Omelette - 580 kcal");
                mainOptions.Add("Mushroom Alfredo - 630 kcal");
                mainOptions.Add("Potato & Cheese Bake - 650 kcal");
            }
            if (diet == "Halal" || diet == "None")
            {
                mainOptions.Add("Chicken Biryani - 650 kcal");
                mainOptions.Add("Lamb Kebab Roll - 670 kcal");
                mainOptions.Add("Halal Beef Burger - 700 kcal");
                mainOptions.Add("Butter Chicken - 690 kcal");
                mainOptions.Add("Mutton Curry - 720 kcal");
            }
        }
        else if (goal == "Maintain Weight")
        {
            if (diet == "Vegan" || diet == "None")
            {
                mainOptions.Add("Tofu Stir Fry - 350 kcal");
                mainOptions.Add("Vegan Chili - 370 kcal");
                mainOptions.Add("Grain Bowl - 380 kcal");
                mainOptions.Add("Mixed Veg Rice - 390 kcal");
                mainOptions.Add("Coconut Curry - 400 kcal");
            }
            if (diet == "Vegetarian" || diet == "None")
            {
                mainOptions.Add("Veggie Pasta - 420 kcal");
                mainOptions.Add("Mushroom Stir Fry - 430 kcal");
                mainOptions.Add("Spinach Paneer Wrap - 440 kcal");
                mainOptions.Add("Egg Curry - 410 kcal");
                mainOptions.Add("Cheese Noodles - 450 kcal");
            }
            if (diet == "Halal" || diet == "None")
            {
                mainOptions.Add("Chicken Wrap - 450 kcal");
                mainOptions.Add("Halal Chicken Curry - 460 kcal");
                mainOptions.Add("Grilled Chicken Kebab - 440 kcal");
                mainOptions.Add("Tandoori Chicken - 470 kcal");
                mainOptions.Add("Fish Cutlet - 430 kcal");
            }
        }

        // SIDES
        sideOptions.Add("Fruit Salad - 150 kcal");
        if (diet != "Vegan") sideOptions.Add("Yogurt Pot - 180 kcal");
        if (diet == "Vegan" || diet == "Vegetarian" || diet == "None")
            sideOptions.Add("Roasted Veggies - 120 kcal");
        if (diet == "Vegetarian" || diet == "None")
            sideOptions.Add("Hummus & Crackers - 160 kcal");
        if (diet == "None" || diet == "Halal")
            sideOptions.Add("Spicy Potato Wedges - 200 kcal");

        // DRINKS
        drinkOptions.Add("Water - 0 kcal");
        drinkOptions.Add("Green Tea - 5 kcal");
        if (diet != "Vegan")
        {
            drinkOptions.Add("Milkshake - 200 kcal");
            drinkOptions.Add("Low-Fat Milk - 100 kcal");
        }
        if (diet == "Vegan" || diet == "Vegetarian" || diet == "None")
            drinkOptions.Add("Orange Juice - 100 kcal");

        mainCoursePicker.ItemsSource = mainOptions;
        sidePicker.ItemsSource = sideOptions;
        drinkPicker.ItemsSource = drinkOptions;
    }

    private int ExtractCalories(string item)
    {
        var match = System.Text.RegularExpressions.Regex.Match(item, @"(\d+)\s*kcal");
        return match.Success ? int.Parse(match.Groups[1].Value) : 0;
    }

    private void OnAddToCaloriesClicked(object sender, EventArgs e)
    {
        string main = mainCoursePicker.SelectedItem?.ToString() ?? "";
        string side = sidePicker.SelectedItem?.ToString() ?? "";
        string drink = drinkPicker.SelectedItem?.ToString() ?? "";

        int total = ExtractCalories(main) + ExtractCalories(side) + ExtractCalories(drink);
        UserData.TotalCalories += total;

        string day = DateTime.Now.DayOfWeek.ToString();
        string diet = dietPicker.SelectedItem?.ToString() ?? "N/A";

        suggestionsLabel.Text = $"Meal Plan for {day}\nGoal: {goal}\nDiet: {diet}\nMain: {main}\nSide: {side}\nDrink: {drink}";
        suggestionsLabel.IsVisible = true;

        calorieTotalLabel.Text = $"Total Calories: {total} kcal";
        calorieTotalLabel.IsVisible = true;

        string historyEntry = $"{DateTime.Now:MMM dd} - {main}, {side}, {drink} ({total} kcal)";
        mealHistory.Add(historyEntry);
        SaveMealHistory();

        mealHistoryLabel.Text = "Meal History:\n" + string.Join("\n", mealHistory.TakeLast(3));
        mealHistoryLabel.IsVisible = true;

        DisplayAlert("Meal Added", $"{total} kcal added to your calorie tracker.", "OK");
    }

    private void OnBackToHomeClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new HomePage();
    }
}
