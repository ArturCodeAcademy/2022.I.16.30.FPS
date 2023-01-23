using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(Turret), typeof(Health))]
public class DestroyTurret : MonoBehaviour
{
    [SerializeField, Min(0)] private float _explosionForce;
    [SerializeField] private Transform[] _detachables;
    
    private Health _health;
    private Turret _turret;

    private const float EXPLOSION_RADIUS = 10;

    private void Start()
    {
        _health = GetComponent<Health>();
        _turret = GetComponent<Turret>();
        _health.OnHealthEnd.AddListener(OnEndHealth);
    }

    private void OnEndHealth()
    {
        //foreach (Transform part in _turret.transform) // Fix
        foreach (Transform part in _detachables) // Fix
        {
            part.parent = null;
            if (!part.TryGetComponent(out Rigidbody rigidbody))
                rigidbody = part.gameObject.AddComponent<Rigidbody>();
            rigidbody.AddExplosionForce(_explosionForce, _turret.transform.position, EXPLOSION_RADIUS);
        }
        Destroy(gameObject); // new
    }

    private void OnDestroy()
    {
        _health.OnHealthEnd.RemoveListener(OnEndHealth);
    }
}
