using Avalonia.Controls;

namespace Stas.Monitor.Views;

public partial class ErrorWindow : Window
{
    public string? ErrorMessage
    {
        get => TextError.Text;
        set => TextError.Text = value;
    }
    public ErrorWindow()
    {
        InitializeComponent();
    }
}

