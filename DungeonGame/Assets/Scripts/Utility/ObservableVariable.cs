using System;

public class ObservableVariable<T>
{
    
    private T _value;
    private Action<T> _onValueChanged1T;
    private Action<T, T> _onValueChanged2T;

    public T Value { get { return GetValue(); } set { SetValueAndNotify(value); } }

    public T ValueRaw { get { return GetValue(); } set { SetValueWithoutNotify(value); } }

    public ObservableVariable(T value = default)
    {
        this._value = value;
        this._onValueChanged1T = null;
        this._onValueChanged2T = null;
    }

    private void Notify(T oldValue, T newValue)
    {
        this._onValueChanged1T?.Invoke(newValue);
        this._onValueChanged2T?.Invoke(oldValue, newValue);
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

    public void AddListener(Action<T> fn)
    {
        _onValueChanged1T += fn;
    }

    public void AddListener(Action<T, T> fn)
    {
        _onValueChanged2T += fn;
    }

    public void RemoveListener(Action<T> fn)
    {
        _onValueChanged1T -= fn;
    }

    public void RemoveListener(Action<T, T> fn)
    {
        _onValueChanged2T -= fn;
    }

    public void RemoveAllListeners()
    {
        _onValueChanged1T = null;
        _onValueChanged2T = null;
    }

}
