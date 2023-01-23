using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitRetranslator : MonoBehaviour, IHitable
{
    [SerializeField, Min(0)] private float _hitMultiplier;
    [SerializeField] private Health _health;

    public float Hit(float damage)
    {
        return _health?.Hit(damage * _hitMultiplier) ?? 0;
    }

    public void Kill()
    {
        _health?.Kill();
    }
}
