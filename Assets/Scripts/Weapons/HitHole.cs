using UnityEngine;

public class HitHole : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _hitSpriteRenderer;
    [SerializeField, Min(0)] private float _hideDuration;

    private float _holeLifeTime;
    private float _leftTimeToHide;

    public void Setup(float holeLifeTime)
    {
        _holeLifeTime = holeLifeTime;
        _leftTimeToHide = _hideDuration;
    }

    private void Update()
    {
        _holeLifeTime -= Time.deltaTime;

        if (_holeLifeTime <= 0)
        {
            _leftTimeToHide -= Time.deltaTime;
            if (_leftTimeToHide <= 0)
                Destroy(gameObject);

            Color color = _hitSpriteRenderer.color;
            color.a = _leftTimeToHide / _hideDuration;
            _hitSpriteRenderer.color = color;
        }
    }
}
