    using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Microsoft.Win32;
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

  private void OnSelectedThermometer(string e)
  {
      CheckBoxTypes.Children.Clear();
      MeasurementLists.Children.Clear();
      SelectedThermometer?.Invoke(this, e);
  }
}
