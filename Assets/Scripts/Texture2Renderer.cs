using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Texture2Renderer : MonoBehaviour
{
    [SerializeField]
    Renderer renderer;
    public Texture Texture
    {
        get;
        set;
    }

    private void Reset()
    {
        renderer = GetComponent<Renderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        renderer.material.mainTexture = Texture;
    }

    // Update is called once per frame
    void Update()
    {
        renderer.material.mainTexture = Texture;
    }
}
