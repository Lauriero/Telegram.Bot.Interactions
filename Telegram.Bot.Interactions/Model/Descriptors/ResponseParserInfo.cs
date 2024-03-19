using Telegram.Bot.Interactions.Model.Responses.Abstraction;
using Telegram.Bot.Interactions.Parsers;
using Telegram.Bot.Interactions.Utilities.Collections;

namespace Telegram.Bot.Interactions.Model.Descriptors;

/// <summary>
/// Contains info about the response parser.
/// Gets generated when the parser is loaded into the service.
/// </summary>
public class ResponseParserInfo : IDefaultSettableEntity
{
    /// <summary>
    /// Type of the parser.
    /// </summary>
    public Type ParserType { get; }
 
    /// <summary>
    /// Type of the response that can be parsed with this parser.
    /// </summary>
    public Type TargetResponseType { get; }
    
    /// <summary>
    /// Specifies whether this parser will be used by default
    /// to parse responses of the <see cref="ParserType"/>.
    /// Only one default parser for type is allowed to be loaded into the registry.
    /// </summary>
    public bool Default { get; }
    
    public IResponseParser<IUserResponse> Instance { get; }
    
    public ResponseParserInfo(Type parserType, Type targetResponseType, bool @default,
        IResponseParser<IUserResponse> instance)
    {
        Default            = @default;
        Instance           = instance;
        ParserType         = parserType;
        TargetResponseType = targetResponseType;
    }
}