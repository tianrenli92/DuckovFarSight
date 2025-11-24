using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace FarSight;

public static class Utils
{
    public static IEnumerable<CharacterMainControl> EnumerateNpcCharacters()
    {
        var characterSpawnerRoots = Resources.FindObjectsOfTypeAll<CharacterSpawnerRoot>() ??
                                    Array.Empty<CharacterSpawnerRoot>();
        foreach (var root in characterSpawnerRoots)
        {
            var characters = GetFieldValue<List<CharacterMainControl>>(root, "createdCharacters");
            if (characters is null) continue;

            foreach (var character in characters)
            {
                yield return character;
            }
        }
    }

    public static T? GetFieldValue<T>(object instance, string fieldName) where T : class
    {
        return instance.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance)
            ?.GetValue(instance) as T;
    }
}