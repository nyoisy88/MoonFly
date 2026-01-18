using System;
using System.Collections.Generic;

public static class SignalBus
{
    public static Dictionary<Type, List<Delegate>> listeners = new();

    public static void Subcribe<T>(Action<T> callback)
    {
        var type = typeof(T);
        if (!listeners.TryGetValue(type, out var list))
        {
            list = new List<Delegate>();
            listeners[type] = list;
        }
        list.Add(callback);
    }

    public static void Unsubcribe<T>(Action<T> callback)
    {
        var type = typeof(T);
        if (listeners.TryGetValue(type,out var list))
        {
            list.Remove(callback);
        }
    }

    public static void Fire<T>(T signal)
    {
        var type = typeof (T);
        if (!listeners.TryGetValue(type, out var list)) return;

        foreach (var listener in list.ToArray()){
            ((Action<T>)listener).Invoke(signal);
        }
    }
}
