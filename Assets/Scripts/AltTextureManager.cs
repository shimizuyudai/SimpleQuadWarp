using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AltTextureManager
{
    public Dictionary<string, Texture2D> AltTextureDictionary
    {
        get;
        private set;
    }

    public AltTextureManager()
    {
        AltTextureDictionary = new Dictionary<string, Texture2D>();
    }

    private Texture2D LoadTexture(string path)
    {
        var bytes = File.ReadAllBytes(path);
        var texture = new Texture2D(1, 1);
        texture.LoadImage(bytes);
        texture.Apply();
        AltTextureDictionary.Add(path, texture);
        return texture;
    }

    public Texture2D GetTexture(string path)
    {
        var filePath = path;

        if (!File.Exists(filePath))
        {
            filePath = IOHandler.IntoStreamingAssets(Path.Combine("AltTextures", path));
        }

        if (AltTextureDictionary.ContainsKey(filePath))
        {
            return AltTextureDictionary[filePath];
        }

       
        if (File.Exists(filePath))
        {
            return LoadTexture(filePath);
        }

        return null;
    }
   
}
