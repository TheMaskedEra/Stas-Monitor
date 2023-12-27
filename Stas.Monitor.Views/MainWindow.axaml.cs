using System;
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
  public void SetupSettings(IEnumerable<string> names, IEnumerable<string> displayTimers, string[] types)
  {
    ListName.ItemsSource = names;
    ListName.SelectedIndex = 0;

    Timers.ItemsSource = displayTimers;
    Timers.SelectedIndex = 1;

    GenerateThermometerSettings(types);
  }

  private void GenerateThermometerSettings(string[] types)
  {
      foreach (var type in types)
      {
          var checkBox = new CheckBox()
          {
              Content = type,
              IsChecked = true
          };

          var measurementList = new MeasurementList()
          {
              Type = type,
              Title = type
          };

          checkBox.IsCheckedChanged += (sender, e) =>
          {
              measurementList.IsVisible = (bool)checkBox.IsChecked;
          };

          Lists.Children.Add(measurementList);

          CheckBoxTypes.Children.Add(checkBox);
      }
  }

  public void RefreshLists(IDictionary<string, IList<MeasurementViewModel>> data)
  {
      foreach (var measurementList in Lists.Children.OfType<MeasurementList>())
      {
          var type = measurementList.Type;
          var exists = data.ContainsKey(type);

          measurementList.Items = exists ? data[type] : new List<MeasurementViewModel>();
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
}
