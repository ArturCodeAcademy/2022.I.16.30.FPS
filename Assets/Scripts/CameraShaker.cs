using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraShaker : MonoBehaviour
{
    [SerializeField] private float _shakeAmplitude = 1.2f;
    [SerializeField] private float _shakeFrequency = 2.0f;

    private CinemachineVirtualCamera _cvc;
    private CinemachineBasicMultiChannelPerlin _noise;

    private void Awake()
    {
        _cvc = GetComponent<CinemachineVirtualCamera>();
        _noise = _cvc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Start()
    {
        SetActiveShaker(false);
    }

    public void SetActiveShaker(bool active = true)
    {
        _noise.m_AmplitudeGain = active ? _shakeAmplitude : 0;
        _noise.m_FrequencyGain = active ? _shakeFrequency : 0;
    }
}
