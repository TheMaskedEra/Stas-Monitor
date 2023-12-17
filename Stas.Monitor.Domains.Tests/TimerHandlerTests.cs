namespace Stas.Monitor.Domains.Tests;

public class TimerHandlerTests
{
    [Test]
    public void TimerHandler_GetTimer_ShouldReturnCorrectTimeSpan()
    {
        // Arrange
        IEnumerable<int> values = new List<int> { 60, 120, 180, 3600 };
        var timerHandler = new TimerHandler(values);

        // Act
        var timer1 = timerHandler.GetTimer("1m 0s");
        var timer2 = timerHandler.GetTimer("2m 0s");
        var timer3 = timerHandler.GetTimer("3m 0s");
        var timer4 = timerHandler.GetTimer("1h 0m 0s");

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(timer1, Is.EqualTo(TimeSpan.FromMinutes(1)));
            Assert.That(timer2, Is.EqualTo(TimeSpan.FromMinutes(2)));
            Assert.That(timer3, Is.EqualTo(TimeSpan.FromMinutes(3)));
            Assert.That(timer4, Is.EqualTo(TimeSpan.FromHours(1)));
        });
    }

    [Test]
    public void TimerHandler_DisplayTimer_ShouldBeConverted()
    {
        //Arrange
        IEnumerable<int> values = new List<int> { 60, 120, 180, 3600 };
        var timerHandler = new TimerHandler(values);

        //Act
        const string display1 = "1m 0s";
        const string display2 = "2m 0s";
        const string display3 = "3m 0s";
        const string display4 = "1h 0m 0s";

        var displays = timerHandler.DisplayTimers.ToList();

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(displays, Does.Contain(display1));
            Assert.That(displays, Does.Contain(display2));
            Assert.That(displays, Does.Contain(display3));
            Assert.That(displays, Does.Contain(display4));
        });
    }

    [Test]
    public void TimerHandler_ShouldReturnTimerZeroOnUnregisteredValue()
    {
        //Arrange
        IEnumerable<int> values = new List<int> { 60, 120, 180, 3600 };
        var timerHandler = new TimerHandler(values);

        //Act
        var zeroValue = TimeSpan.Zero;
        var unregisteredValue = timerHandler.GetTimer("5h 0m 0s");

        //Assert
        Assert.That(zeroValue, Is.EqualTo(unregisteredValue));
    }
}
