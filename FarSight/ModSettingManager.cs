using System.Collections.Generic;
using Duckov.Modding;
using ModSetting.Api;
using SodaCraft.Localizations;
using UnityEngine;

namespace FarSight;

public static class ModSettingManager
{
    private const string KeyZoomOut = "ZoomOut";
    private const string KeyZoomIn = "ZoomIn";
    private const string KeyZoomReset = "ZoomReset";
    private const string KeyFov = "FOV";

    private static readonly Dictionary<SystemLanguage, Dictionary<string, string>> LanguagePack = new();
    private static ModInfo _modInfo;
    private static SettingsBuilder? _settingsBuilder;

    public static void OnAfterSetup(ModInfo modInfo)
    {
        InitLanguagePack();
        _modInfo = modInfo;
        _settingsBuilder = SettingsBuilder.Create(modInfo);
        InitSetting(_settingsBuilder);
        AddUI(_settingsBuilder, LocalizationManager.CurrentLanguage);
        LocalizationManager.OnSetLanguage += LocalizationManager_OnSetLanguage;
        Setting.OnFovChange += Setting_OnFovChange;
    }

    public static void OnDisable()
    {
        LanguagePack.Clear();
        LocalizationManager.OnSetLanguage -= LocalizationManager_OnSetLanguage;
        Setting.OnFovChange -= Setting_OnFovChange;
    }


    private static void LocalizationManager_OnSetLanguage(SystemLanguage systemLanguage)
    {
        ModSetting.ModBehaviour.RemoveMod(_modInfo);
        AddUI(_settingsBuilder!, systemLanguage);
    }

    private static void Setting_OnFovChange(float fov)
    {
        ModSetting.ModBehaviour.SetValue(_modInfo, KeyFov, fov);
    }

    private static void InitLanguagePack()
    {
        LanguagePack.Add(SystemLanguage.English, new Dictionary<string, string>
        {
            { KeyZoomOut, "Zoom out" },
            { KeyZoomIn, "Zoom in" },
            { KeyZoomReset, "Zoom reset" },
            { KeyFov, "FOV" },
        });
        LanguagePack.Add(SystemLanguage.ChineseSimplified, new Dictionary<string, string>
        {
            { KeyZoomOut, "拉远" },
            { KeyZoomIn, "拉近" },
            { KeyZoomReset, "重置" },
            { KeyFov, "视野" },
        });
        LanguagePack.Add(SystemLanguage.Russian, new Dictionary<string, string>
        {
            { KeyZoomOut, "Отдалить" },
            { KeyZoomIn, "Приблизить" },
            { KeyZoomReset, "Сброс" },
            { KeyFov, "Угол обзора" },
        });
    }

    private static void InitSetting(SettingsBuilder settingsBuilder)
    {
        if (settingsBuilder.HasConfig())
        {
            Setting.ZoomOut = settingsBuilder.GetSavedValue(KeyZoomOut, out KeyCode zoomOut) ? zoomOut : KeyCode.PageUp;
            Setting.ZoomIn = settingsBuilder.GetSavedValue(KeyZoomIn, out KeyCode zoomIn) ? zoomIn : KeyCode.PageDown;
            Setting.ZoomReset = settingsBuilder.GetSavedValue(KeyZoomReset, out KeyCode zoomReset)
                ? zoomReset
                : KeyCode.Home;
            Setting.Fov = settingsBuilder.GetSavedValue(KeyFov, out float fov) ? fov : FOVManager.BaseDefaultFOV;
        }
        else
        {
            // Set default values
            Setting.ZoomIn = KeyCode.PageDown;
            Setting.ZoomOut = KeyCode.PageUp;
            Setting.ZoomReset = KeyCode.Home;
            Setting.Fov = FOVManager.BaseDefaultFOV;
        }
    }

    private static void AddUI(SettingsBuilder settingsBuilder, SystemLanguage language)
    {
        if (!LanguagePack.TryGetValue(language, out var dictionary))
            dictionary = LanguagePack[SystemLanguage.English];

        settingsBuilder
            .AddKeybinding(KeyZoomOut, dictionary[KeyZoomOut], Setting.ZoomOut, KeyCode.PageUp, Setting.SetZoomOut)
            .AddKeybinding(KeyZoomIn, dictionary[KeyZoomIn], Setting.ZoomIn, KeyCode.PageDown, Setting.SetZoomIn)
            .AddKeybinding(KeyZoomReset, dictionary[KeyZoomReset], Setting.ZoomReset, KeyCode.Home,
                Setting.SetZoomReset)
            .AddSlider(KeyFov, dictionary[KeyFov], Setting.Fov, new Vector2(1f, 100f), Setting.SetFov);
    }
}