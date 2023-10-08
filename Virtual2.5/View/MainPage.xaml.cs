namespace Virtual2._5
{
    public partial class MainPage : ContentPage
    {
        double currentScale = 1;
        double startScale = 1;
        double xOffset = 0;
        double yOffset = 0;

        public MainPage()
        {
            InitializeComponent();
            PinchGestureRecognizer pinchGesture = new PinchGestureRecognizer();
            pinchGesture.PinchUpdated += PinchGestureRecognizer_PinchUpdated;
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
        }

        async void TakePhoto_Clicked(object sender, EventArgs e)
        {
            TakePhotoAsync();
        }

        
        
        
        
        
        
        
        private void PinchGestureRecognizer_PinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            if (e.Status == GestureStatus.Started)
            {
                startScale = Content.Scale;
                Content.AnchorX = 0;
                Content.AnchorY = 0;
            }

            if (e.Status == GestureStatus.Running)
            {
                currentScale += (e.Scale - 1) * startScale;
                currentScale = Math.Max(1, currentScale);

                double renderedX = Content.X + xOffset;
                double deltaX = renderedX / Width;
                double deltaWidth = Width / (Content.Width * startScale);
                double originX = (e.ScaleOrigin.X - deltaX) * deltaWidth;

                double renderedY = Content.Y + yOffset;
                double deltaY = renderedY / Height;
                double deltaHeight = Height / (Content.Height * startScale);
                double originY = (e.ScaleOrigin.Y - deltaY) * deltaHeight;

                double targetX = xOffset - (originX * Content.Width) * (currentScale - startScale);
                double targetY = xOffset - (originY * Content.Width) * (currentScale - startScale);

                Content.TranslationX = Math.Clamp(targetX, -Content.Width * (currentScale - 1), 0);
                Content.TranslationY = Math.Clamp(targetY, -Content.Width * (currentScale - 1), 0);

                Content.Scale = currentScale;
            }
            if (e.Status == GestureStatus.Completed)
            {
                xOffset = Content.TranslationX; 
                yOffset = Content.TranslationY;
            }
        }
    }
}