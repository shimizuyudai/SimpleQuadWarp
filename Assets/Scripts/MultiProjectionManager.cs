using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using TypeUtils;
using Klak.Spout;
using UnityEngine.Video;

public class MultiProjectionManager : MonoBehaviour
{
    [SerializeField]
    GameObject spoutReceiverPrefab, imagePrefab, videoPrefab;
    [SerializeField]
    KeyCode saveKey;
    [SerializeField]
    WarpablePlaneController warpablePlaneController;

    List<WarpablePlane> planes;
    [SerializeField]
    IntVec2 segment;
    [SerializeField]
    Vector2 screenSize;

    [SerializeField]
    string textureSettingsFileName, projectionSettingsFileName;

    public enum TextureType
    {
        Spout,
        Image,
        Video
    }

    ProjectionTextureSettings LoadProjectionTextureSettings()
    {
        var settings = IOHandler.LoadJson<ProjectionTextureSettings>(IOHandler.IntoStreamingAssets(textureSettingsFileName));
        return settings ?? new ProjectionTextureSettings();
    }

    private List<MultiProjectionSettings> LoadProjectionSettings()
    {
        var settings = IOHandler.LoadJson<List<MultiProjectionSettings>>(IOHandler.IntoStreamingAssets(projectionSettingsFileName));
        return settings ?? new List<MultiProjectionSettings>();
    }

    byte[] GetImage(string path)
    {
        if (File.Exists(path))
        {
            return File.ReadAllBytes(path);
        }
        var p = IOHandler.IntoStreamingAssets(Path.Combine("Images", path));
        if (File.Exists(p))
        {
            return File.ReadAllBytes(p);
        }
        return null;
    }

    string GetVideoPath(string path)
    {
        if (File.Exists(path))
        {
            return path;
        }
        var p = IOHandler.IntoStreamingAssets(Path.Combine("Videos", path));
        if (File.Exists(p))
        {
            return p;
        }

        return path;
    }

    void Init()
    {

        var textureSettings = LoadProjectionTextureSettings();
        var multiProjectionSettings = LoadProjectionSettings();
        if (textureSettings.ProjectionTextureInfoList != null)
        {
            var size = Vector2.one * 3f;
            var position = Vector3.zero;

            planes = new List<WarpablePlane>();
            foreach (var textureInfo in textureSettings.ProjectionTextureInfoList)
            {
                GameObject go = null;
                var textureType = TextureType.Spout;
                var result = EUtils.StringToEnumElement(typeof(TextureType), textureInfo.TextureType, ref textureType);
                if (!result) continue;
                switch (textureType)
                {
                    case TextureType.Spout:
                        go = GameObject.Instantiate(spoutReceiverPrefab) as GameObject;
                        var spoutReceiver = go.GetComponent<SpoutReceiver>();
                        spoutReceiver.sourceName = textureInfo.Name;
                        break;

                    case TextureType.Image:
                        go = GameObject.Instantiate(imagePrefab) as GameObject;
                        var bytes = GetImage(textureInfo.Name);
                        if (bytes != null)
                        {
                            var renderer = go.GetComponent<Renderer>();
                            var texture = new Texture2D(1, 1);
                            texture.LoadImage(bytes);
                            texture.Apply();
                            renderer.material.mainTexture = texture;
                        }

                        break;

                    case TextureType.Video:
                        go = GameObject.Instantiate(videoPrefab) as GameObject;
                        var videoPlayer = go.GetComponent<VideoPlayer>();
                        var path = GetVideoPath(textureInfo.Name);
                        videoPlayer.url = path;

                        break;

                }

                var projectionPlane = go.GetComponent<ProjectionPlane>();
                projectionPlane.projectionTextureInfo = textureInfo;
                go.transform.position = Vector3.zero;
                var selectable = go.GetComponent<RaySelectableObject>();
                selectable.SelectedEvent += Selectable_SelectedEvent;
                selectable.ReleasedEvent += Selectable_ReleasedEvent;
                var plane = go.GetComponent<WarpablePlane>();
                plane.Init(position, size, segment.x, segment.y);

                var projectionSetting = multiProjectionSettings.FirstOrDefault(e => e.Id == textureInfo.Id);
                if (projectionSetting != null)
                {
                    plane.Restore(projectionSetting.CornerPointInfomations);
                }
                planes.Add(plane);
            }
        }
        warpablePlaneController.WarpablePlanes = planes;
    }

    private void Start()
    {
        Init();
    }

    private void Selectable_ReleasedEvent(RaySelectableObject obj)
    {
        var pos = obj.gameObject.transform.localPosition;
        pos.z = 0f;
        obj.gameObject.transform.localPosition = pos;
    }

    private void Selectable_SelectedEvent(RaySelectableObject obj)
    {
        var pos = obj.gameObject.transform.localPosition;
        pos.z = -1;
        obj.gameObject.transform.localPosition = pos;
    }

    private void Update()
    {
        if (Input.GetKeyDown(saveKey))
        {
            Save();
        }
    }



    private void Save()
    {
        var settingsList = new List<MultiProjectionSettings>();
        foreach (var plane in planes)
        {
            var projectionPlane = plane.gameObject.GetComponent<ProjectionPlane>();
            var settings = new MultiProjectionSettings
            {
                Id = projectionPlane.projectionTextureInfo.Id,
                Name = projectionPlane.projectionTextureInfo.Name,
                CornerPointInfomations = plane.CornerPointInfomations
            };
            settingsList.Add(settings);
        }
        IOHandler.SaveJson(IOHandler.IntoStreamingAssets(projectionSettingsFileName), settingsList);
    }
}

public class ProjectionTextureSettings
{
    public List<ProjectionTextureInfo> ProjectionTextureInfoList;
}

public class MultiProjectionSettings
{
    public string Id;
    public string Name;
    public WarpablePlane.CornerPointInfomation[] CornerPointInfomations;
}

public class ProjectionTextureInfo
{
    public string Id;
    public string TextureType;
    public string Name;
    public string AltTextureName;
}
