using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : WeaponBase
{
    [SerializeField, Min(0.05f)] protected float _fireRateInThreeShootsMode;

    private FireMode _fireMode;

    void Start()
    {
        _fireMode = FireMode.SingleShoot;
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetMouseButtonDown(2))
            _fireMode = FireMode.SingleShoot == _fireMode
                ? FireMode.ThreeShoots
                : FireMode.SingleShoot;
    }

    protected override IEnumerator ShootCoroutine()
    {
        WaitForSeconds wait;
        if (_fireMode == FireMode.SingleShoot)
            wait = new WaitForSeconds(_fireRate);
        else
            wait = new WaitForSeconds(_fireRateInThreeShootsMode);

        for (int i = 0; i < (int)_fireMode; i++)
        {
            Shoot();
            yield return wait;
        }
    }

    enum FireMode
    {
        SingleShoot = 1,
        ThreeShoots = 3
    }
}
