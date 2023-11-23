using Microsoft.Maui.Layouts;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration;
using System.Text.Json.Serialization;
using static System.Net.Mime.MediaTypeNames;
using Image = Microsoft.Maui.Controls.Image;
using Microsoft.Maui.Storage;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.Json;

namespace Virtual2._5.View;

public partial class CodeByAut : ContentPage
{
    private List<Image> _movableImages = new List<Image>();
    private double currentScale = 1, startScale = 1;
    private Image _selectedImage;
    private double _startX, _startY;
    private double _sliderScale = 1.0;
    private double _rotationScale = 1.0;

    public CodeByAut()
	{
		InitializeComponent();
        var panGesture = new PanGestureRecognizer();
        panGesture.PanUpdated += OnPanUpdated;
        ManipulableImage.GestureRecognizers.Add(panGesture);
    }

    private async void OnPickImageClicked(object sender, EventArgs e)
    {
        try
        {
            var result = await MediaPicker.PickPhotoAsync();
            if (result != null)
            {
                var stream = await result.OpenReadAsync();
                Image newImage = new Image
                {
                    Source = ImageSource.FromStream(() => stream),
                    Aspect = Aspect.AspectFit,
                    WidthRequest = 200,
                    HeightRequest = 200,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                };


                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += OnImageTapped;
                newImage.GestureRecognizers.Add(tapGestureRecognizer);

                var panGesture = new PanGestureRecognizer();
                panGesture.PanUpdated += OnPanUpdated;
                newImage.GestureRecognizers.Add(panGesture);


                ImageContainer.Children.Add(newImage);
            }
        }
        catch (Exception ex)
        {

        }
    }

    private void ResetVisualFeedbackForAllImages()
    {
        foreach (var child in ImageContainer.Children)
        {
            if (child is Image image)
            {
                image.Scale = 1;
            }
        }
    }

    private void OnImageTapped(object sender, TappedEventArgs e)
    {
        if (sender is Image tappedImage)
        {
            _selectedImage = tappedImage;
            ResetVisualFeedbackForAllImages();
            tappedImage.Scale = 1.1;
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

    private enum TransformMode
    {
        Move,
        Scale,
        Rotate
    }

    private TransformMode _currentMode = TransformMode.Move;
    private Point _lastPanPoint;

    private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
    {
        if (sender is Image image && image == _selectedImage)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    _startX = image.TranslationX;
                    _startY = image.TranslationY;
                    break;
                case GestureStatus.Running:
                    image.TranslationX = _startX + e.TotalX;
                    image.TranslationY = _startY + e.TotalY;
                    break;
            }
        }
    }

    private void OnClearImagesClicked(object sender, EventArgs e)
    {
        if (_selectedImage != null)
        {
            _selectedImage.Source = null;

            ImageContainer.Children.Remove(_selectedImage);

            _selectedImage.TranslationX = 0;
            _selectedImage.TranslationY = 0;
            _selectedImage.Scale = 1;
            _selectedImage.Rotation = 0;
            _selectedImage.RotationX = 0;
            _selectedImage.RotationY = 0;

            _selectedImage = null;
        }

        SliderTranslationX.Value = 0;
        SliderTranslationY.Value = 0;
        SliderScale.Value = 1;
        SliderRotation.Value = 0;
    }

    private async void OnCaptureImageClicked(object sender, EventArgs e)
    {
        try
        {
            var photo = await MediaPicker.CapturePhotoAsync();
            if (photo != null)
            {
                var stream = await photo.OpenReadAsync();
                ManipulableImage.Source = ImageSource.FromStream(() => stream);
            }
        }
        catch (Exception ex)
        {
            
        }
    }

