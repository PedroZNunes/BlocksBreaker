using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    private float screenRatio;
    private float currentRatio;

    private float sceneWidth = 11.25f;

    private void Awake()
    {
    }

    private void Start()
    {

#if UNITY_ANDROID && !UNITY_EDITOR

        Resolution res = Screen.currentResolution;

        float unitsPerPixel = sceneWidth / res.width;

        float desiredHalfHeight = 0.5f * unitsPerPixel * res.height;

        cam.orthographicSize = desiredHalfHeight;


        //screenRatio = res.width / res.height;
        //screenRatio = (screenRatio > 1) ? (1 / screenRatio) : screenRatio;

        //sceneWidth = sceneWidth / currentRatio * screenRatio;

        //currentRatio = screenRatio;
        //cam.orthographicSize = sceneWidth;
        
#endif

    }

}
