using System;
using System.Data.Common;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Serilog;
using Stas.Monitor.Views;
using ClpNet;
using ClpNet.Extensions;
using MySql.Data.MySqlClient;
using Stas.Monitor.Domains;
using Stas.Monitor.Infrastructures;
using Stas.Monitor.Infrastructures.CSV;
using Stas.Monitor.Infrastructures.Database;
using Stas.Monitor.Infrastructures.Ini;
using Stas.Monitor.Presentations;
using Stas.Monitor.Presentations.IViews;

namespace Stas.Monitor.App;

public class App : Application
{
    private MainWindow? _mainWindow;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);

        var log = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        Log.Logger = log;
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var errorWindow = new ErrorWindow();
            try
            {
                var cmdParser = VerifyArguments(desktop.Args ?? Array.Empty<string>());

                _mainWindow = new MainWindow();

                var iniConfigReader = new IniConfigurationReader(cmdParser.GetParameterValue("config-file"));
                var configuration = iniConfigReader.RetrieveConfiguration();

                /*DbProviderFactories.RegisterFactory("MySql.Data.MySqlClient", MySqlClientFactory.Instance);
                var sqlFactory = DbProviderFactories.GetFactory("MySql.Data.MySqlClient");
                var queries = new MySqlQueries();
                var measurementRepository = new DatabaseMeasurementRepository(sqlFactory, queries, configuration.Data);$*/

                var measurementsData = configuration.Data;
                var measurementRepository = new CsvMeasurementRepository(measurementsData["measurements"], measurementsData["alerts"]);

                var measurementTypes = Enum.GetNames(typeof(MeasurementType));

                var measurementsRetriever = new MeasurementsRetriever(measurementRepository);
                var measurementPresenter = new MeasurementPresenter(measurementsRetriever)
                {
                    Types = measurementTypes
                };

                var mainPresenter = new MainPresenter(_mainWindow, measurementPresenter, configuration.ThermometerNames, configuration.TimerHandler);

                DispatcherTimer.Run(() =>
                {
                    mainPresenter.RefreshLists();
                    Log.Logger.Information("Measurements Refreshed.");
                    return true;
                }, TimeSpan.FromSeconds(5));

                desktop.MainWindow = _mainWindow;
            }
            catch (Exception e)
            {
                errorWindow.ErrorMessage = e.Message;
                desktop.MainWindow = errorWindow;
            }

        }

        base.OnFrameworkInitializationCompleted();
    }

    private static CommandLineParser VerifyArguments(string[] args)
    {
        var cmdParser = new CommandLineParser(args);
        if ( !cmdParser.HasParameter("config-file") || string.IsNullOrEmpty(cmdParser.GetParameterValue("config-file")))
        {
            throw new ArgumentException("monitor: missing configuration file argument");
        }

        return cmdParser;
    }
}
