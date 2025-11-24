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
    private const string KeyFov = "Fov";
    private const string KeyNpcFovMultiplier = "NpcFovMultiplier";

    private const KeyCode DefaultZoomOut = KeyCode.PageUp;
    private const KeyCode DefaultZoomIn = KeyCode.PageDown;
    private const KeyCode DefaultZoomReset = KeyCode.Home;
    private const float DefaultNpcFovMultiplier = 1.5f;

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
            { KeyNpcFovMultiplier, "NPC FOV Multiplier (Takes effect after loading a new scenario)" },
        });
        LanguagePack.Add(SystemLanguage.ChineseSimplified, new Dictionary<string, string>
        {
            { KeyZoomOut, "拉远" },
            { KeyZoomIn, "拉近" },
            { KeyZoomReset, "重置" },
            { KeyFov, "视野" },
            { KeyNpcFovMultiplier, "NPC 视野倍率 (加载新场景后生效)" },
        });
        LanguagePack.Add(SystemLanguage.Russian, new Dictionary<string, string>
        {
            { KeyZoomOut, "Отдалить" },
            { KeyZoomIn, "Приблизить" },
            { KeyZoomReset, "Сброс" },
            { KeyFov, "Угол обзора" },
            { KeyNpcFovMultiplier, "Множитель угла обзора NPC (Применяется после загрузки нового сценария)" },
        });
    }

    private static void InitSetting(SettingsBuilder settingsBuilder)
    {
        if (settingsBuilder.HasConfig())
        {
            Setting.ZoomOut = settingsBuilder.GetSavedValue(KeyZoomOut, out KeyCode zoomOut) ? zoomOut : DefaultZoomOut;
            Setting.ZoomIn = settingsBuilder.GetSavedValue(KeyZoomIn, out KeyCode zoomIn) ? zoomIn : DefaultZoomIn;
            Setting.ZoomReset = settingsBuilder.GetSavedValue(KeyZoomReset, out KeyCode zoomReset)
                ? zoomReset
                : DefaultZoomReset;
            Setting.Fov = settingsBuilder.GetSavedValue(KeyFov, out float fov) ? fov : FovManager.BaseDefaultFov;
            Setting.NpcFovMultiplier = settingsBuilder.GetSavedValue(KeyNpcFovMultiplier, out float npcFovMultiplier)
                ? npcFovMultiplier
                : DefaultNpcFovMultiplier;
        }
        else
        {
            // Set default values
            Setting.ZoomIn = DefaultZoomIn;
            Setting.ZoomOut = DefaultZoomOut;
            Setting.ZoomReset = DefaultZoomReset;
            Setting.Fov = FovManager.BaseDefaultFov;
            Setting.NpcFovMultiplier = DefaultNpcFovMultiplier;
        }
    }

    private static void AddUI(SettingsBuilder settingsBuilder, SystemLanguage language)
    {
        if (!LanguagePack.TryGetValue(language, out var dictionary))
            dictionary = LanguagePack[SystemLanguage.English];

        settingsBuilder
            .AddKeybinding(KeyZoomOut, dictionary[KeyZoomOut], Setting.ZoomOut, DefaultZoomOut, Setting.SetZoomOut)
            .AddKeybinding(KeyZoomIn, dictionary[KeyZoomIn], Setting.ZoomIn, DefaultZoomIn, Setting.SetZoomIn)
            .AddKeybinding(KeyZoomReset, dictionary[KeyZoomReset], Setting.ZoomReset, DefaultZoomReset,
                Setting.SetZoomReset)
            .AddSlider(KeyFov, dictionary[KeyFov], Setting.Fov, new Vector2(1f, 100f), Setting.SetFov)
            .AddSlider(KeyNpcFovMultiplier, dictionary[KeyNpcFovMultiplier], Setting.NpcFovMultiplier,
                new Vector2(1f, 2f), Setting.SetNpcFovMultiplier);
    }
}