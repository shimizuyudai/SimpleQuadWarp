using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerController : MonoBehaviour
{
    [SerializeField]
    VideoPlayer videoPlayer;

    private void OnEnable()
    {
        videoPlayer.enabled = true;
    }

    private void OnDisable()
    {
        videoPlayer.enabled = false;
    }
}
