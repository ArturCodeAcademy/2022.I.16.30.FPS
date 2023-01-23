using UnityEngine;

public class Stamina : MonoBehaviour
{
    public StaminaChangedEvent OnStaminaChanged;

    [SerializeField, Min(0)] private float _recoveryScale = 1;
    [SerializeField, Min(0)] private float _maxValue = 10;

    private float _value;
    private bool _usedOnThisFrame = false;

    private void Awake()
    {
        OnStaminaChanged ??= new StaminaChangedEvent();
        _value = _maxValue;
    }

    private void LateUpdate()
    {
        if (_usedOnThisFrame)
        {
            _usedOnThisFrame = false;
        }
        else if (_value < _maxValue)
        {
            _value += Time.deltaTime * _recoveryScale;
            _value = Mathf.Min(_value, _maxValue);
            OnStaminaChanged?.Invoke(GetArgs());
        }
    }

    public StaminaChangedEventArgs GetArgs()
    {
        return new StaminaChangedEventArgs()
        {
            MaxValue = _maxValue,
            Value = _value
        };
    }

    public bool TryUseStamina(float useScale = 1)
    {
        _usedOnThisFrame = true;
        if (_value < Time.deltaTime * useScale)
            return false;

        _value -= Time.deltaTime * useScale;
        _value = Mathf.Max(_value, 0);
        OnStaminaChanged?.Invoke(GetArgs());
        return true;
    }
}
