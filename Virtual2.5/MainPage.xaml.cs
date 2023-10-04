using System.Drawing;

namespace Virtual2._5
{
    public partial class MainPage : ContentPage
    {
        Microsoft.Maui.Graphics.Point center;
        double radius;

        public MainPage()
        {
            InitializeComponent();
        }


        void absoluteLayout_SizeChanged(object sender, EventArgs e)
        {
            /*center = new Microsoft.Maui.Graphics.Point(absoluteLayout.Width / 2, absoluteLayout.Height / 2);
            AbsoluteLayout.SetLayoutBounds(image, new Rectangle(center.X - image.Width / 2),center.Y - radius,
            AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));*/
            radius = Math.Min(absoluteLayout.Width, absoluteLayout.Height) / 2;
        }

        async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            image.Rotation= 0;
            image.AnchorY = radius / image.Height;
            await image.RotateTo(360, 2000);
        }
    }
}