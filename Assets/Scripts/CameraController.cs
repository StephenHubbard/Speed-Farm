using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private CinemachineVirtualCamera _cinemachineVirtualCamera;

    private void Awake() {
        _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        HandleMovement();
        HandleZoom();
    }

    private void HandleMovement()
    {
        Vector2 inputMoveDir = GetCameraMoveVector();

        float moveSpeed = 10f;

        Vector3 moveVector = transform.up * inputMoveDir.y + transform.right * inputMoveDir.x;
        transform.position += moveVector * moveSpeed * Time.deltaTime;
    }

    private void HandleZoom()
    {
        float zoomIncreaseAmount = 15f;
        float currentZoom = _cinemachineVirtualCamera.m_Lens.OrthographicSize;
        float targetZoom = currentZoom;
        targetZoom += GetCameraZoomAmount() * zoomIncreaseAmount;

        float zoomSpeed = 5f;

        _cinemachineVirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(currentZoom, targetZoom, Time.deltaTime * zoomSpeed);
    }

    private Vector2 GetCameraMoveVector()
    {
        Vector2 inputMoveDir = new Vector2(0, 0);

        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDir.y = +1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputMoveDir.y = -1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDir.x = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDir.x = +1f;
        }

        return inputMoveDir;
    }

    private float GetCameraZoomAmount()
    {
        float zoomAmount = 0f;

        if (Input.mouseScrollDelta.y > 0)
        {
            zoomAmount = -1f;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            zoomAmount = +1f;
        }

        return zoomAmount;
    }
}