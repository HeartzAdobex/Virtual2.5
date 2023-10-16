namespace Virtual2._5.View;

public partial class CodeByAut : ContentPage
{
    private List<Image> _movableImages = new List<Image>();
    private double currentScale = 1, startScale = 1;
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
                var image = new Image
                {
                    Source = ImageSource.FromStream(() => stream),
                    WidthRequest = 150,
                    HeightRequest = 150,
                    Aspect = Aspect.AspectFill
                };

                var panGesture = new PanGestureRecognizer();
                panGesture.PanUpdated += OnPanUpdated;
                image.GestureRecognizers.Add(panGesture);

                var pinchGesture = new PinchGestureRecognizer();
                pinchGesture.PinchUpdated += OnPinchUpdated;
                image.GestureRecognizers.Add(pinchGesture);

                _movableImages.Add(image);
                MainLayout.Children.Add(image);
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

    private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
    {
        var image = sender as Image;
        var currentLocation = AbsoluteLayout.GetLayoutBounds(image);

        switch (e.StatusType)
        {
            case GestureStatus.Running:
                currentLocation.X += e.TotalX;
                currentLocation.Y += e.TotalY;
                AbsoluteLayout.SetLayoutBounds(image, currentLocation);
                break;
            case GestureStatus.Completed:
                break;
        }
    }

    private void OnClearImagesClicked(object sender, EventArgs e)
    {
        foreach (var image in _movableImages)
        {
            MainLayout.Children.Remove(image);
        }
        _movableImages.Clear();
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