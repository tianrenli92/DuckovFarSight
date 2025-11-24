using ItemStatsSystem.Stats;
using UnityEngine;

namespace FarSight;

public static class FovManager
{
    public const float BaseDefaultFov = 23.5f;
    private const float ZoomSpeed = 10f;
    private const float AdsFovDiff = -0.7f;
    private const float MinDefaultFov = 1f;

    private static readonly int RecoilControlHash = nameof(CharacterMainControl.RecoilControl).GetHashCode();

    private static GameCamera Camera => GameCamera.Instance;

    private static float DeltaFov => ZoomSpeed * Time.deltaTime;

    private static Modifier? _fovRecoilModifier;

    public static void OnAfterSetup()
    {
        _fovRecoilModifier = new Modifier(ModifierType.PercentageMultiply, 1, null);
        SceneLoader.onAfterSceneInitialize += ApplyFovRecoilModifier;
    }

    public static void OnDisable()
    {
        SceneLoader.onAfterSceneInitialize -= ApplyFovRecoilModifier;
    }

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
        // Adjust recoil based on FOV
        _fovRecoilModifier?.Value = Setting.Fov / BaseDefaultFov - 1f;
    }

    private static void ApplyFovRecoilModifier(SceneLoadingContext sceneLoadingContext)
    {
        // Adjust recoil
        var recoil = CharacterMainControl.Main?.CharacterItem?.GetStat(RecoilControlHash);
        if (recoil == null)
        {
            DebugUtils.Log("Main character recoil not found. Cannot apply FOV recoil modifier.");
            return;
        }

        var oldRecoilValue = recoil.Value;
        recoil.AddModifier(_fovRecoilModifier);
        DebugUtils.Log($"Recoil changed from {oldRecoilValue} to {recoil.Value} after applying FOV recoil modifier");
    }
}