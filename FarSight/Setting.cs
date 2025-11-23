using System;
using UnityEngine;

namespace FarSight;

public static class Setting {
    public static KeyCode ZoomOut { get; set; }
    public static KeyCode ZoomIn { get; set; }
    public static KeyCode ZoomReset { get; set; }
    public static float Fov { get; set; }
    public static event Action<float>? OnFovChange;

    public static void SetZoomOut(KeyCode value) => ZoomOut = value;
    public static void SetZoomIn(KeyCode value) => ZoomIn = value;
    public static void SetZoomReset(KeyCode value) => ZoomReset = value;
    public static void SetFov(float value)
    {
        Fov = value;
        OnFovChange?.Invoke(value);
    }
}
