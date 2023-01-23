using System.Collections;
using UnityEngine;

public class TurretTrail : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private float _duration;

    private float _time = -1;
    private Vector3 _begin;
    private Vector3 _target;

    public void Setup(Vector3 target)
    {
        _begin = transform.position;
        _target = target;
        _time = 0;
    }

    private void Update()
    {
        if (_time < 0 || _time > _duration)
            return;

        _time += Time.deltaTime;
        transform.position = Vector3.Lerp(_begin, _target, _time / _duration);
    }
}
