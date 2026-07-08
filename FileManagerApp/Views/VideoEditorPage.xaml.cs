using FileManagerApp.ViewModels;

namespace FileManagerApp.Views;

public partial class VideoEditorPage : ContentPage
{
    public VideoEditorPage(VideoEditorViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
