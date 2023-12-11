using System.Collections.Generic;
using Avalonia.Controls;
using Stas.Monitor.Presentations;

namespace Stas.Monitor.Views;

public partial class MeasurementList : UserControl
{
    public MeasurementList(IEnumerable<MeasurementViewModel> viewModels, string type)
    {
        InitializeComponent();

        Measurements.ItemsSource = viewModels;
        Title.Text = type;
    }
}

