using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Serilog;
using Stas.Monitor.Views;
using ClpNet;
using ClpNet.Extensions;
using Stas.Monitor.Domains;
using Stas.Monitor.Infrastructures;
using Stas.Monitor.Presentations;

namespace Stas.Monitor.App;

public class App : Application
{
    private MainWindow? _mainWindow;

    //TODO commit les fichiers debug ?
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);

        var log = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        Log.Logger = log; // Définit le log dans un singleton (Beurk)
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {

            var mainPresenter = SetupApp(desktop.Args ?? Array.Empty<string>());
            _mainWindow = new MainWindow(mainPresenter);

            desktop.MainWindow = _mainWindow;
            
            DispatcherTimer.Run(() =>
            {
                Log.Logger.Information("You should starts checking for file updates here");
                return true;
            }, TimeSpan.FromSeconds(1));
        }

        base.OnFrameworkInitializationCompleted();
    }

    private MainPresenter SetupApp(string[] args)
    {
      var cmdParser = new CommandLineParser(args);
      if ( !cmdParser.HasParameter("config-file") || string.IsNullOrEmpty(cmdParser.GetParameterValue("config-file")))
      {
        throw new ArgumentException("monitor: missing configuration file argument");
      }

      IConfigurationReader reader = new IniConfigurationReader(cmdParser.GetParameterValue("config-file"));
      var configuration = reader.GetData();
      return new MainPresenter(configuration);
    }
}