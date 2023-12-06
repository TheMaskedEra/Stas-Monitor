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
  //TODO Delegate for events
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

  public void ShowMeasurements(IEnumerable<MeasurementViewModel> measures)
  {

  } //Measurements.ItemsSource = measures;

  public void CreateType(string typeName)
  {

      var textBlockName = new TextBlock { Text = typeName };

      var stackBlock = new StackPanel();
      stackBlock.Children.Add(textBlockName);

      Measurements.Children.Add(stackBlock);

      var checkBox = new CheckBox
      {
          Content = typeName,
          IsChecked = true
      };
      checkBox.IsCheckedChanged += (sender, e) =>
      {
          stackBlock.IsVisible = (bool)checkBox.IsChecked;
      };
      CheckBoxTypes.Children.Add(checkBox);
  }

  private void OnSelectedThermometer(string e)
  {
      CheckBoxTypes.Children.Clear();
      Measurements.Children.Clear();
    SelectedThermometer?.Invoke(this, e);
  }
}
