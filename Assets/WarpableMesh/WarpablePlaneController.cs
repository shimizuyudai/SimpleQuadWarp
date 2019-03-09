using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WarpablePlaneController : MonoBehaviour {
    [SerializeField]
    RaySelector selector;

    public List<WarpablePlane> WarpablePlanes;
    Vector3 preMousePosition;
    [SerializeField]
    KeyCode resetKey;
    // Use this for initialization
    void Start()
    {
        preMousePosition = selector.Camera.ScreenToWorldPoint(Input.mousePosition);
    }

    // Update is called once per frame
    void Update () {
        var isControl = false;
        var worldMousePos = selector.Camera.ScreenToWorldPoint(Input.mousePosition);
        //worldMousePos.z = 0f;

        var selectedPlane = WarpablePlanes.FirstOrDefault(e => e.IsEnable);

        if (Input.GetMouseButtonDown(0))
        {
            if (selectedPlane != null)
            {
                isControl = selectedPlane.OnPointerDown(worldMousePos);
                selectedPlane.IsWarp = isControl;
            }

            if (!isControl)
            {
                selector.OnClicked();
            }
        }



        if (Input.GetMouseButtonUp(0))
        {
            foreach (var plane in WarpablePlanes)
            {
                plane.IsWarp = false;
            }
        }

        if (selectedPlane != null)
        {
            if (selectedPlane.IsWarp)
            {
                selectedPlane.Warp(worldMousePos - preMousePosition);
            }
        }


        if(Input.GetKeyDown(resetKey))
        {
            foreach (var warpPlane in WarpablePlanes)
            {
                if(warpPlane.IsEnable)
                warpPlane.Clear();
            }
        }


        preMousePosition = worldMousePos;
    }
}
