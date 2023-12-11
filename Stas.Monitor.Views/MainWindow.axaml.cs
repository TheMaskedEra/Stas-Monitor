using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Stas.Monitor.Presentations;
using Stas.Monitor.Presentations.IViews;

namespace Stas.Monitor.Views;

public partial class MainWindow : Window, IMainView
{
  public event EventHandler<string>? SelectedThermometer;

  public event EventHandler<string>? SelectedTimer;

  public MainWindow()
  {
    InitializeComponent();
  }
  public void SetupSettings(IEnumerable<string> names, IEnumerable<string> timers)
  {
    ListName.ItemsSource = names;
    ListName.SelectedIndex = 0;

    Timers.ItemsSource = timers;
    Timers.SelectedIndex = 1;
  }

  public void CreateType(string typeName, IEnumerable<MeasurementViewModel> measurements)
  {

      var measurementList = new MeasurementList(measurements, typeName);

      MeasurementLists.Children.Add(measurementList);

      var checkBox = new CheckBox
      {
          Content = typeName,
          IsChecked = true
      };

      checkBox.IsCheckedChanged += (sender, e) =>
      {
          measurementList.IsVisible = (bool)checkBox.IsChecked;
      };

      CheckBoxTypes.Children.Add(checkBox);
  }

  private void OnSelectedThermometer(object? sender, SelectionChangedEventArgs selectionChangedEventArgs)
  {
      var thermometer = ListName.SelectedItem?.ToString();
      CheckBoxTypes.Children.Clear();
      MeasurementLists.Children.Clear();
      SelectedThermometer?.Invoke(this, thermometer!);
  }
  private void Timers_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
  {
      var timer = Timers.SelectedItem?.ToString();
      SelectedTimer?.Invoke(this, timer!);
  }
}
