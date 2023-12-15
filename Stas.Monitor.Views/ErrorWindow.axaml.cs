using Avalonia.Controls;

namespace Stas.Monitor.Views;

public partial class ErrorWindow : Window
{

    public ErrorWindow(string errorMessage)
    {
        InitializeComponent();
        TextError.Text = errorMessage;
    }
}

