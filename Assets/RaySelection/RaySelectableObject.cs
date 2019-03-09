using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RaySelectableObject : MonoBehaviour {
    [SerializeField]
    protected int id;
    public int Id
    {
        get {
            return id;
        }
    }

    public event Action<RaySelectableObject> SelectedEvent;
    public event Action<RaySelectableObject> ReleasedEvent;


    public void OnSelected()
    {
        if (SelectedEvent != null) SelectedEvent(this);
    }

    public void OnReleased()
    {
        if (ReleasedEvent != null) ReleasedEvent(this);
    }
}
