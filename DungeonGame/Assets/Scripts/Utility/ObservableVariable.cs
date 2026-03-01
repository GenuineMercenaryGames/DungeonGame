using System;

public class ObservableVariable<T>
{
    
    private T _value;
    public Action<T, T> OnValueChanged;

    public T Value { get { return GetValue(); } set { SetValueAndNotify(value); } }

    public T ValueRaw { get { return GetValue(); } set { SetValueWithoutNotify(value); } }

    public ObservableVariable(T value = default)
    {
        this._value = value;
        this.OnValueChanged = null;
    }

    private void Notify(T oldValue, T newValue)
    {
        this.OnValueChanged?.Invoke(oldValue, newValue);
    }

    public void Notify()
    {
        Notify(_value, _value);
    }

    public void SetValue(T value, bool notify = true)
    {
        if (notify)
        {
            SetValueAndNotify(value);
        }
        else
        {
            SetValueWithoutNotify(value);
        }
    }

    public void SetValueAndNotify(T value)
    {
        var oldValue = _value;
        var newValue = value;
        _value = newValue;
        Notify(oldValue, newValue);
    }

    public void SetValueWithoutNotify(T value)
    {
        _value = value;
    }

    public T GetValue()
    {
        return _value;
    }
}
