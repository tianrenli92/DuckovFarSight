using System.Collections.Generic;
using UnityEngine;

namespace FarSight;

public static class NpcFovManager
{
    private static readonly Dictionary<int, float> DefaultCharacterPresetSightDistances = [];
    private static readonly Dictionary<int, float> DefaultAiControllerSightDistances = [];

    public static void OnEnable()
    {
        SceneLoader.onFinishedLoadingScene += AdjustNpcFov;
    }

    public static void OnDisable()
    {
        SceneLoader.onFinishedLoadingScene -= AdjustNpcFov;
    }

    private static void AdjustNpcFov(SceneLoadingContext context)
    {
        var characterPresets = Resources.FindObjectsOfTypeAll<CharacterRandomPreset>() ?? [];
        foreach (var preset in characterPresets)
        {
            var presetId = preset.GetInstanceID();
            if (!DefaultCharacterPresetSightDistances.TryGetValue(presetId, out var defaultSightDistance))
            {
                defaultSightDistance = preset.sightDistance;
                DefaultCharacterPresetSightDistances[presetId] = defaultSightDistance;
            }

            var adjustedSightDistance = defaultSightDistance * Setting.NpcFovMultiplier;
            if (Mathf.Approximately(adjustedSightDistance, preset.sightDistance)) continue;
            DebugUtils.Log(
                $"{presetId}, {preset.DisplayName}, {preset.team}, {preset.health}, {defaultSightDistance}");
            DebugUtils.Log($"Preset sight distance adjusted from {preset.sightDistance} to {adjustedSightDistance}");
            preset.sightDistance = adjustedSightDistance;

            var aiController = Utils.GetFieldValue<AICharacterController>(preset, "aiController");
            if (aiController is null) continue;

            var aiControllerId = aiController.GetInstanceID();
            if (!DefaultAiControllerSightDistances.TryGetValue(aiControllerId, out var defaultAiSightDistance))
            {
                defaultAiSightDistance = aiController.sightDistance;
                DefaultAiControllerSightDistances[aiControllerId] = defaultAiSightDistance;
            }

            var adjustedAiSightDistance = defaultAiSightDistance * Setting.NpcFovMultiplier;
            if (Mathf.Approximately(adjustedAiSightDistance, aiController.sightDistance)) continue;
            DebugUtils.Log($"{aiControllerId}, {defaultAiSightDistance}");
            DebugUtils.Log(
                $"AI sight distance adjusted from {aiController.sightDistance} to {adjustedAiSightDistance}");
            aiController.sightDistance = adjustedAiSightDistance;
        }
    }
}