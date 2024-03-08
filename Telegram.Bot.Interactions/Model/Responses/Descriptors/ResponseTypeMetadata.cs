using Telegram.Bot.Interactions.Model.Responses.Abstraction;

namespace Telegram.Bot.Interactions.Model.Responses.Descriptors;

/// <summary>
/// Is used to map the values of <see cref="InteractionResponseType"/>,
/// types of the implementations of <see cref="IInteractionResponseConfig{TResponse}"/>
/// and types of the implementations of <see cref="IUserResponse"/>.
/// </summary>
public class ResponseTypeMetadata :
    IEquatable<ResponseTypeMetadata>,
    IComparable<ResponseTypeMetadata>
{
    /// <summary>
    /// The default configuration type - implementation
    /// of the <see cref="IInteractionResponseConfig{TResponse}"/>
    /// that will be used as a default for this response type.
    /// </summary>
    public Type? DefaultConfigType { get; }
    
    /// <summary>
    /// Implementation of the <see cref="IUserResponse"/> that correlates with the response type.
    /// </summary>
    public Type ResponseType { get; }
    
    /// <summary>
    /// Basic type that is used to identify the type.
    /// </summary>
    public InteractionResponseType BasicType { get; }
    
    public ResponseTypeMetadata(InteractionResponseType basicType, Type responseType, Type? defaultConfigType)
    {
        BasicType = basicType;
        DefaultConfigType = defaultConfigType;
        ResponseType = responseType;
    }

    public bool Equals(ResponseTypeMetadata? other)
    {
        if (ReferenceEquals(null, other)) {
            return false;
        }

        if (ReferenceEquals(this, other)) {
            return true;
        }

        return BasicType == other.BasicType;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) {
            return false;
        }

        if (ReferenceEquals(this, obj)) {
            return true;
        }

        if (obj.GetType() != this.GetType()) {
            return false;
        }

        return Equals((ResponseTypeMetadata) obj);
    }

    public override int GetHashCode()
    {
        return (int) BasicType;
    }

    public int CompareTo(ResponseTypeMetadata? other)
    {
        if (ReferenceEquals(this, other)) {
            return 0;
        }

        if (ReferenceEquals(null, other)) {
            return 1;
        }

        return BasicType.CompareTo(other.BasicType);
    }
}