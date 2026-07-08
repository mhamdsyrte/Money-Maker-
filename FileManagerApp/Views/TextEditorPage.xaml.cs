using FileManagerApp.ViewModels;

namespace FileManagerApp.Views;

public partial class TextEditorPage : ContentPage
{
    public TextEditorPage(TextEditorViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
