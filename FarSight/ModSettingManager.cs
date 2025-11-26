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
        AddUI(_settingsBuilder, systemLanguage);
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
        LanguagePack.Add(SystemLanguage.ChineseTraditional, new Dictionary<string, string>
        {
            { KeyZoomOut, "拉遠" },
            { KeyZoomIn, "拉近" },
            { KeyZoomReset, "重置" },
            { KeyFov, "視野" },
            { KeyNpcFovMultiplier, "NPC 視野倍率 (加載新場景後生效)" },
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
        Setting.ModSettingLoaded = true;
        if (!settingsBuilder.HasConfig()) return;
        Setting.ZoomOut = settingsBuilder.GetSavedValue(KeyZoomOut, out KeyCode zoomOut) ? zoomOut : Setting.DefaultZoomOut;
        Setting.ZoomIn = settingsBuilder.GetSavedValue(KeyZoomIn, out KeyCode zoomIn) ? zoomIn : Setting.DefaultZoomIn;
        Setting.ZoomReset = settingsBuilder.GetSavedValue(KeyZoomReset, out KeyCode zoomReset)
            ? zoomReset
            : Setting.DefaultZoomReset;
        Setting.Fov = settingsBuilder.GetSavedValue(KeyFov, out float fov) ? fov : Setting.DefaultFov;
        Setting.NpcFovMultiplier = settingsBuilder.GetSavedValue(KeyNpcFovMultiplier, out float npcFovMultiplier)
            ? npcFovMultiplier
            : Setting.DefaultNpcFovMultiplier;
    }

    private static void AddUI(SettingsBuilder? settingsBuilder, SystemLanguage language)
    {
        if (settingsBuilder is null) return;
        if (!LanguagePack.TryGetValue(language, out var dictionary))
            dictionary = LanguagePack[SystemLanguage.English];

        settingsBuilder
            .AddKeybinding(KeyZoomOut, dictionary[KeyZoomOut], Setting.ZoomOut, Setting.DefaultZoomOut, Setting.SetZoomOut)
            .AddKeybinding(KeyZoomIn, dictionary[KeyZoomIn], Setting.ZoomIn, Setting.DefaultZoomIn, Setting.SetZoomIn)
            .AddKeybinding(KeyZoomReset, dictionary[KeyZoomReset], Setting.ZoomReset, Setting.DefaultZoomReset,
                Setting.SetZoomReset)
            .AddSlider(KeyFov, dictionary[KeyFov], Setting.Fov, new Vector2(1f, 100f), Setting.SetFov)
            .AddSlider(KeyNpcFovMultiplier, dictionary[KeyNpcFovMultiplier], Setting.NpcFovMultiplier,
                new Vector2(1f, 2f), Setting.SetNpcFovMultiplier);
    }
}