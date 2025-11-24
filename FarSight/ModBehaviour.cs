using UnityEngine;

namespace FarSight;

public class ModBehaviour : Duckov.Modding.ModBehaviour
{
    private void Awake()
    {
        Debug.Log("FarSight is loaded");
    }

    protected override void OnAfterSetup()
    {
        Debug.Log("FarSight is set up");
        ModSettingManager.OnAfterSetup(info);
    }

    private void Update()
    {
        FovManager.Update();
    }

    private void OnDisable()
    {
        Debug.Log("FarSight is disabled");
        ModSettingManager.OnDisable();
    }
}