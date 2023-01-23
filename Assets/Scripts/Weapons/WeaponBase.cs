using System.Collections;
using System.Linq;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [SerializeField] protected Transform _hole;
    [SerializeField, Min(0.05f)] protected float _fireRate;
    [SerializeField, Min(0f)] protected float _damage;

    [Header("Effects")]
    [SerializeField] protected Transform _shootParticlesPrefab;
    [SerializeField] protected Transform _hitParticlesPrefab;
    [SerializeField] protected Transform _shootHitHolePrefab;
    [SerializeField, Min(1)] protected float _holeLifeTime;

    [Header("Positions")]
    [SerializeField] protected Vector3 _standartPosition;
    [SerializeField] protected Vector3 _aimingPosition;
    [SerializeField] protected AnimationCurve _aiminLerpCurve;
    [SerializeField, Range(0.01f, 2)] protected float _changingDuration;

    protected Transform _camera;
    protected Coroutine _shootingCoroutine;
    protected bool _canShoot = true;
    protected bool _isAiming = false;

    private float _aimingTime = 0;

    private void Awake()
    {
        _camera = Camera.main.transform;
        _aimingTime = _changingDuration;
        SetStandartPosition();
    }

    protected virtual void Update()
    {
        Vector3 startPos = _isAiming ? _standartPosition : _aimingPosition;
        Vector3 targetPos = _isAiming ? _aimingPosition : _standartPosition;

        if (_aimingTime >= _changingDuration)
            return;

        _aimingTime += Time.deltaTime;
        float lerp = Mathf.Clamp(_aimingTime / _changingDuration, 0, 1);
        lerp = Mathf.Clamp(_aiminLerpCurve.Evaluate(lerp), 0, 1); ;
        transform.localPosition = Vector3.Lerp(startPos, targetPos, lerp);
    }

    public void StartAiming()
    {
        _isAiming = true;
        _aimingTime = 0;
    }

    public void StopAiming()
    {
        _isAiming = false;
        _aimingTime = 0;
    }

    public void SetStandartPosition()
    {
        transform.localPosition = _standartPosition;
    }

    public virtual void OnBeginShoot()
    {
        if (_canShoot == false)
            return;
        _canShoot = false;

        _shootingCoroutine = StartCoroutine(ShootCoroutine());
    }

    public virtual void OnEndShoot()
    {
        StopCoroutine(_shootingCoroutine);
        _canShoot = true;
    }

    protected virtual IEnumerator ShootCoroutine()
    {
        WaitForSeconds wait = new WaitForSeconds(_fireRate);
        Shoot();
        yield return wait;
    }

    protected virtual void Shoot()
    {
        RaycastHit[] hits = Physics.RaycastAll(_camera.position, _camera.forward)
            .OrderByDescending(x => Vector3.Distance(x.point, _camera.position)).ToArray();

        for (int i = hits.Length - 1; i >= 0; i--)
        {
            if (hits[i].transform == Player.Instance.transform)
                continue;

            if (hits[i].transform.TryGetComponent(out IHitable hitable)) // new // Fix
            {
                hitable.Hit(_damage);
            }
            var hole = Instantiate(_shootHitHolePrefab, hits[i].point, Quaternion.identity);
            hole.forward = hits[i].normal;
            hole.transform.parent = hits[i].transform;
            if (_shootParticlesPrefab != null)
                Instantiate(_shootParticlesPrefab, _hole.position, _hole.rotation);
            if (hole.TryGetComponent(out HitHole hitHole))
                hitHole.Setup(_holeLifeTime);
            else
                Destroy(hole.gameObject, _holeLifeTime);
            var hitEffect = Instantiate(_hitParticlesPrefab, hits[i].point, Quaternion.identity);
            hitEffect.forward = hits[i].normal;

            break;
        }
    }
}
