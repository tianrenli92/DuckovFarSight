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
    private const string KeyQuickZoomOut = "QuickZoomOut";
    private const string KeyFov = "Fov";
    private const string KeyApplyFavoriteFov = "ApplyFavoriteFov";
    private const string KeyFavoriteFov = "FavoriteFov";
    private const string KeyApplySecondFavoriteFov = "ApplySecondFavoriteFov";
    private const string KeySecondFavoriteFov = "SecondFavoriteFov";
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
            { KeyQuickZoomOut, "Quick zoom out" },
            { KeyFov, "FOV" },
            { KeyApplyFavoriteFov, "Apply favorite FOV" },
            { KeyFavoriteFov, "Favorite FOV" },
            { KeyApplySecondFavoriteFov, "Apply second favorite FOV" },
            { KeySecondFavoriteFov, "Second favorite FOV" },
            { KeyNpcFovMultiplier, "NPC FOV Multiplier (Takes effect after loading a new scenario)" },
        });
        LanguagePack.Add(SystemLanguage.ChineseSimplified, new Dictionary<string, string>
        {
            { KeyZoomOut, "拉远" },
            { KeyZoomIn, "拉近" },
            { KeyZoomReset, "重置" },
            { KeyQuickZoomOut, "快速拉远" },
            { KeyFov, "视野" },
            { KeyApplyFavoriteFov, "应用常用视野 1" },
            { KeyFavoriteFov, "常用视野 1" },
            { KeyApplySecondFavoriteFov, "应用常用视野 2" },
            { KeySecondFavoriteFov, "常用视野 2" },
            { KeyNpcFovMultiplier, "NPC 视野倍率 (加载新场景后生效)" },
        });
        LanguagePack.Add(SystemLanguage.ChineseTraditional, new Dictionary<string, string>
        {
            { KeyZoomOut, "拉遠" },
            { KeyZoomIn, "拉近" },
            { KeyZoomReset, "重置" },
            { KeyQuickZoomOut, "快速拉遠" },
            { KeyFov, "視野" },
            { KeyApplyFavoriteFov, "應用常用視野 2" },
            { KeyFavoriteFov, "常用視野 2" },
            { KeyApplySecondFavoriteFov, "應用常用視野 2" },
            { KeySecondFavoriteFov, "常用視野 2" },
            { KeyNpcFovMultiplier, "NPC 視野倍率 (加載新場景後生效)" },
        });
        LanguagePack.Add(SystemLanguage.Russian, new Dictionary<string, string>
        {
            { KeyZoomOut, "Отдалить" },
            { KeyZoomIn, "Приблизить" },
            { KeyZoomReset, "Сброс" },
            { KeyQuickZoomOut, "Быстрое отдаление" },
            { KeyFov, "Угол обзора" },
            { KeyApplyFavoriteFov, "Применить любимый угол обзора" },
            { KeyFavoriteFov, "Любимый угол обзора" },
            { KeyApplySecondFavoriteFov, "Применить второй любимый угол обзора" },
            { KeySecondFavoriteFov, "Второй любимый угол обзора" },
            { KeyNpcFovMultiplier, "Множитель угла обзора NPC (Применяется после загрузки нового сценария)" },
        });
    }

    private static void InitSetting(SettingsBuilder settingsBuilder)
    {
        Setting.ModSettingLoaded = true;
        if (!settingsBuilder.HasConfig()) return;
        Setting.ZoomOut = settingsBuilder.GetSavedValue(KeyZoomOut, out KeyCode zoomOut)
            ? zoomOut
            : Setting.DefaultZoomOut;
        Setting.ZoomIn = settingsBuilder.GetSavedValue(KeyZoomIn, out KeyCode zoomIn) ? zoomIn : Setting.DefaultZoomIn;
        Setting.ZoomReset = settingsBuilder.GetSavedValue(KeyZoomReset, out KeyCode zoomReset)
            ? zoomReset
            : Setting.DefaultZoomReset;
        Setting.QuickZoomOut = settingsBuilder.GetSavedValue(KeyQuickZoomOut, out KeyCode quickZoomOut)
            ? quickZoomOut
            : Setting.DefaultQuickZoomOut;
        Setting.Fov = settingsBuilder.GetSavedValue(KeyFov, out float fov) ? fov : Setting.DefaultFov;
        Setting.ApplyFavoriteFov = settingsBuilder.GetSavedValue(KeyApplyFavoriteFov, out KeyCode applyFavoriteFov)
            ? applyFavoriteFov
            : Setting.DefaultApplyFavoriteFov;
        Setting.FavoriteFov = settingsBuilder.GetSavedValue(KeyFavoriteFov, out float favoriteFov)
            ? favoriteFov
            : Setting.DefaultFavoriteFov;
        Setting.ApplySecondFavoriteFov =
            settingsBuilder.GetSavedValue(KeyApplySecondFavoriteFov, out KeyCode applySecondFavoriteFov)
                ? applySecondFavoriteFov
                : Setting.DefaultApplySecondFavoriteFov;
        Setting.SecondFavoriteFov = settingsBuilder.GetSavedValue(KeySecondFavoriteFov, out float secondFavoriteFov)
            ? secondFavoriteFov
            : Setting.DefaultSecondFavoriteFov;
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
            .AddKeybinding(KeyZoomOut, dictionary[KeyZoomOut], Setting.ZoomOut, Setting.DefaultZoomOut,
                Setting.SetZoomOut)
            .AddKeybinding(KeyZoomIn, dictionary[KeyZoomIn], Setting.ZoomIn, Setting.DefaultZoomIn,
                Setting.SetZoomIn)
            .AddKeybinding(KeyZoomReset, dictionary[KeyZoomReset], Setting.ZoomReset, Setting.DefaultZoomReset,
                Setting.SetZoomReset)
            .AddKeybinding(KeyQuickZoomOut, dictionary[KeyQuickZoomOut], Setting.QuickZoomOut,
                Setting.DefaultQuickZoomOut,
                Setting.SetQuickZoomOut)
            .AddSlider(KeyFov, dictionary[KeyFov], Setting.Fov, new Vector2(1f, 100f), Setting.SetFov)
            .AddSlider(KeyFavoriteFov, dictionary[KeyFavoriteFov], Setting.FavoriteFov, new Vector2(1f, 100f),
                Setting.SetFavoriteFov)
            .AddKeybinding(KeyApplyFavoriteFov, dictionary[KeyApplyFavoriteFov], Setting.ApplyFavoriteFov,
                Setting.DefaultApplyFavoriteFov, Setting.SetApplyFavoriteFov)
            .AddSlider(KeySecondFavoriteFov, dictionary[KeySecondFavoriteFov], Setting.SecondFavoriteFov,
                new Vector2(1f, 100f), Setting.SetSecondFavoriteFov)
            .AddKeybinding(KeyApplySecondFavoriteFov, dictionary[KeyApplySecondFavoriteFov],
                Setting.ApplySecondFavoriteFov, Setting.DefaultApplySecondFavoriteFov,
                Setting.SetApplySecondFavoriteFov)
            .AddSlider(KeyNpcFovMultiplier, dictionary[KeyNpcFovMultiplier], Setting.NpcFovMultiplier,
                new Vector2(1f, 2f), Setting.SetNpcFovMultiplier);
    }
}