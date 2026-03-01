using System;

public class ObservableVariable<T>
{
    public delegate void FuncIn0();
    public delegate void FuncIn1<U>(U v1);
    public delegate void FuncIn2<U>(U v1, U v2);
    public delegate void FuncOut<U>(out U result, U input);

    private T _value;
    private FuncIn0 _onValueChanged0T;
    private FuncIn1<T> _onValueChanged1T;
    private FuncIn2<T> _onValueChanged2T;
    private FuncOut<T> _onPreprocess;

    public T Value { get { return GetValue(); } set { SetValueAndNotify(value); } }

    public ObservableVariable(T value = default)
    {
        this._value = value;
        this._onValueChanged0T = null;
        this._onValueChanged1T = null;
        this._onValueChanged2T = null;
        this._onPreprocess = null;
    }

    private void _Notify(T oldValue, T newValue)
    {
        this._onValueChanged0T?.Invoke();
        this._onValueChanged1T?.Invoke(newValue);
        this._onValueChanged2T?.Invoke(oldValue, newValue);
    }

    private void _Preprocess()
    {
        this._onPreprocess?.Invoke(out this._value, this._value);
    }

    public void Notify()
    {
        _Notify(_value, _value);
    }

    public T GetValue()
    {
        return _value;
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
        var oldValue = this._value;
        this._value = value;
        _Preprocess();
        _Notify(oldValue, this._value);
    }

    public void SetValueWithoutNotify(T value)
    {
        this._value = value;
        _Preprocess();
    }

    public void AddListener(FuncIn0 f) { this._onValueChanged0T += f; }
    public void AddListener(FuncIn1<T> f) { this._onValueChanged1T += f; }
    public void AddListener(FuncIn2<T> f) { this._onValueChanged2T += f; }

    public void RemoveListener(FuncIn0 f) { this._onValueChanged0T -= f; }
    public void RemoveListener(FuncIn1<T> f) { this._onValueChanged1T -= f; }
    public void RemoveListener(FuncIn2<T> f) { this._onValueChanged2T -= f; }

    public void RemoveAllListeners()
    {
        this._onValueChanged0T = null;
        this._onValueChanged1T = null;
        this._onValueChanged2T = null;
    }

    public void AddPreprocessor(FuncOut<T> f) { this._onPreprocess += f; }
    public void RemovePreprocessor(FuncOut<T> f) { this._onPreprocess -= f; }

    public void RemoveAllPreprocessors() { this._onPreprocess = null; }
}
