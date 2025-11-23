using UnityEngine;

namespace FarSight;

public static class FOVManager
{
    public const float BaseDefaultFOV = 23.5f;
    private const float ZoomSpeed = 10f;
    private const float AdsFOVDiff = -0.7f;
    private const float MinDefaultFOV = 1f;

    private static GameCamera Camera => GameCamera.Instance;

    private static float DeltaFOV => ZoomSpeed * Time.deltaTime;

    public static void Update()
    {
        if (!Camera) return;

        var currentFov = Setting.Fov;
        var changed = false;
        
        // Zoom out
        if (Input.GetKey(Setting.ZoomOut))
        {
            currentFov += DeltaFOV;
            changed = true;
        }
        // Zoom in
        if (Input.GetKey(Setting.ZoomIn))
        {
            currentFov -= DeltaFOV;
            changed = true;
        }
        // Reset
        if (Input.GetKey(Setting.ZoomReset))
        {
            currentFov = BaseDefaultFOV;
            changed = true;
        }
        
        if (changed)
        {
            currentFov = Mathf.Max(currentFov, MinDefaultFOV);
            Setting.SetFov(currentFov);
        }

        // Apply FOV
        Camera.defaultFOV = Setting.Fov;
        Camera.adsFOV = Setting.Fov + AdsFOVDiff;
    }
}
