using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Stas.Monitor.Presentations;
using Stas.Monitor.Presentations.IViews;

namespace Stas.Monitor.Views;

public partial class MainWindow : Window, IMainView
{
  public event EventHandler<string>? SelectedThermometer;

  public event EventHandler<int>? SelectedTimer;

  public IDictionary<string, IList<MeasurementViewModel>> MeasurementLists { get; set; } //TODO donner des valeurs par défaut ?

  public MainWindow()
  {
    InitializeComponent();

    MeasurementLists = new Dictionary<string, IList<MeasurementViewModel>>();
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
          var measurementList = new MeasurementList(measurements[type], type);

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
      //TODO verify null

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
