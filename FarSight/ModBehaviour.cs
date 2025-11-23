using UnityEngine;

namespace FarSight;

public class ModBehaviour : Duckov.Modding.ModBehaviour
{
    public float zoomSpeed = 10f;

    private const float BaseDefaultFOV = 23.5f;
    private const float BaseAdsFOV = 22.8f;
    private const float MinDefaultFOV = 1f;
    private const float MinAdsFOV = 0.3f;

    private static GameCamera Camera => GameCamera.Instance;

    private float DeltaFOV => zoomSpeed * Time.deltaTime;

    private float _currentDefaultFOV;
    private float _currentAdsFOV;

    private void Awake()
    {
        Debug.Log("FarSight Loaded!!!");
        _currentDefaultFOV = BaseDefaultFOV;
        _currentAdsFOV = BaseAdsFOV;
    }

    private void Update()
    {
        if (!Camera) return;
        // Zoom out
        if (Input.GetKey(KeyCode.PageUp))
        {
            _currentDefaultFOV += DeltaFOV;
            _currentAdsFOV += DeltaFOV;
        }
        // Zoom in
        if (Input.GetKey(KeyCode.PageDown))
        {
            _currentDefaultFOV -= DeltaFOV;
            _currentAdsFOV -= DeltaFOV;
        }
        // Reset
        if (Input.GetKey(KeyCode.Home))
        {
            _currentDefaultFOV = BaseDefaultFOV;
            _currentAdsFOV = BaseAdsFOV;
        }
        // Avoid negative FOV
        _currentDefaultFOV = Mathf.Max(_currentDefaultFOV, MinDefaultFOV);
        _currentAdsFOV = Mathf.Max(_currentAdsFOV, MinAdsFOV);
        // Apply FOV
        Camera.defaultFOV = _currentDefaultFOV;
        Camera.adsFOV = _currentAdsFOV;
    }
}