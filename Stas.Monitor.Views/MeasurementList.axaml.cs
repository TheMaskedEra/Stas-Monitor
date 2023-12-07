using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Stas.Monitor.Presentations;

namespace Stas.Monitor.Views;

public partial class MeasurementList : UserControl
{
    public MeasurementList(IEnumerable<MeasurementViewModel> viewModels)
    {
        InitializeComponent();

        Measurements.ItemsSource = viewModels;
    }
}

