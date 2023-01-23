using System.Collections;
using UnityEngine;

public class Rifle : WeaponBase
{
    protected override IEnumerator ShootCoroutine()
    {
        WaitForSeconds wait = new WaitForSeconds(_fireRate);
        while (true)
        {
            Shoot();
            yield return wait;
            _canShoot = true;
        }
    }
}
