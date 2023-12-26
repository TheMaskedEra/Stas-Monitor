using System.Collections.Generic;
using Avalonia.Controls;
using Stas.Monitor.Presentations;

namespace Stas.Monitor.Views;

public partial class MeasurementList : UserControl
{
    public string Title
    {
        get;
    }

    public IEnumerable<MeasurementViewModel> Items
    {
        set => Measurements.ItemsSource = value;
    }

    public MeasurementList(string type)
    {
        InitializeComponent();
        Header.Text = type;
        Title = type;
    }
}

