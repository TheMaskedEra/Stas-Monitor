using Avalonia.Controls;
using Stas.Monitor.Presentations;

namespace Stas.Monitor.Views;

public partial class MainWindow : Window
{
  private MainPresenter _presenter;
    public MainWindow(MainPresenter presenter)
    {
      _presenter = presenter;
      InitializeComponent();
      
    }
}