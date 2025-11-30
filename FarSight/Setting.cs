using System;
using UnityEngine;

namespace FarSight;

public static class Setting
{
    public const KeyCode DefaultZoomOut = KeyCode.PageUp;
    public const KeyCode DefaultZoomIn = KeyCode.PageDown;
    public const KeyCode DefaultZoomReset = KeyCode.Home;
    public const KeyCode DefaultQuickZoomOut = KeyCode.End;
    public const KeyCode DefaultApplyFavoriteFov = KeyCode.Insert;
    public const KeyCode DefaultApplySecondFavoriteFov = KeyCode.Delete;
    public const float DefaultFov = 23.5f;
    public const float DefaultFavoriteFov = DefaultFov * 2;
    public const float DefaultSecondFavoriteFov = DefaultFov * 3;
    public const float DefaultNpcFovMultiplier = 1.5f;

    public static bool ModSettingLoaded { get; set; }
    public static KeyCode ZoomOut { get; set; } = DefaultZoomOut;
    public static KeyCode ZoomIn { get; set; } = DefaultZoomIn;
    public static KeyCode ZoomReset { get; set; } = DefaultZoomReset;
    public static KeyCode QuickZoomOut { get; set; } = DefaultQuickZoomOut;
    public static KeyCode ApplyFavoriteFov { get; set; } = DefaultApplyFavoriteFov;
    public static KeyCode ApplySecondFavoriteFov { get; set; } = DefaultApplySecondFavoriteFov;
    public static float Fov { get; set; } = DefaultFov;
    public static float FavoriteFov { get; set; } = DefaultFavoriteFov;
    public static float SecondFavoriteFov { get; set; } = DefaultSecondFavoriteFov;
    public static event Action<float>? OnFovChange;
    public static float NpcFovMultiplier { get; set; } = DefaultNpcFovMultiplier;

    public static void SetZoomOut(KeyCode value) => ZoomOut = value;
    public static void SetZoomIn(KeyCode value) => ZoomIn = value;
    public static void SetZoomReset(KeyCode value) => ZoomReset = value;
    public static void SetQuickZoomOut(KeyCode value) => QuickZoomOut = value;
    public static void SetApplyFavoriteFov(KeyCode value) => ApplyFavoriteFov = value;
    public static void SetApplySecondFavoriteFov(KeyCode value) => ApplySecondFavoriteFov = value;

    public static void SetFov(float value)
    {
        Fov = value;
        OnFovChange?.Invoke(value);
    }

    public static void SetFavoriteFov(float value) => FavoriteFov = value;
    public static void SetSecondFavoriteFov(float value) => SecondFavoriteFov = value;

    public static void SetNpcFovMultiplier(float value) => NpcFovMultiplier = value;
}