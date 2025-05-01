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

        if (string.IsNullOrEmpty(gender))
        {
            bmiValueLabel.IsVisible = false;
            bmiAdviceLabel.Text = "Please select your gender.";
            bmiAdviceLabel.IsVisible = true;
            return;
        }

        // Try reading and converting height and weight
        if (double.TryParse(heightEntry.Text, out double height) &&
            double.TryParse(weightEntry.Text, out double weight) &&
            height > 0)
        {
            double bmi = weight / (height * height);
            string message;

            // Slightly different ranges and messages based on gender
            if (gender == "Male")
            {
                if (bmi < 18.5)
                    message = "Your BMI is below the healthy range. Consider speaking with a health professional.";
                else if (bmi < 25)
                    message = "You're in the healthy range. Keep it up!";
                else if (bmi < 30)
                    message = "You're slightly over. Some regular activity and small changes can help.";
                else
                    message = "Your BMI is higher than recommended. Start small — it all adds up.";
            }
            else // Female
            {
                if (bmi < 18.5)
                    message = "Your BMI is a bit low. Maybe check in with a professional for support.";
                else if (bmi < 24)
                    message = "Looks good! You're within the healthy range.";
                else if (bmi < 29)
                    message = "You're slightly above the ideal range. Consistency is key.";
                else
                    message = "BMI is elevated — be kind to yourself and take things one step at a time.";
            }

            bmiValueLabel.Text = $"BMI: {bmi:F1}";
            bmiAdviceLabel.Text = message;

            bmiValueLabel.IsVisible = true;
            bmiAdviceLabel.IsVisible = true;
        }
        else
        {
            // Invalid or missing input
            bmiValueLabel.IsVisible = false;
            bmiAdviceLabel.Text = "Please enter a valid height and weight.";
            bmiAdviceLabel.IsVisible = true;
        }
    }
}
