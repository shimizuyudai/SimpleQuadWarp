using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaySelector : MonoBehaviour {
    [SerializeField]
    Camera camera;
    public Camera Camera
    {
        get {
            return camera;
        }
    }
    [SerializeField]
    LayerMask layerMask;

    RaySelectableObject preSelectedObject;

    

    public void OnClicked()
    {
        var ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            var component = hit.transform.gameObject.GetComponent<RaySelectableObject>();
            if (preSelectedObject != null)
            {
                if (component != preSelectedObject)
                {
                    preSelectedObject.OnReleased();
                }
            }
            component.OnSelected();
            preSelectedObject = component;
        }
        else
        {
            if (preSelectedObject != null)
            {
                preSelectedObject.OnReleased();
                preSelectedObject = null;
            }
        }
    }

    public void Release()
    {
        if (preSelectedObject != null)
        {
            preSelectedObject.OnReleased();
            preSelectedObject = null;
        }
    }
}
