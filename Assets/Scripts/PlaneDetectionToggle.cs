using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARPlaneManager))]
public class PlaneDetectionToggle : MonoBehaviour
{
    private ARPlaneManager _planeManager;

    private void Awake()
    {
        _planeManager = GetComponent<ARPlaneManager>();
    }


    public void TogglePlaneDetection()
    {
        _planeManager.enabled = !_planeManager.enabled;

        SetAllPlanesActive(_planeManager.enabled);
    }

    private void SetAllPlanesActive(bool value)
    {
        foreach (var plane in _planeManager.trackables)
        {
            plane.gameObject.SetActive(value);
        }
    }
}