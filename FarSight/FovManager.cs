using UnityEngine;

namespace FarSight;

public static class FovManager
{
    public const float BaseDefaultFov = 23.5f;
    private const float ZoomSpeed = 10f;
    private const float AdsFovDiff = -0.7f;
    private const float MinDefaultFov = 1f;

    private static GameCamera Camera => GameCamera.Instance;

    private static float DeltaFov => ZoomSpeed * Time.deltaTime;

    public static void Update()
    {
        if (!Camera) return;

        var currentFov = Setting.Fov;
        var changed = false;
        
        // Zoom out
        if (Input.GetKey(Setting.ZoomOut))
        {
            currentFov += DeltaFov;
            changed = true;
        }
        // Zoom in
        if (Input.GetKey(Setting.ZoomIn))
        {
            currentFov -= DeltaFov;
            changed = true;
        }
        // Reset
        if (Input.GetKey(Setting.ZoomReset))
        {
            currentFov = BaseDefaultFov;
            changed = true;
        }
        
        if (changed)
        {
            currentFov = Mathf.Max(currentFov, MinDefaultFov);
            Setting.SetFov(currentFov);
        }

        // Apply FOV
        Camera.defaultFOV = Setting.Fov;
        Camera.adsFOV = Setting.Fov + AdsFovDiff;
    }
}
