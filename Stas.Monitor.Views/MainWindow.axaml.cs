using System.Collections.Generic;
using Avalonia.Controls;
using Stas.Monitor.Presentations;
using Stas.Monitor.Presentations.IViews;

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

    public void SetNames(ISet<string> names)
    {
      ListName.ItemsSource = names;
      ListName.SelectedIndex = 0;
      _presenter.SelectThermometer((string) ListName.SelectedItem!);
    }

    public void ShowMeasurements(IEnumerable<MeasurementViewModel> measures)
    {
      Measurements.ItemsSource = measures;
    }
}