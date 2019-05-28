using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class AltTextureController : MonoBehaviour
{
    [SerializeField]
    MonoBehaviour textureControlBehaviour;
    [SerializeField]
    Renderer renderer;

    public Texture AltTexture
    {
        get;
        set;
    }

    private void Reset()
    {
        renderer = GetComponent<Renderer>();
    }

    public void OnSelected()
    {
        if (AltTexture == null) return;
        textureControlBehaviour.enabled = false;
        renderer.material.mainTexture = AltTexture;
    }

    public void OnReleased()
    {
        if (AltTexture == null) return;
        textureControlBehaviour.enabled = true;
    }
}
