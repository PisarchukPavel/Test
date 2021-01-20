using System;
using UnityEngine;

public abstract class Variable<T> : ScriptableObject
{
    public event Action<T> OnChanged;
    
    public T Value
    {
        get => _value;
        set => SetValue(value);
    }

    [SerializeField]
    private T _value = default;
    
    private void SetValue(T newValue)
    {
        _value = newValue;
        OnChanged?.Invoke(_value);
    }
    
    public static implicit operator T(Variable<T> variable)
    {
        return variable._value;
    }
}
