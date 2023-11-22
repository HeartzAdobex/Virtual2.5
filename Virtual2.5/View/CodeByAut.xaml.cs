using Microsoft.Maui.Layouts;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration;

namespace Virtual2._5.View;

public partial class CodeByAut : ContentPage
{
    private List<Image> _movableImages = new List<Image>();
    private double currentScale = 1, startScale = 1;
    private Image _selectedImage;
    private double _startX, _startY;

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
                ManipulableImage.Source = ImageSource.FromStream(() => stream);
            }
        }
        catch (Exception ex)
        {
           
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
        var image = (Image)sender;

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

    private void OnClearImagesClicked(object sender, EventArgs e)
    {
        ManipulableImage.Source = null;

        SliderTranslationX.Value = 0;
        SliderTranslationY.Value = 0;
        SliderScale.Value = 1;
        SliderRotation.Value = 0;

        ManipulableImage.TranslationX = 0;
        ManipulableImage.TranslationY = 0;
        ManipulableImage.Scale = 1;
        ManipulableImage.Rotation = 0;
        ManipulableImage.RotationX = 0;
        ManipulableImage.RotationY = 0;
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
        if (ManipulableImage != null)
        {
            ManipulableImage.RotationX = e.NewValue;

            double scale = CalculateScaleBasedOnRotationX(e.NewValue);
            ManipulableImage.Scale = scale;
        }
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
            ManipulableImage.TranslationY = e.NewValue;
    }

    private void SliderScale_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        ManipulableImage.Scale = e.NewValue;
    }

    private void SliderRotation_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        ManipulableImage.Rotation = e.NewValue;
    }

    private async void OnSaveImageClicked(object sender, EventArgs e)
    {
        
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