using UnityEngine;

namespace FarSight;

public class ModBehaviour : Duckov.Modding.ModBehaviour
{
    private const float ZoomSpeed = 10f;
    private const float BaseDefaultFOV = 23.5f;
    private const float AdsFOVDiff = -0.7f;
    private const float MinDefaultFOV = 1f;

    private static GameCamera Camera => GameCamera.Instance;

    private static float DeltaFOV => ZoomSpeed * Time.deltaTime;

    private float _currentDefaultFOV;

    private void Awake()
    {
        Debug.Log("FarSight Loaded!!!");
        _currentDefaultFOV = BaseDefaultFOV;
    }

    private void Update()
    {
        if (!Camera) return;
        // Zoom out
        if (Input.GetKey(KeyCode.PageUp))
        {
            _currentDefaultFOV += DeltaFOV;
        }
        // Zoom in
        if (Input.GetKey(KeyCode.PageDown))
        {
            _currentDefaultFOV -= DeltaFOV;
        }
        // Reset
        if (Input.GetKey(KeyCode.Home))
        {
            _currentDefaultFOV = BaseDefaultFOV;
        }
        // Avoid negative FOV
        _currentDefaultFOV = Mathf.Max(_currentDefaultFOV, MinDefaultFOV);
        // Apply FOV
        Camera.defaultFOV = _currentDefaultFOV;
        Camera.adsFOV = _currentDefaultFOV + AdsFOVDiff;
    }
}