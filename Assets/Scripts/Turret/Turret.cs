using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Params")]
    [SerializeField, Min(0)] private float _rotionSpeed;
    [SerializeField, Min(0)] private float _cooldownDuration;
    [SerializeField, Min(0)] private float _damage;
    [SerializeField, Min(0)] private float _viewDistance;

    [Header("Turret")]
    [SerializeField] private Quaternion _defaultPylonRotation;
    [SerializeField] private Transform _pylon;
    [SerializeField] private Transform _shootHole;

    [Header("Prefabs")]
    [SerializeField] private GameObject _shootEffectPrefab;
    [SerializeField] private TrailRenderer _bulletTrail;

    private Transform _target;
    private float _recoveryTime;
    private bool _canShoot = true;

    private void Start()
    {
        _target = Player.Instance.transform;
    }

    private void Update()
    {
        Recovery();
        Rotate();
        TryShoot();
    }

    private void Recovery()
    {
        if (_canShoot)
            return;

        _recoveryTime -= Time.deltaTime;
        if (_recoveryTime <= 0)
            _canShoot = true;
    }

    private void TryShoot()
    {
        if (!_canShoot)
            return;

        if (Physics.Raycast(_shootHole.position, _shootHole.forward, out RaycastHit hit))
        {
            if (hit.transform == _target.transform)
            {
                _canShoot = false;
                _recoveryTime = _cooldownDuration;
                if (_target.TryGetComponent(out IHitable hitable))
                {
                    hitable.Hit(_damage);
                }
                if (_shootEffectPrefab != null)
                {
                    Instantiate(_shootEffectPrefab, _shootHole.position, _shootHole.rotation);
                }
                if (_bulletTrail != null)
                {
                    TrailRenderer trail = Instantiate(_bulletTrail, _shootHole.position, Quaternion.identity);
                    trail.GetComponent<TurretTrail>().Setup(hit.point);
                }
            }
        }
    }

    private void Rotate()
    {
        Quaternion targetRot;
        if (Vector3.Distance(_target.position, transform.position) <= _viewDistance)
            targetRot = Quaternion.FromToRotation(transform.forward, _target.position - transform.position);
        else
            targetRot = _defaultPylonRotation;

        Vector3 rot = targetRot.eulerAngles;
        rot.x = 0;
        rot.z = 0;
        targetRot.eulerAngles = rot;

        _pylon.rotation = Quaternion.RotateTowards(_pylon.rotation, targetRot, Time.deltaTime * _rotionSpeed);
    }

#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, _viewDistance);
    }

    private void OnValidate()
    {
        if (_pylon != null)
            _pylon.rotation = _defaultPylonRotation;
    }

#endif
}
