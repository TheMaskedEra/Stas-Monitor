using NSubstitute;
using Stas.Monitor.Domains;
using Stas.Monitor.Presentations.IPresenters;
using Stas.Monitor.Presentations.IViews;

namespace Stas.Monitor.Presentations.Tests;

public class MainPresenterTests
{
    [Test]
    public void Constructor_ShouldSubscribeToSelectedThermometerEvent()
    {
        // Arrange
        var viewMock = Substitute.For<IMainView>();
        var measurementPresenterMock = Substitute.For<IMeasurementPresenter>();
        var thermometersNames = new[] { "Thermometer1", "Thermometer2" };

        var timerHandler = new TimerHandler(new List<int> { 30, 60, 300});

        // Act
        var mainPresenter = new MainPresenter(viewMock, measurementPresenterMock, thermometersNames, timerHandler);

        // Assert
        viewMock.Received(1).SelectedThermometer += Arg.Any<EventHandler<string>>();
    }

    [Test]
    public void Constructor_ShouldSubscribeToSelectedTimerEvent()
    {
        // Arrange
        var viewMock = Substitute.For<IMainView>();
        var measurementPresenterMock = Substitute.For<IMeasurementPresenter>();
        var thermometersNames = new[] { "Thermometer1", "Thermometer2" };

        var timerHandler = new TimerHandler(new List<int> {30, 60, 300});

        // Act
        var mainPresenter = new MainPresenter(viewMock, measurementPresenterMock, thermometersNames, timerHandler);

        // Assert
        viewMock.Received(1).SelectedTimer += Arg.Any<EventHandler<string>>();
    }

    [Test]
    public void MainPresenter_WhenRefreshLists_ShouldShowOnViewAndDelegateToMeasurementPresenter()
    {
        //Arrange
        var measurementPresenterMock = Substitute.For<IMeasurementPresenter>();
        var mainViewMock = Substitute.For<IMainView>();

        var thermometersNames = new[] { "Living Room", "Bedroom" };
        var timerHandler = new TimerHandler(new List<int> { 30, 60, 300 });
        var mainPresenter = new MainPresenter(mainViewMock, measurementPresenterMock, thermometersNames, timerHandler);

        var measurement = new Measurement("Temperature", DateTime.Now, 25, null);
        var returnedDictionary = new Dictionary<string, IList<MeasurementViewModel>>();
        returnedDictionary.Add("Temperature", new List<MeasurementViewModel> { new (measurement) });

        measurementPresenterMock.RetrieveThermometerMeasurements().Returns(returnedDictionary);

        //Act
        mainPresenter.RefreshLists();

        //Assert
        measurementPresenterMock.Received(1).RetrieveThermometerMeasurements();
        mainViewMock.Received(1).RefreshLists(returnedDictionary);
    }

    [Test]
    public void MainPresenter_WhenThermometerSelected_ShouldChangeViewInformationAndRefreshList()
    {
        //Arrange
        var measurementPresenterMock = Substitute.For<IMeasurementPresenter>();
        var mainViewMock = Substitute.For<IMainView>();

        var thermometersNames = new[] { "Living Room", "Bedroom" };
        var timerHandler = new TimerHandler(new List<int> { 30, 60, 300 });
        var mainPresenter = new MainPresenter(mainViewMock, measurementPresenterMock, thermometersNames, timerHandler);

        //Act
        mainViewMock.SelectedThermometer += Raise.Event<EventHandler<string>>(mainViewMock, thermometersNames[0]);

        //Assert
        measurementPresenterMock.Received(1).CurrentThermometer = thermometersNames[0];
        measurementPresenterMock.Received(1).RetrieveThermometerMeasurements();
        mainViewMock.Received(1).RefreshLists(Arg.Any<IDictionary<string, IList<MeasurementViewModel>>>());
    }

    [Test]
    public void MainPresenter_WhenTimerSelected_ShouldChangeRefreshListAndChangeMeasurementPresenter()
    {
        //Arrange
        var measurementPresenterMock = Substitute.For<IMeasurementPresenter>();
        var mainViewMock = Substitute.For<IMainView>();

        var thermometersNames = new[] { "Living Room", "Bedroom" };
        var timerHandler = new TimerHandler(new List<int> { 30, 60, 300 });
        var displays = timerHandler.DisplayTimers.ToList();
        var mainPresenter = new MainPresenter(mainViewMock, measurementPresenterMock, thermometersNames, timerHandler);

        //Act
        mainViewMock.SelectedTimer += Raise.Event<EventHandler<string>>(mainViewMock, displays[1]);

        //Assert
        measurementPresenterMock.Received(1).CurrentTimer = timerHandler.GetTimer(displays[1]).TotalSeconds;
        measurementPresenterMock.Received(1).RetrieveThermometerMeasurements();
        mainViewMock.Received(1).RefreshLists(Arg.Any<IDictionary<string, IList<MeasurementViewModel>>>());
    }
}
