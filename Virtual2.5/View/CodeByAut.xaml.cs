using Microsoft.Maui.Layouts;
using Microsoft.Maui.Controls;

namespace Virtual2._5.View;

public partial class CodeByAut : ContentPage
{
    private List<Image> _movableImages = new List<Image>();
    private double currentScale = 1, startScale = 1;
    private Image _selectedImage;
    public CodeByAut()
	{
		InitializeComponent();
	}

    private async void OnPickImageClicked(object sender, EventArgs e)
    {
        try
        {
            var result = await MediaPicker.PickPhotoAsync();
            if (result != null)
            {
                var stream = await result.OpenReadAsync();
                _selectedImage = new Image
                {
                    Source = ImageSource.FromStream(() => stream),
                    WidthRequest = 150 * ResizeSlider.Value,
                    HeightRequest = 150 * ResizeSlider.Value,
                    Aspect = Aspect.AspectFill
                };

                var panGesture = new PanGestureRecognizer();
                panGesture.PanUpdated += OnPanUpdated;
                _selectedImage.GestureRecognizers.Add(panGesture);

                var pinchGesture = new PinchGestureRecognizer();
                pinchGesture.PinchUpdated += OnPinchUpdated;
                _selectedImage.GestureRecognizers.Add(pinchGesture);

                _selectedImage.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(() =>
                    {
                        ResizeSlider.IsVisible = false;
                        _selectedImage = null;
                    }),
                    NumberOfTapsRequired = 2
                });

                Grid.SetRow(_selectedImage, 0);
                MainLayout.Children.Add(_selectedImage);

                if (_selectedImage.Source != null)
                {
                    ResizeSlider.IsVisible = true;
                }
                _selectedImage.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(() =>
                    {
                        ResizeSlider.IsVisible = false;
                    })
                });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error picking image: {ex.Message}");
        }
    }

    private void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
    {
        var image = sender as Image;

        if (e.Status == GestureStatus.Started)
        {
            startScale = currentScale;
        }

        if (e.Status == GestureStatus.Running)
        {
            currentScale = startScale * e.Scale;

            image.Scale = currentScale;
        }

        if (e.Status == GestureStatus.Completed)
        {
            currentScale = image.Scale;
        }
    }

    private double _startX, _startY;
    private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
    {
        var image = sender as Image;

        switch (e.StatusType)
        {
            case GestureStatus.Started:
                _startX = e.TotalX;
                _startY = e.TotalY;
                break;

            case GestureStatus.Running:
                var deltaX = e.TotalX - _startX;
                var deltaY = e.TotalY - _startY;

                image.TranslationX += deltaX;
                image.TranslationY += deltaY;

                _startX = e.TotalX;
                _startY = e.TotalY;
                break;
        }
    }

    private void OnClearImagesClicked(object sender, EventArgs e)
    {
        var imagesToRemove = new List<Image>();

        foreach (var child in MainLayout.Children)
        {
            if (child is Image && child != BackgroundImage)
            {
                imagesToRemove.Add((Image)child);
            }
        }

        foreach (var image in imagesToRemove)
        {
            MainLayout.Children.Remove(image);
        }
    }

    private async void OnSaveImageClicked(object sender, EventArgs e)
    {
        var stream = await MainLayout.CaptureAsync();

        var filename = $"CapturedImage_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.png";
        var publicPath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures).AbsolutePath;
        var filePath = Path.Combine(publicPath, filename);
        using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            await stream.CopyToAsync(fileStream);
        }

        await DisplayAlert("Image Saved", $"Image saved as {filename} in the Pictures folder.", "OK");
    }

    private void ResizeSlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        if (_selectedImage != null)
        {
            _selectedImage.Scale = e.NewValue;
        }
    }

    private async void OnSetBackgroundClicked(object sender, EventArgs e)
    {
        try
        {
            var result = await MediaPicker.PickPhotoAsync();
            if (result != null)
            {
                var stream = await result.OpenReadAsync();
                BackgroundImage.Source = ImageSource.FromStream(() => stream);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error picking image for background: {ex.Message}");
        }
    }
}