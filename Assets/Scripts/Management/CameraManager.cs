using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    private float sceneWidth = 11.2f;

    private void Update()
    {

 #if UNITY_ANDROID && !UNITY_EDITOR 

        float unitsPerPixel = sceneWidth / Screen.width;

        float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;

        this.cam.orthographicSize = desiredHalfHeight;

 #endif

    }

}
