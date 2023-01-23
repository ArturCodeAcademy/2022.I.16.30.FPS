using UnityEngine;

public class GunHandler : MonoBehaviour
{
    private WeaponBase[] _weapons;
    private int _gunIndex = 0;

    private void Start()
    {
        _weapons = GetComponentsInChildren<WeaponBase>();
        if (_weapons != null || _weapons.Length > 0)
            for (int i = 0; i < _weapons.Length; i++)
                _weapons[i].gameObject.SetActive(i == _gunIndex);
    }

    private void Update()
    {
        if (_weapons == null || _weapons.Length <= 0)
            return;

        ChangeGun();
        if (Input.GetMouseButtonDown(0))
            _weapons[_gunIndex].OnBeginShoot();
        if (Input.GetMouseButtonUp(0))
            _weapons[_gunIndex].OnEndShoot();

        if (Input.GetMouseButtonDown(1))
            _weapons [_gunIndex].StartAiming();
        if (Input.GetMouseButtonUp(1))
            _weapons[_gunIndex].StopAiming();
    }

    private void ChangeGun()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            _gunIndex += Input.mouseScrollDelta.y > 0 ? 1 : 0;

            if (_gunIndex >= _weapons.Length)
                _gunIndex = 0;
            if (_gunIndex < 0)
                _gunIndex = _weapons.Length - 1;

            for (int i = 0; i < _weapons.Length; i++)
                _weapons[i].gameObject.SetActive(i == _gunIndex);
            _weapons[_gunIndex].SetStandartPosition();
        }
    }
}
