using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSystem2D : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private PolygonCollider2D polygonCollider2D;
    [SerializeField] private float _camMoveSpeed = 10f;
    [SerializeField] private bool useEdgeScrolling = false;
    [SerializeField] private bool useDragPan = false;
    [SerializeField] private float orthographicSizeMin = 10;
    [SerializeField] private float orthographicSizeMax = 50;
    [SerializeField] private float targetOrthographicSize = 10;

    private bool dragPanMoveActive;
    private Vector2 lastMousePosition;
    private Camera cam;

    private float mapMinX, mapMaxX, mapMinY, mapMaxY;

    private void Awake() {
        cam = Camera.main;

        mapMinX = polygonCollider2D.transform.position.x;
        mapMaxX = polygonCollider2D.bounds.size.x;

        mapMinY = polygonCollider2D.transform.position.y;
        mapMaxY = polygonCollider2D.bounds.size.y;
    }

    private void Update()
    {
        HandleCameraMovement();

        if (useEdgeScrolling)
        {
            HandleCameraMovementEdgeScrolling();
        }

        if (useDragPan)
        {
            HandleCameraMovementDragPan();
        }

        HandleCameraZoom_OrthographicSize();
    }

    private void HandleCameraMovement()
    {
        Vector3 inputDir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) inputDir.y = +1f;
        if (Input.GetKey(KeyCode.S)) inputDir.y = -1f;
        if (Input.GetKey(KeyCode.A)) inputDir.x = -1f;
        if (Input.GetKey(KeyCode.D)) inputDir.x = +1f;

        Vector3 moveVector = transform.up * inputDir.y + transform.right * inputDir.x;

        transform.position += moveVector * _camMoveSpeed * Time.deltaTime;

        transform.position = ClampCamera(transform.position);

    }

    private void HandleCameraMovementEdgeScrolling()
    {
        Vector3 inputDir = new Vector3(0, 0, 0);

        int edgeScrollSize = 20;

        if (Input.mousePosition.x < edgeScrollSize)
        {
            inputDir.x = -1f;
        }
        if (Input.mousePosition.y < edgeScrollSize)
        {
            inputDir.z = -1f;
        }
        if (Input.mousePosition.x > Screen.width - edgeScrollSize)
        {
            inputDir.x = +1f;
        }
        if (Input.mousePosition.y > Screen.height - edgeScrollSize)
        {
            inputDir.z = +1f;
        }

        Vector3 moveVector = transform.up * inputDir.y + transform.right * inputDir.x;

        transform.position += moveVector * _camMoveSpeed * Time.deltaTime;

        transform.position = ClampCamera(transform.position);
    }

    private void HandleCameraMovementDragPan()
    {
        Vector3 inputDir = new Vector3(0, 0, 0);

        if (Input.GetMouseButtonDown(2))
        {
            dragPanMoveActive = true;
            lastMousePosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(2))
        {
            dragPanMoveActive = false;
        }

        if (dragPanMoveActive)
        {
            Vector2 mouseMovementDelta = (Vector2)Input.mousePosition - lastMousePosition;

            float dragPanSpeed = 1f;
            inputDir.x = mouseMovementDelta.x * dragPanSpeed;
            inputDir.y = mouseMovementDelta.y * dragPanSpeed;

            lastMousePosition = Input.mousePosition;
        }

        Vector3 moveVector = transform.up * inputDir.y + transform.right * inputDir.x;

        transform.position += moveVector * _camMoveSpeed * Time.deltaTime;

        transform.position = ClampCamera(transform.position);
    }

    private void HandleCameraZoom_OrthographicSize()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            targetOrthographicSize -= 1;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            targetOrthographicSize += 1;
        }

        targetOrthographicSize = Mathf.Clamp(targetOrthographicSize, orthographicSizeMin, orthographicSizeMax);

        float zoomSpeed = 10f;

        cinemachineVirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(cinemachineVirtualCamera.m_Lens.OrthographicSize, targetOrthographicSize, Time.deltaTime * zoomSpeed);
    }

    private Vector3 ClampCamera(Vector3 targetPosition) {
        float camHeight = cinemachineVirtualCamera.m_Lens.OrthographicSize;
        float camWidth = cinemachineVirtualCamera.m_Lens.OrthographicSize * cam.aspect;

        float minX = mapMinX + camWidth;
        float maxX = mapMaxX - camWidth;
        float minY = mapMinY + camHeight;
        float maxY = mapMaxY - camHeight;

        float newX = Mathf.Clamp(targetPosition.x, minX, maxX);
        float newY = Mathf.Clamp(targetPosition.y, minY, maxY);

        return new Vector3(newX, newY, targetPosition.z);
    }

}
