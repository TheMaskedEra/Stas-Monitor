using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
  public void SetupSettings(IEnumerable<string> names, IEnumerable<string> displayTimers)
  {
    ListName.ItemsSource = names;
    ListName.SelectedIndex = 0;

    Timers.ItemsSource = displayTimers;
    Timers.SelectedIndex = 1;
  }

  public void GenerateThermometerSettings(IEnumerable<MeasurementViewModel> viewModels)
  {
      Lists.Children.Clear();
      CheckBoxTypes.Children.Clear();

      var groupedMeasurements = GroupMeasurementsByTypes(viewModels);

      foreach (var type in groupedMeasurements.Keys)
      {
          var measurementList = new MeasurementList()
          {
              ViewModels = groupedMeasurements[type],
              Type = type,
          };

          var checkBox = new CheckBox
          {
              Content = type,
              IsChecked = true
          };

          checkBox.IsCheckedChanged += (sender, e) =>
          {
              measurementList.IsVisible = (bool)checkBox.IsChecked;
          };

          Lists.Children.Add(measurementList);

          CheckBoxTypes.Children.Add(checkBox);
      }
  }

  private void OnSelectedThermometer(object? sender, SelectionChangedEventArgs args)
  {
      var thermometer = args.AddedItems[0]!.ToString();

      SelectedThermometer?.Invoke(this, thermometer!);
  }
  private void Timers_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
  {
      if (sender is not ComboBox)
      {
          return;
      }

      var timer = e.AddedItems[0]!.ToString();
      SelectedTimer?.Invoke(this, timer!);
  }

  private static IDictionary<string, IEnumerable<MeasurementViewModel>> GroupMeasurementsByTypes(IEnumerable<MeasurementViewModel> viewModels)
  {
      var groupedMeasurementViewModels = viewModels.GroupBy(measurement=> measurement.Type);
      return groupedMeasurementViewModels.ToDictionary(group => group.Key, group => group.AsEnumerable());
  }
}
