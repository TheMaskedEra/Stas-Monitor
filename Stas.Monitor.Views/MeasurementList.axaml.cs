using System.Collections.Generic;
using Avalonia.Controls;
using Stas.Monitor.Presentations;

namespace Stas.Monitor.Views;

public partial class MeasurementList : UserControl
{
    public string Type
    {
        get;
        init;
    } = "None";

    public string Title
    {
        init => Header.Text = value;
    }

    public IEnumerable<MeasurementViewModel> Items
    {
        set => Measurements.ItemsSource = value;
    }

    public MeasurementList()
    {
        InitializeComponent();
    }
}

