using System.Collections.Generic;
using Avalonia.Controls;
using Stas.Monitor.Presentations;

namespace Stas.Monitor.Views;

public partial class MeasurementList : UserControl
{
    public string Title
    {
        get;
        init;
    } = "None";

    public IEnumerable<MeasurementViewModel> Items
    {
        set => Measurements.ItemsSource = value;
    }

    public MeasurementList()
    {
        InitializeComponent();
    }
}

