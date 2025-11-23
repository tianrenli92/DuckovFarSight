using UnityEngine;

namespace FarSight;

public class ModBehaviour : Duckov.Modding.ModBehaviour
{
    private FOVManager? _fovManager;

    private void Awake()
    {
        Debug.Log("FarSight Loaded!!!");
        _fovManager = new FOVManager();
    }

    protected override void OnAfterSetup()
    {
        ModSettingManager.OnAfterSetup(info);
    }

    private void Update()
    {
        _fovManager!.Update();
    }

    private void OnDisable()
    {
        ModSettingManager.OnDisable();
    }
}