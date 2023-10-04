using Android.App;
using Android.Database;
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

        //Taking photo
        async Task TakePhotoAsync()
        {
            try
            {
                var photo = await MediaPicker.Default.CapturePhotoAsync();
                var PhotoPath = await LoadPhotoAsync(photo);
                await DisplayAlert("System Message", $"Capturing completed!:{PhotoPath}", "OK");
                
            }
            catch (FeatureNotSupportedException fnsEx)
            {

            }
            catch (PermissionException pEx)
            {

            }
            catch (Exception ex)
            {

            }
        }
        
        //Loading Taken photo from storage
        async Task<string> LoadPhotoAsync(FileResult photo)
        {
            //canceled
            if(photo == null)
            {
                return "";
            }

            //save the file to storage
            string newFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

            using Stream sourceStream = await photo.OpenReadAsync();
            using FileStream fileStream = File.OpenWrite(newFilePath);

            await sourceStream.CopyToAsync(fileStream); // Copy Source Stream to new
            return newFilePath;
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

        async void Addimg_Clicked(object sender, EventArgs e)
        {
            var pickResult = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = FilePickerFileType.Png,
                PickerTitle = "Select your image"
            });

            if (pickResult == null)
                return;
            var stream = await pickResult.OpenReadAsync();
            resultImage.Source = ImageSource.FromStream(() => stream);
            /*if (MediaPicker.Default.IsCaptureSupported)
            {
                FileResult myPhoto = await MediaPicker.Default.CapturePhotoAsync();
                if (myPhoto != null)
                {
                    //saving the imagge captured in the app
                    string localFilePath = Path.Combine(FileSystem.CacheDirectory, myPhoto.FileName);
                    using Stream sourceStream = await myPhoto.OpenReadAsync();
                    using FileStream localFileStream = File.OpenWrite(localFilePath);
                    await sourceStream.CopyToAsync(localFileStream);
                    await DisplayAlert("Alert", localFileStream.Name, "Ok");
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("OOPS", "Your device isn't supported!", "Ok");
            }*/
        }

        async void TakePhoto_Clicked(object sender, EventArgs e)
        {
            TakePhotoAsync();
        }
    }
}