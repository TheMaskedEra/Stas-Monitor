using System.Collections.Generic;
using System.Drawing;
using Avalonia.Controls;
using Microsoft.VisualBasic;
using Stas.Monitor.Presentations;

namespace Stas.Monitor.Views;

public partial class MainWindow : Window, IMainView
{
  private readonly MainPresenter _presenter;
    public MainWindow(MainPresenter presenter)
    {
      _presenter = presenter;
      InitializeComponent();
      _presenter.SetView(this);

      ListName.SelectionChanged += (sender, args) =>
      {
        presenter.SelectThermometer((string) ListName.SelectedItem!);
      };
    }

    public void SetNames(IEnumerable<string> names)
    {
      ListName.ItemsSource = names;
      ListName.SelectedIndex = 0;
      _presenter.SelectThermometer((string) ListName.SelectedItem!);
    }

    public void ShowCurrentThermometer(string message)
    {
      ThermometerName.Text = message;
    }
}