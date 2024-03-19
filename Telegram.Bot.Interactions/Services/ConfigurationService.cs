using Telegram.Bot.Interactions.Services.Abstraction;

namespace Telegram.Bot.Interactions.Services;

public class ConfigurationService : IConfigurationService
{
    public bool StrictLoadingModeEnabled { get; set; }
    
    
}