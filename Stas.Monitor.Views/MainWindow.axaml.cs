using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Stas.Monitor.Presentations;
using Stas.Monitor.Presentations.IViews;

namespace Stas.Monitor.Views;

public partial class MainWindow : Window, IMainView
{
  public event EventHandler<string>? SelectedThermometer;
  public MainWindow()
  {
    InitializeComponent();

    ListName.SelectionChanged += (sender, args) =>
    {
      OnSelectedThermometer((string) ListName.SelectedItem!);
    };
  }

  public void SetNames(ISet<string> names)
  {
    ListName.ItemsSource = names;
    ListName.SelectedIndex = 0;
    OnSelectedThermometer((string) ListName.SelectedItem!);
  }

  public void ShowMeasurements(IEnumerable<StatementViewModel> measures)
  {
    Measurements.ItemsSource = measures;
  }
  private void OnSelectedThermometer(string e)
  {
    SelectedThermometer?.Invoke(this, e);
  }
}