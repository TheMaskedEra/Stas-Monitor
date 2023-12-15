using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Stas.Monitor.Presentations;
using Stas.Monitor.Presentations.IViews;

namespace Stas.Monitor.Views;

public partial class MainWindow : Window, IMainView
{
  public event EventHandler<string>? SelectedThermometer;

  public event EventHandler<int>? SelectedTimer;

  public MainWindow()
  {
    InitializeComponent();
  }
  public void SetupSettings(IEnumerable<string> names, IList<(int Value, string DisplaySettings)> timers)
  {
    ListName.ItemsSource = names;
    ListName.SelectedIndex = 0;

    Timers.ItemsSource = timers;
    Timers.SelectedIndex = 1;
  }

  public void GenerateThermometerSettings(IDictionary<string, IEnumerable<MeasurementViewModel>> measurements)
  {
      Lists.Children.Clear();
      CheckBoxTypes.Children.Clear();

      foreach (var type in measurements.Keys)
      {
          var measurementList = new MeasurementList()
          {
              ViewModels = measurements[type],
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

  public void UpdateLists(IDictionary<string, IEnumerable<MeasurementViewModel>> viewModels)
  {
      var lists = Lists.Children;

      foreach (var list in lists)
      {
          if (list is not MeasurementList measurementList)
          {
              continue;
          }

          measurementList.ViewModels = viewModels[measurementList.Type];
      }
  }

  private void OnSelectedThermometer(object? sender, SelectionChangedEventArgs args)
  {
      var thermometer = args.AddedItems[0]!.ToString();
      //TODO verify null ?

      SelectedThermometer?.Invoke(this, thermometer!);
  }
  private void Timers_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
  {
      if (sender is ComboBox { SelectedItem: (int value, _) })
      {
          SelectedTimer?.Invoke(this, value);
      }
  }
}
