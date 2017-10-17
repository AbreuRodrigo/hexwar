using System.Collections;
using UnityEngine;

public class PinchZoom : MonoBehaviour
{
    public Camera mainCamera;
    public float perspectiveZoomSpeed = 0.01f;
    public float orthoZoomSpeed = 0.01f;
    public float minZoon = 5;
    public float maxZoom = 20;
    private float zoomSpeedMouse = -25;
    private bool initializing = true;

    void Start()
    {
        mainCamera.orthographicSize = maxZoom;

        iTween.ValueTo(gameObject, iTween.Hash(
            "from", mainCamera.orthographicSize,
            "to", minZoon,
            "time", 1f,
            "delay", 1,
            "easetype", iTween.EaseType.easeOutCirc,
            "onupdatetarget", gameObject, 
            "onupdate", "OnFirstZoom"
        ));
    }

    void Update()
    {
        if (!initializing)
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
            {
                ZoomViaTouch();
            }
            else
            {
                ZoomViaMouse();
            }
        }
    }

    private void OnFirstZoom(float value)
    {
        mainCamera.orthographicSize = value;

        if(mainCamera.orthographicSize <= minZoon)
        {
            mainCamera.orthographicSize = minZoon;
            initializing = false;
        }
    }

    private void ZoomViaTouch()
    {
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            DoZoom(deltaMagnitudeDiff);
        }
    }

    private void ZoomViaMouse()
    {
        if (Input.mouseScrollDelta != Vector2.zero)
        {
            DoZoom(Input.mouseScrollDelta.y * zoomSpeedMouse);
        }
    }

    private void DoZoom(float deltaMagnitudeDiff)
    {
        if (mainCamera.orthographic && mainCamera.orthographicSize <= maxZoom || mainCamera.orthographicSize >= minZoon)
        {
            mainCamera.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;
            mainCamera.orthographicSize = Mathf.Max(mainCamera.orthographicSize, 0.1f);

            if (mainCamera.orthographicSize > maxZoom)
            {
                mainCamera.orthographicSize = maxZoom;
            }
            else if (mainCamera.orthographicSize < minZoon)
            {
                mainCamera.orthographicSize = minZoon;
            }
        }
        else if (mainCamera.fieldOfView <= maxZoom || mainCamera.orthographicSize >= minZoon)
        {
            mainCamera.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;
            mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView, minZoon, maxZoom);
        }
    }
}