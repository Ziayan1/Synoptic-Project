namespace ImprovingPhysicalHealthApp;

public partial class BmiPage : ContentPage
{
    public BmiPage()
    {
        InitializeComponent();
    }

    private void OnCalculateBmiClicked(object sender, EventArgs e)
    {
        var gender = genderPicker.SelectedItem?.ToString();

        // Checks if gender was selected
        if (string.IsNullOrEmpty(gender))
        {
            bmiValueLabel.IsVisible = false;
            bmiAdviceLabel.Text = "Please select your gender.";
            bmiAdviceLabel.IsVisible = true;
            return;
        }

        // converting height and weight input
        if (double.TryParse(heightEntry.Text, out double height) &&
            double.TryParse(weightEntry.Text, out double weight) &&
            height > 0)
        {
            double bmi = weight / (height * height);
            string message;

            // results for males
            if (gender == "Male")
            {
                if (bmi < 18.5)
                    message = "Your BMI is below the healthy range. Consider speaking with a health professional for more details.";
                else if (bmi < 25)
                    message = "You are in the healthy range!";
                else if (bmi < 30)
                    message = "You are slightly over the reccomended BMI. Act on it today to see improvents.";
                else
                    message = "Your BMI results are higher than recommended. Start making change today.";
            }
            else // for Females
            {
                if (bmi < 18.5)
                    message = "Your BMI is a bit lower than reccomended. Check in with a professional for advice.";
                else if (bmi < 24)
                    message = "Healthy BMI! You're within the reccomended range.";
                else if (bmi < 29)
                    message = "You're slightly above the reccomended BMI range. Stay consistent for improvements.";
                else
                    message = "BMI is above the healthy range. Start today for improvemtns!";
            }

            // Show result
            bmiValueLabel.Text = $"BMI: {bmi:F1}";
            bmiAdviceLabel.Text = message;

            // Saves data globally
            UserData.Bmi = bmi;
            UserData.Gender = gender;

            bmiValueLabel.IsVisible = true;
            bmiAdviceLabel.IsVisible = true;
        }
        else
        {
            // Invalid input message for valiodation
            bmiValueLabel.IsVisible = false;
            bmiAdviceLabel.Text = "Please enter a valid height and weight.";
            bmiAdviceLabel.IsVisible = true;
        }
    }

    // return back to Home Page
    private void OnBackToHomeClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new HomePage();
    }
}
