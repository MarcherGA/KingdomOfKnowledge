using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class FlashingLight : MonoBehaviour
{
    [SerializeField] private float _minIntensity;
    [SerializeField] private float _maxIntensity;
    [SerializeField] private float flashInterval;


    private Light2D _light;

    void Start()
    {
        _light = GetComponent<Light2D>();
    }

    void Update()
    {
        // Calculate the intensity using a sine wave
        _light.intensity = Mathf.Lerp(_minIntensity, _maxIntensity, (Mathf.Sin(2 * Mathf.PI * Time.time / flashInterval) + 1) / 2);

    }
}