    private void SliderTranslationX_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        if (_selectedImage != null)
        {
            _selectedImage.RotationX = e.NewValue;

            double scale = CalculateScaleBasedOnRotationX(e.NewValue);
            _selectedImage.Scale = scale;
        }
        ApplyCombinedScale();
    }

    private double CalculateScaleBasedOnRotationX(double rotationX)
    {
        const double maxScale = 1.0;
        const double minScale = 0.8;

        double normalizedRotation = Math.Abs(rotationX % 360);
        if (normalizedRotation > 180)
            normalizedRotation = 360 - normalizedRotation;

        double scale = maxScale - ((normalizedRotation / 180) * (maxScale - minScale));
        return scale;
    }

    private void SliderTranslationY_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        if (_selectedImage != null)
        {
            _selectedImage.RotationY = e.NewValue;

            double scale = CalculateScaleBasedOnRotationY(e.NewValue);
            _selectedImage.Scale = scale;
        }
        ApplyCombinedScale();
    }

    private double CalculateScaleBasedOnRotationY(double rotationY)
    {
        const double maxScale = 1.0;
        const double minScale = 0.8;

        double normalizedRotation = Math.Abs(rotationY % 360);
        if (normalizedRotation > 180)
            normalizedRotation = 360 - normalizedRotation;

        double scale = maxScale - ((normalizedRotation / 180) * (maxScale - minScale));
        return scale;
    }
    private void SliderScale_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        if (_selectedImage != null)
        {
            _sliderScale = e.NewValue;

            _selectedImage.Scale = _sliderScale;
            ApplyCombinedScale();
        }
    }

    private void ApplyCombinedScale()
    {
        if (_selectedImage != null)
        {
            _selectedImage.Scale = _sliderScale * _rotationScale;
        }
    }

    private void SliderRotation_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        if (_selectedImage != null)
        {
            _selectedImage.Rotation = e.NewValue;
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
            Console.WriteLine($"Error picking: {ex.Message}");
        }
    }

    private async void OnSaveImageClicked(object sender, EventArgs e)
    {
        SavingIndicator.IsRunning = true;
        SavingIndicator.IsVisible = true;
        SavingLabel.IsVisible = true;

        await Task.Run(() =>
        {
            var imageProperties = new ImageProperties
            {
                Scale = ManipulableImage.Scale,
                Rotation = ManipulableImage.Rotation,
                RotationX = ManipulableImage.RotationX,
                RotationY = ManipulableImage.RotationY,
                TranslationX = ManipulableImage.TranslationX,
                TranslationY = ManipulableImage.TranslationY,
            };

            var serializedData = SerializeObject(imageProperties);
            SaveSerializedData(serializedData);
        });

        SavingIndicator.IsRunning = false;
        SavingIndicator.IsVisible = false;
        SavingLabel.IsVisible = false;

        await DisplayAlert("Save", "Image properties saved successfully.", "OK");
    }

    private void SaveSerializedData(string jsonData)
    {
        string folderPath = FileSystem.AppDataDirectory;
        string filePath = Path.Combine(folderPath, "image_properties.json");
        File.WriteAllText(filePath, jsonData);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        var serializedData = LoadSerializedData();
        var imageProperties = DeserializeObject(serializedData);
        if (imageProperties != null)
        {
            ApplyImageProperties(imageProperties);
        }
    }

    private void ApplyImageProperties(ImageProperties properties)
    {
        ManipulableImage.Scale = properties.Scale;
        ManipulableImage.Rotation = properties.Rotation;
        ManipulableImage.RotationX = properties.RotationX;
        ManipulableImage.RotationY = properties.RotationY;
        ManipulableImage.TranslationX = properties.TranslationX;
        ManipulableImage.TranslationY = properties.TranslationY;
    }

    private string LoadSerializedData()
    {
        string folderPath = FileSystem.AppDataDirectory;
        string filePath = Path.Combine(folderPath, "image_properties.json");

        if (File.Exists(filePath))
        {
            return File.ReadAllText(filePath);
        }

        return string.Empty;
    }

    private ImageProperties DeserializeObject(string serializedData)
    {
        if (string.IsNullOrWhiteSpace(serializedData))
            return null;

        try
        {
            return JsonSerializer.Deserialize<ImageProperties>(serializedData);
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"JSON Error: {ex.Message}");
            return null;
        }
    }

    private string SerializeObject(ImageProperties imageProperties)
    {
        return JsonSerializer.Serialize(imageProperties);
    }
}