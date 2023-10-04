namespace CalMall
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        async private void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EquationPage(1));
        }

        async private void Button_Clicked_1(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EquationPage(2));
        }

        async private void Button_Clicked_2(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EquationPage(3));
        }
    }
}