using UnityEngine;

public class PinchZoom : MonoBehaviour
{
    public float perspectiveZoomSpeed = 0.01f;
    public float orthoZoomSpeed = 0.01f;
    public float minZoon = 5;
    public float maxZoom = 20;
    private float zoomSpeedMouse = -25;

    void Update()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
        {
            ZoomViaTouch();
        }
        else
        {
            ZoomViaMouseAndKeyboard();
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

    private void ZoomViaMouseAndKeyboard()
    {
        if (Input.mouseScrollDelta != Vector2.zero)
        {
            DoZoom(Input.mouseScrollDelta.y * zoomSpeedMouse);
        }
    }

    private void DoZoom(float deltaMagnitudeDiff)
    {
        if (Camera.main.orthographic && Camera.main.orthographicSize <= maxZoom || Camera.main.orthographicSize >= minZoon)
        {
            Camera.main.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;
            Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize, 0.1f);

            if (Camera.main.orthographicSize > maxZoom)
            {
                Camera.main.orthographicSize = maxZoom;
            }
            else if (Camera.main.orthographicSize < minZoon)
            {
                Camera.main.orthographicSize = minZoon;
            }
        }
        else if (Camera.main.fieldOfView <= maxZoom || Camera.main.orthographicSize >= minZoon)
        {
            Camera.main.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;
            Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, minZoon, maxZoom);
        }
    }
}