using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CalMall;

public partial class EquationPage : ContentPage
{
    private readonly int equationNumber;
    public EquationPage(int equationNumber)
	{
		InitializeComponent();
        this.equationNumber = equationNumber;
    }


    private void Button_Clicked(object sender, EventArgs e)
    {
        if (double.TryParse(NumberEntry.Text, out double number))
        {
            var calculationResult = PerformCalculation(number);

            Navigation.PushAsync(new ResultPage(calculationResult));
        }
        else
        {

        }
    }


    private CalculationResult PerformCalculation(double number)
    {
        double A = number;
        double R = 0;
        double S = 0;
        double F = 0;
        double S_Path = 0;
        double S_Indoor = 0;
        double S_Garden = 0;
        double S_Office = 0;
        double S_Toilet = 0;
        double c = 0;
        double C = 0;
        double BC = 0;
        double OSA = 0;
        double G = 0;
        double FAR = 0;
        double OSR = 0;

        if (equationNumber == 1)
        {
            R = 0.5;
            F = (A * 50) / 100;
            S = (F * 50) / 100;
            S_Path = (F * 35) / 100;
            S_Indoor = (F * 10) / 100;
            S_Garden = (F * 1) / 100;
            S_Office = (F * 1) / 100;
            S_Toilet = (F * 3) / 100;
            c = (S / 40);
            C = (c * 20);
            BC = (A * 30) / 100;
            OSA = (A * 70) / 100;
            G = (OSA * 25) / 100;
            FAR = (F / A);
            OSR = (OSA / F) * 100;
        }
        else if (equationNumber == 2)
        {
            R = 1;
            F = (A * 100) / 100;
            S = (F * 50) / 100;
            S_Path = (F * 35) / 100;
            S_Indoor = (F * 10) / 100;
            S_Garden = (F * 1) / 100;
            S_Office = (F * 1) / 100;
            S_Toilet = (F * 3) / 100;
            c = (S / 40);
            C = (c * 25);
            BC = (A * 50) / 100;
            OSA = (A * 50) / 100;
            G = (OSA * 25) / 100;
            FAR = (F + 0.8) *C / A;
            OSR = OSA / (F + C) * 100;
        }
        else if (equationNumber == 3)
        {
             R = 2;
             F = (A * 200) / 100;
             S = (F * 50) / 100;
             S_Path = (F * 35) / 100;
             S_Indoor = (F * 10) / 100;
             S_Garden = (F * 1) / 100;
             S_Office = (F * 1) / 100;
             S_Toilet = (F * 3) / 100;
             c = (S / 40);
             C = (c * 25);
             BC = (A * 70) / 100;
             OSA = (A * 30) / 100;
             G = (OSA * 25) / 100;
             FAR = F + (0.8 * C) / A;
             OSR = OSA / (F + C) * 100;

        }

        return new CalculationResult
        {
            A = A,
            R = R,
            S = S,
            F = F,
            S_Path = S_Path,
            S_Indoor = S_Indoor,
            OSA = OSA,
            OSR = OSR,
            FAR = FAR,
            S_Garden = S_Garden,
            S_Office = S_Office,
            S_Toilet = S_Toilet,
            c = c,
            C = C,
            BC = BC,
            G = G,
        };
    }
}