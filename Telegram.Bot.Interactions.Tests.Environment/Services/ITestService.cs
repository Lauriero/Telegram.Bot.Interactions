namespace Telegram.Bot.Interactions.Tests.Environment.Services;

public interface ITestService
{
    const int TEST_VARIABLE_VALUE = 1;

    public int Test { get; set; }
}