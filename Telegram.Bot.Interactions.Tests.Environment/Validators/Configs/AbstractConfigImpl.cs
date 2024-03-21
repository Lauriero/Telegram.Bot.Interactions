using Telegram.Bot.Interactions.Tests.Environment.Responses;

namespace Telegram.Bot.Interactions.Tests.Environment.Validators.Configs;

public class AbstractConfigImpl : IAbstractConfig<IAbstractResponse>
{
    public string TestParameter => "Test";
}