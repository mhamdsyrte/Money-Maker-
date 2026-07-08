using FileManagerApp.ViewModels;

namespace FileManagerApp.Views;

public partial class FileBrowserPage : ContentPage
{
    private readonly FileBrowserViewModel _viewModel;

    public FileBrowserPage(FileBrowserViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadFilesCommand.Execute(null);
    }
}
