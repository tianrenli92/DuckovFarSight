using UnityEngine;

namespace FarSight;

public class ModBehaviour : Duckov.Modding.ModBehaviour
{
    private void Awake()
    {
        DebugUtils.Log("FarSight is loaded");
    }

    protected override void OnAfterSetup()
    {
        DebugUtils.Log("FarSight is set up");
        ModSettingManager.OnAfterSetup(info);
        NpcFovManager.OnAfterSetup();
    }

    private void Update()
    {
        FovManager.Update();
    }

    private void OnDisable()
    {
        DebugUtils.Log("FarSight is disabled");
        ModSettingManager.OnDisable();
        NpcFovManager.OnDisable();
    }
}