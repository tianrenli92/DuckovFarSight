using UnityEngine;

namespace FarSight;

public static class DebugUtils
{
        private const float ListNpcCharactersIntervalSeconds = 1f;
        private static float _listNpcCharactersCooldown = ListNpcCharactersIntervalSeconds;
    
        public static void Log(object message)
        {
            Debug.Log($"[FarSight] {message}");
        }

        public static void ListNpcCharactersOnUpdate()
    {
        if (StepScanTimer())
        {
            ListNpcCharacters();
        }
    }

    private static void ListNpcCharacters()
    {
        Log("==== NPC Characters ====");
        foreach (var character in Utils.EnumerateNpcCharacters())
        {
            var preset = character.characterPreset;
            Log(
                $"{character.GetInstanceID()}, {preset.GetInstanceID()}, {preset.DisplayName}, {preset.team}, {preset.health}, {preset.sightDistance}");
        }
        Log("========================");
    }

    private static bool StepScanTimer()
    {
        _listNpcCharactersCooldown -= Time.deltaTime;
        if (_listNpcCharactersCooldown > 0f)
            return false;

        _listNpcCharactersCooldown = ListNpcCharactersIntervalSeconds;
        return true;
    }
}