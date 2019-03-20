using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpablePlaneControlViewSettingManager : MonoBehaviour
{
    [SerializeField]
    string settingFileName;
    [SerializeField]
    WarpablePlaneControlViewSetting setting;

    private void Awake()
    {
        var info = IOHandler.LoadJson<WarpablePlaneControlViewInfo>(IOHandler.IntoStreamingAssets(settingFileName));
        if (info == null) return;
        setting.ControlPointSize = info.ControlPointSize;
        setting.CornerPointColor = info.CornerPointColor.ToColor();
        setting.CenterPointColor = info.CenterPointColor.ToColor();
    }
}
