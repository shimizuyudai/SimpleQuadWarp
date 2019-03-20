using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create WarpablePlaneControlViewSetting", fileName = "WarpablePlaneControlViewSetting.asset")]
public class WarpablePlaneControlViewSetting : ScriptableObject
{
    [SerializeField]
    Vector3 adjustPosition;
    public Vector3 AdjustPosition
    {
        get
        {
            return adjustPosition;
        }
    }

    [SerializeField]
    public Color CornerPointColor, CenterPointColor;

    [SerializeField]
    public float ControlPointSize;
}
