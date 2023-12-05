using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Stas.Monitor.Presentations;
using Stas.Monitor.Presentations.IViews;

namespace Stas.Monitor.Views;

public partial class MainWindow : Window, IMainView
{
  public event EventHandler<string>? SelectedThermometer;
  public event EventHandler<bool>? TemperatureCheck;
  public event EventHandler<bool>? HumidityCheck;
  //TODO Delegate for events
  public MainWindow()
  {
    InitializeComponent();
    ListName.SelectionChanged += (sender, args) =>
    {
      OnSelectedThermometer((string) ListName.SelectedItem!);
    };

    Temperature.IsCheckedChanged += (sender, args) =>
    {
        OnTemperatureCheckedChanged((bool)Temperature.IsChecked!);
    };
    Humidity.IsCheckedChanged += (sender, args) =>
    {
        OnHumidityCheckedChanged((bool) Humidity.IsChecked!);
    };
  }
  public void SetNames(ISet<string> names)
  {
    ListName.ItemsSource = names;
    ListName.SelectedIndex = 0;
    OnSelectedThermometer((string) ListName.SelectedItem!);
  }

  public void ShowMeasurements(IEnumerable<MeasurementViewModel> measures) => Measurements.ItemsSource = measures;

  private void OnSelectedThermometer(string e) => SelectedThermometer?.Invoke(this, e);

  private void OnTemperatureCheckedChanged(bool value) => TemperatureCheck?.Invoke(this, value);
  private void OnHumidityCheckedChanged(bool value) => HumidityCheck?.Invoke(this, value);
}
