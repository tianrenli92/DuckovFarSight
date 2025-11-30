using ItemStatsSystem.Stats;
using SodaCraft.Localizations;
using UnityEngine;

namespace FarSight;

public static class FovManager
{
    private const float ZoomSpeed = 10f;
    private const float AdsFovDiff = -0.7f;
    private const float MinDefaultFov = 1f;

    private static readonly int RecoilControlHash = nameof(CharacterMainControl.RecoilControl).GetHashCode();

    private static GameCamera Camera => GameCamera.Instance;

    private static float DeltaFov => ZoomSpeed * Time.deltaTime;

    private static Modifier? _fovRecoilModifier;

    public static void OnEnable()
    {
        _fovRecoilModifier = new Modifier(ModifierType.PercentageMultiply, 1, null);
        SceneLoader.onAfterSceneInitialize += ApplyFovRecoilModifier;
        SceneLoader.onAfterSceneInitialize += ShowWarningIfModSettingIsNotLoaded;
    }

    public static void OnDisable()
    {
        SceneLoader.onAfterSceneInitialize -= ApplyFovRecoilModifier;
        SceneLoader.onAfterSceneInitialize -= ShowWarningIfModSettingIsNotLoaded;
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
            currentFov = Setting.DefaultFov;
            changed = true;
        }

        // Quick Zoom Out
        if (Input.GetKeyDown(Setting.QuickZoomOut))
        {
            currentFov += Setting.DefaultFov * 0.5f;
            changed = true;
        }

        // Favorite FOV
        if (Input.GetKeyDown(Setting.ApplyFavoriteFov))
        {
            currentFov = Setting.FavoriteFov;
            changed = true;
        }

        // Second Favorite FOV
        if (Input.GetKeyDown(Setting.ApplySecondFavoriteFov))
        {
            currentFov = Setting.SecondFavoriteFov;
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
        _fovRecoilModifier!.Value = Setting.Fov / Setting.DefaultFov - 1f;
    }

    private static void ApplyFovRecoilModifier(SceneLoadingContext sceneLoadingContext)
    {
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

    private static void ShowWarningIfModSettingIsNotLoaded(SceneLoadingContext sceneLoadingContext)
    {
        if (Setting.ModSettingLoaded) return;
        DebugUtils.Log("ModSetting is not loaded");
        CharacterMainControl.Main?.PopText(GetModSettingIsNotLoadedWarningText());
    }

    private static string GetModSettingIsNotLoadedWarningText()
    {
        switch (LocalizationManager.CurrentLanguage)
        {
            case SystemLanguage.ChineseSimplified:
                return "更远视距模组无法正常加载，请安装并加载ModSetting再启用更远视距！";
            case SystemLanguage.ChineseTraditional:
                return "更远视距模組無法正常加載，請安裝並加載ModSetting再啟用更遠視距！";
            case SystemLanguage.Russian:
                return
                    "Мод Увеличенная дальность обзора не может быть загружен корректно. Пожалуйста, установите и загрузите ModSetting перед включением Увеличенная дальность обзора!";
            case SystemLanguage.English:
            default:
                return
                    "Far Sight mod cannot be loaded correctly. Please install and load ModSetting before enabling Far Sight!";
        }
    }
}