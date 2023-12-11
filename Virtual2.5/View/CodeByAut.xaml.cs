using Image = Microsoft.Maui.Controls.Image;
using System.Diagnostics;

namespace Virtual2._5.View;

public partial class CodeByAut : ContentPage
{
    private List<Image> _movableImages = new List<Image>();
    private double currentScale = 1, startScale = 1;
    private Image _selectedImage;
    private double _startX, _startY;
    private Dictionary<Image, double> originalScales = new Dictionary<Image, double>();
    private Dictionary<Image, ImageProperties> imagePropertiesDictionary = new Dictionary<Image, ImageProperties>();



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
                    VerticalOptions = LayoutOptions.Center,
                };

                imagePropertiesDictionary[newImage] = new ImageProperties
                {
                    Scale = 1,
                    Rotation = 0,
                    TranslationX = 0,
                    TranslationY = 0,
                };

                originalScales[newImage] = 1.0;
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

    private bool isUpdatingSlidersProgrammatically = false;

    private void OnImageTapped(object sender, TappedEventArgs e)
    {
        if (sender is Image tappedImage)
        {
            _selectedImage = tappedImage;
            tappedImage.Opacity = 0.5;

            SliderTranslationX.ValueChanged -= SliderTranslationX_ValueChanged;
            SliderTranslationY.ValueChanged -= SliderTranslationY_ValueChanged;
            SliderScale.ValueChanged -= SliderScale_ValueChanged;
            SliderRotation.ValueChanged -= SliderRotation_ValueChanged;

            UpdateSlidersForSelectedImage();

            SliderTranslationX.ValueChanged += SliderTranslationX_ValueChanged;
            SliderTranslationY.ValueChanged += SliderTranslationY_ValueChanged;
            SliderScale.ValueChanged += SliderScale_ValueChanged;
            SliderRotation.ValueChanged += SliderRotation_ValueChanged;

            ApplyVisualFeedback(tappedImage);
            ResetVisualFeedbackForAllImages();
        }
    }

    private void UpdateSlidersForSelectedImage()
    {
        if (_selectedImage != null)
        {
            SliderTranslationX.Value = _selectedImage.TranslationX;
            SliderTranslationY.Value = _selectedImage.TranslationY;
            SliderScale.Value = _selectedImage.Scale;
            SliderRotation.Value = _selectedImage.Rotation;
        }
    }

    private void ApplyVisualFeedback(Image tappedImage)
    {
        tappedImage.Opacity = 0.5;
        foreach (var child in ImageContainer.Children)
        {
            if (child is Image img && img != tappedImage)
            {
                img.Opacity = 1.0;
            }
        }
    }

    private void ResetVisualFeedbackForAllImages()
    {
        foreach (var child in ImageContainer.Children)
        {
            if (child is Image img && img != _selectedImage)
            {
                img.Opacity = 1.0;
            }
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

    private void SliderTranslationX_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        if (_selectedImage != null && SliderTranslationX_ValueChanged != null)
        {
                _selectedImage.RotationX = e.NewValue;
                var scale = CalculateScaleBasedOnRotationX(e.NewValue);
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
        if (_selectedImage != null && SliderTranslationY_ValueChanged != null)
        {
                    _selectedImage.RotationY = e.NewValue;
                    var scale = CalculateScaleBasedOnRotationY(e.NewValue);

        }
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
        if (!isUpdatingSlidersProgrammatically && _selectedImage != null)
        {
                if (imagePropertiesDictionary.ContainsKey(_selectedImage))
                {
                    var props = imagePropertiesDictionary[_selectedImage];
                    props.Scale = e.NewValue;
                    _selectedImage.Scale = e.NewValue;
                }
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

    private async void OnCaptureImageClicked(object sender, EventArgs e)
    {
        try
        {
                var photo = await MediaPicker.CapturePhotoAsync();
                if (photo != null)
                {
                    var stream = await photo.OpenReadAsync();
                    Image newImage = new Image
                    {
                        Source = ImageSource.FromStream(() => stream),
                        Aspect = Aspect.AspectFit,
                        WidthRequest = 200,
                        HeightRequest = 200,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                    };

                    imagePropertiesDictionary[newImage] = new ImageProperties
                    {
                        Scale = 1,
                        Rotation = 0,
                        TranslationX = 0,
                        TranslationY = 0,
                    };

                    originalScales[newImage] = 1.0;

                    var tapGestureRecognizer = new TapGestureRecognizer();
                    tapGestureRecognizer.Tapped += OnImageTapped;
                    newImage.GestureRecognizers.Add(tapGestureRecognizer);

                    var panGesture = new PanGestureRecognizer();
                    panGesture.PanUpdated += OnPanUpdated;
                    newImage.GestureRecognizers.Add(panGesture);

                    ImageContainer.Children.Add(newImage);
                    _movableImages.Add(newImage);
                }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error capturing image: {ex.Message}");
        }
    }

    private async void OnSaveImageClicked(object sender, EventArgs e)
    {

    }
}
