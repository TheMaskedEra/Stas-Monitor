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
using Stas.Monitor.Domains.Repository;
using Stas.Monitor.Infrastructures;
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

        Log.Logger = log; // Définit le log dans un singleton (Beurk) TODO
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {

            _mainWindow = new MainWindow();
            var presenter = SetupApp(desktop.Args ?? Array.Empty<string>(), _mainWindow);

            desktop.MainWindow = _mainWindow;

            DispatcherTimer.Run(() =>
            {
                presenter.RefreshLists();
                Log.Logger.Information("You should starts checking for file updates here");
                return true;
            }, TimeSpan.FromSeconds(5));
        }

        base.OnFrameworkInitializationCompleted();
    }

    private MainPresenter SetupApp(string[] args, IMainView view)
    {
      var cmdParser = new CommandLineParser(args);
      if ( !cmdParser.HasParameter("config-file") || string.IsNullOrEmpty(cmdParser.GetParameterValue("config-file")))
      {
        throw new ArgumentException("monitor: missing configuration file argument");
      }

      IConfigurationReader repository = new IniConfigurationReader(cmdParser.GetParameterValue("config-file"));
      var configuration = repository.RetrieveConfiguration();

      DbProviderFactories.RegisterFactory("MySql.Data.MySqlClient", MySqlClientFactory.Instance);
      var sqlFactory = DbProviderFactories.GetFactory("MySql.Data.MySqlClient");
      var measurementRepository = new MeasurementRepository(sqlFactory, configuration.Data);

      var measurementPresenter = new MeasurementPresenter(measurementRepository);
      var mainPresenter = new MainPresenter(view, measurementPresenter, configuration.ThermometerNames, configuration.Timers);

      return mainPresenter;
    }
}
