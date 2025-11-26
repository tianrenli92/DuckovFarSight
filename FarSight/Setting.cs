using System;
using UnityEngine;

namespace FarSight;

public static class Setting
{
    public const KeyCode DefaultZoomOut = KeyCode.PageUp;
    public const KeyCode DefaultZoomIn = KeyCode.PageDown;
    public const KeyCode DefaultZoomReset = KeyCode.Home;
    public const float DefaultFov = 23.5f;
    public const float DefaultNpcFovMultiplier = 1.5f;

    public static bool ModSettingLoaded { get; set; } = false;
    public static KeyCode ZoomOut { get; set; } = DefaultZoomOut;
    public static KeyCode ZoomIn { get; set; } = DefaultZoomIn;
    public static KeyCode ZoomReset { get; set; } = DefaultZoomReset;
    public static float Fov { get; set; } = DefaultFov;
    public static event Action<float>? OnFovChange;
    public static float NpcFovMultiplier { get; set; } = DefaultNpcFovMultiplier;

    public static void SetZoomOut(KeyCode value) => ZoomOut = value;
    public static void SetZoomIn(KeyCode value) => ZoomIn = value;
    public static void SetZoomReset(KeyCode value) => ZoomReset = value;

    public static void SetFov(float value)
    {
        Fov = value;
        OnFovChange?.Invoke(value);
    }

    public static void SetNpcFovMultiplier(float value) => NpcFovMultiplier = value;
}