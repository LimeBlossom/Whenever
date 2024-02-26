

using System.Collections.Generic;

public interface IReadonlyFallbackNames<TKey, TValue>
{
    /// <summary>
    /// Get the fallback value for a given key, or the immediate fallback if the immediate fallback has a higher priority.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="immediateFallback"></param>
    /// <param name="immediateFallbackPriority"></param>
    /// <returns></returns>
    (TValue val, int priority) GetFallbackValue(TKey key, TValue immediateFallback, int immediateFallbackPriority);
    
    IEnumerable<TKey> Keys { get; }
    (TValue val, int priority)? GetFallbackWithPriority(TKey key);
}

public interface IFallbackNames<TKey, TValue> : IReadonlyFallbackNames<TKey, TValue>
{
    /// <summary>
    /// Adds a fallback value for a given key, with a priority.
    /// If a fallback value already exists for the key, it is only replaced if the new priority is higher.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="fallbackValue"></param>
    /// <param name="priority"></param>
    void AddFallback(TKey key, TValue fallbackValue, int priority);
    
    
}

public class FallbackNames<TKey, TVal> : IFallbackNames<TKey, TVal>
{
    private Dictionary<TKey, FallbackWithPriority> fallbacks = new Dictionary<TKey, FallbackWithPriority>();
    
    private struct FallbackWithPriority
    {
        public TVal fallbackValue;
        public int priority;
    }
    
    public FallbackNames()
    {
    }
    
    public FallbackNames(IReadonlyFallbackNames<TKey, TVal> copyFrom)
    {
        foreach (var key in copyFrom.Keys)
        {
            var fallbackDetails = copyFrom.GetFallbackWithPriority(key);
            if(fallbackDetails is null) continue;
            var (fallback, priority) = fallbackDetails.Value;
            fallbacks[key] = new FallbackWithPriority
            {
                fallbackValue = fallback,
                priority = priority
            };
        }
    }

    public (TVal val, int priority) GetFallbackValue(TKey key, TVal immediateFallback, int immediateFallbackPriority)
    {
        if (!fallbacks.TryGetValue(key, out var fallback)) return (immediateFallback, immediateFallbackPriority);
        
        if (fallback.priority > immediateFallbackPriority)
        {
            return (fallback.fallbackValue, fallback.priority);
        }
        return (immediateFallback, immediateFallbackPriority);
    }

    public IEnumerable<TKey> Keys => fallbacks.Keys;
    public (TVal val, int priority)? GetFallbackWithPriority(TKey key)
    {
        var val = GetFallbackValue(key, default, int.MinValue);
        if(val.priority == int.MinValue) return null;
        return val;
    }

    public void AddFallback(TKey key, TVal fallbackValue, int priority)
    {
        var newFallback = new FallbackWithPriority
        {
            fallbackValue = fallbackValue,
            priority = priority
        };
        
        if (!fallbacks.TryGetValue(key, out var existingFallback))
        {
            fallbacks[key] = newFallback;
            return;
        }
        if (priority > existingFallback.priority)
        {
            fallbacks[key] = newFallback;
        }
    }
}

