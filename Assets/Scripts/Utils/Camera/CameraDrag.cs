using UnityEngine;

namespace Hexwar
{
    public class CameraDrag : MonoBehaviour
    {
        private Vector3 resetCamera;
        private Vector3 origin;
        private Vector3 diference;
        private bool drag = false;

        public Camera cameraRef;

        private bool wasZooming = false;

        void Start()
        {
            resetCamera = cameraRef.transform.position;
        }

        void LateUpdate()
        {
            if (Input.touchCount == 0 && wasZooming)
            {
                wasZooming = false;
            }

            if (wasZooming)
            {
                return;
            }

            if (Input.touchCount > 1)
            {
                wasZooming = true;
                return;
            }
            else if (GameManager.Instance.GameState == EGameState.Gameplay)
            {
                if (Input.GetMouseButton(0))
                {
                    diference = (cameraRef.ScreenToWorldPoint(Input.mousePosition) - cameraRef.transform.position);

                    if (!drag)
                    {
                        drag = true;
                        origin = cameraRef.ScreenToWorldPoint(Input.mousePosition);
                    }
                }
                else
                {
                    drag = false;
                }

                if (drag)
                {
                    cameraRef.transform.position = origin - diference;
                }
            }
        }
    }
}