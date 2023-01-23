using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Stamina))]
public class StaminaUI : MonoBehaviour
{
    [SerializeField] private Slider _staminaSlider;

    private Stamina _stamina;

    private void Awake()
    {
        _stamina = GetComponent<Stamina>();
    }

    private void Start()
    {
        _stamina.OnStaminaChanged.AddListener(UpdateUI);
        UpdateUI(_stamina.GetArgs());
    }

    private void UpdateUI(StaminaChangedEventArgs args)
    {
        _staminaSlider.maxValue = args.MaxValue;
        _staminaSlider.value = args.Value;
    }

    private void OnDisable()
    {
        _stamina.OnStaminaChanged.RemoveListener(UpdateUI);
    }
}
