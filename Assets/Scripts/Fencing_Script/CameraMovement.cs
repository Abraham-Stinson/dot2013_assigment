using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform firstPlayer, secondPlayer;
    [SerializeField] private float firstPlayerXPosition, secondPlayerXPosition;

    [SerializeField] private float cameraXPosition;
    [SerializeField] private float distancePlayer;
    [SerializeField] private float minSize = 2f, maxSize = 8.75f;
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float minDistance = 0f, maxDistance = 30f;
    [SerializeField] private float minYPosition = -2f, maxYPosition = 2f;
    [SerializeField] private float minXPosition = -5f, maxXPosition = 5f;

    Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        distancePlayer = Vector3.Distance(firstPlayer.position, secondPlayer.position);
        float distanceRatio = Mathf.InverseLerp(minDistance, maxDistance, distancePlayer);
        CameraMove(distanceRatio);
        CameraZoom(distanceRatio);
    }

    void CameraZoom(float distanceRatio)
    {
        float cameraZoom = Mathf.Lerp(minSize, maxSize, distanceRatio);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, cameraZoom, Time.deltaTime * zoomSpeed);
    }

    void CameraMove(float distanceRatio)
    {
        float firstPlayerXPosition = firstPlayer.position.x;
        float secondPlayerXPosition = secondPlayer.position.x;
        float betweenPlayer = firstPlayerXPosition - secondPlayerXPosition;
        cameraXPosition = (firstPlayerXPosition + secondPlayerXPosition) / 2;

        // X pozisyonunu min ve max s?n?rlamalara göre k?s?tla
        cameraXPosition = Mathf.Clamp(cameraXPosition, minXPosition, maxXPosition);

        float cameraYPosition = Mathf.Lerp(minYPosition, maxYPosition, distanceRatio);
        transform.position = new Vector3(cameraXPosition, cameraYPosition, -10);
    }
}
