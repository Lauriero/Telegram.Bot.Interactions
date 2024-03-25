namespace Telegram.Bot.Interactions.Utilities.Extensions;

public static class TypeExtensions
{
    /// <summary>
    /// If type is derived from generic type which definition is specified in a parameter,
    /// returns the type arguments that were set primarily on the implementation of the generic
    /// interface definition.
    /// If it is not derived, returns null.
    /// </summary>
    public static Type[]? GetParentGenericTypeArguments(this Type type, Type genericParentDefinition)
    {
        return type
            .GetInterfaces()
            .FirstOrDefault(i => 
                i.IsGenericType &&
                i.GetGenericTypeDefinition()
                 .IsEquivalentTo(genericParentDefinition))
            ?.GenericTypeArguments;
    }
    
    public static bool IsSubclassOfRawGeneric(this Type type, Type genericParentDefinition)
    {
        return type
            .GetInterfaces()
            .Any(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition()
                 .IsEquivalentTo(genericParentDefinition));
    }
}