using FileManagerApp.ViewModels;

namespace FileManagerApp.Views;

public partial class ImageEditorPage : ContentPage
{
    public ImageEditorPage(ImageEditorViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
