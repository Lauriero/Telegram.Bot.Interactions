namespace Telegram.Bot.Interactions.Model.Descriptors;

/// <summary>
/// Contains info about the response validator.
/// Is generated when the validator is loaded into the service.
/// </summary>
public class ResponseValidatorInfo
{
    /// <summary>
    /// Type of the validator.
    /// </summary>
    public Type ValidatorType { get; }
 
    /// <summary>
    /// Type of the response that can be validated with this parser.
    /// </summary>
    public Type TargetResponseType { get; }
    
    public bool Default { get; }

    public ResponseValidatorInfo(Type validatorType, Type targetResponseType, bool @default)
    {
        ValidatorType      = validatorType;
        TargetResponseType = targetResponseType;
        Default       = @default;
    }
}
