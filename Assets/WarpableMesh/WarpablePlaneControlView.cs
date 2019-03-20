using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class WarpablePlaneControlView : MonoBehaviour
{
    [SerializeField]
    WarpablePlane warpPlane;
    [SerializeField]
    ParticleSystem ps;
    [SerializeField]
    WarpablePlaneControlViewSetting warpablePlaneControlViewSetting;

    Texture preTexture;

    private void Awake()
    {
        warpPlane.TouchDistanceThreshold = warpablePlaneControlViewSetting.ControlPointSize;
        warpPlane.OnChangeMode += WarpPlane_OnChangeMode;
        ps.Stop();
    }

    private void WarpPlane_OnChangeMode(bool isEnable)
    {
        if (isEnable) return;
        ps.Clear();
    }

    private void LateUpdate()
    {

        if (warpPlane.IsEnable)
        {
            var particles = new List<ParticleSystem.Particle>();
            foreach (var controlPoint in warpPlane.ControlPoints)
            {
                var p = new ParticleSystem.Particle();
                p.startSize = warpPlane.TouchDistanceThreshold * 2f;
                p.startColor = controlPoint.number == warpPlane.CornerPoints.Length ? warpablePlaneControlViewSetting.CenterPointColor : warpablePlaneControlViewSetting.CornerPointColor;
                p.position = controlPoint.position;
                p.position += warpablePlaneControlViewSetting.AdjustPosition;
                particles.Add(p);
            }
            ps.SetParticles(particles.ToArray(), particles.Count);
            //print(particles.Count);
        }
    }
}