using System.Collections.Generic;
using Avalonia.Controls;
using Stas.Monitor.Presentations;

namespace Stas.Monitor.Views;

public partial class MeasurementList : UserControl
{
    public IEnumerable<MeasurementViewModel> ViewModels
    {
        set => Measurements.ItemsSource = value;
    }

    public string? Type
    {
        get => Title.Text;
        init => Title.Text = value;
    }

    public MeasurementList()
    {
        InitializeComponent();
    }
}

