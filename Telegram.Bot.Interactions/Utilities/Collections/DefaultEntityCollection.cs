using System.Collections;
using System.Collections.ObjectModel;

namespace Telegram.Bot.Interactions.Utilities.Collections;

/// <summary>
/// Read-only collection to store entities that can be marked as default.
/// Implements basic default entity getter, that returns entity that has been
/// marked as a default, or the first one in the collection, or null, if there's no
/// elements in the collection. 
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class DefaultEntityCollection<TEntity> : IReadOnlyList<TEntity>
    where TEntity : class, IDefaultSettableEntity
{
    private readonly IReadOnlyList<TEntity> _internal;

    /// <summary>
    /// Contains the parser marked by default if any.
    /// If not, contains the first one in the collection.
    /// If the collection is empty, returns null.
    /// </summary>
    public TEntity? Default
    {
        get {
            if (!_internal.Any()) {
                return null;
            }

            foreach (TEntity entity in _internal) {
                if (entity.Default) {
                    return entity;
                }
            }

            return _internal[0];
        }
    }
    
    public DefaultEntityCollection(IList<TEntity> list)
    {
        _internal = new ReadOnlyCollection<TEntity>(list);
    }

    public IEnumerator<TEntity> GetEnumerator() => 
        _internal.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int Count => _internal.Count;

    public TEntity this[int index] => _internal[index];
}