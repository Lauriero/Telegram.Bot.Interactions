using Telegram.Bot.Interactions.Exceptions.Interactions;
using Telegram.Bot.Interactions.Model.Responses.Abstraction;
using Telegram.Bot.Interactions.Model.Responses.Implementation;

namespace Telegram.Bot.Interactions.Validators;

/// <summary>
/// Validates the response.
/// </summary>
/// <remarks>
/// This interface provides generic type unsafe methods and to implement
/// custom validators it is advised to use <see cref="ResponseValidator{TResponse}"/>.
/// Custom implementations of the validators using this interface will cause a warning.
/// </remarks>
/// <typeparam name="TResponse">
/// Acts as a warrant that the response in the <see cref="ValidateResponseAsync"/> method
/// will be of that type, and the config will also be for only that type.
/// That warranty should be implemented by interaction processor.
/// </typeparam>
public interface IResponseValidator<out TResponse>  
    where TResponse : IUserResponse
{
    /// <summary>
    /// Validates the response returning the validation result.
    /// Uses configuration that has been set up in the
    /// <see cref="BasicResponseModel{TResponse}"/> that
    /// requested to use this validator.
    /// </summary>
    /// <exception cref="ConfigurationNotSupportedException{TResponse}">
    /// Is occurred when this validator cannot handle the provided response type.
    /// </exception>
    public ValueTask<bool> ValidateResponseAsync(IUserResponse response);
}