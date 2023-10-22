namespace Virtual2._5
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
            //MainPage = new View.CodeByAut();
                //new NavigationPage(new MainPage());
        }
    }
}