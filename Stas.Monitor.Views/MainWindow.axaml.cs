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
  }
  public void SetNames(ISet<string> names)
  {
    ListName.ItemsSource = names;
    ListName.SelectedIndex = 0;
  }

  public void CreateType(string typeName, IEnumerable<MeasurementViewModel> measurements)
  {

      var measurementList = new MeasurementList(measurements);

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

  /*private void OnSelectedTimer(object? sender, SelectionChangedEventArgs e)
  {
      throw new NotImplementedException();
  }*/
}
