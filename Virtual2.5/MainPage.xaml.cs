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
            AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            radius = Math.Min(absoluteLayout.Width, absoluteLayout.Height) / 2; */
        }

        async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            /*image.Rotation= 0;
            image.AnchorY = radius / image.Height;
            await image.RotateTo(360, 2000);*/
        }

        private async void Addimg_Clicked(object sender, EventArgs e)
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                FileResult myPhoto = await MediaPicker.Default.CapturePhotoAsync();
                if (myPhoto != null)
                {
                    //saving the imagge captured in the app
                    string localFilePath = Path.Combine(FileSystem.CacheDirectory, myPhoto.FileName);
                    using Stream sourceStream = await myPhoto.OpenReadAsync();
                    using FileStream localFileStream = File.OpenWrite(localFilePath);
                    await sourceStream.CopyToAsync(localFileStream);
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("OOPS", "Your device isn't supported!", "Ok");
            }
        }
    }
}