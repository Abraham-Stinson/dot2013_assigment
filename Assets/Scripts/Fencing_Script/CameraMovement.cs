using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform firstPlayer, secondPlayer;
    float firstPlayerXPosition, secondPlayerXPosition;

    float cameraXPosition;
    float distancePlayer;
    float minSize = 2f, maxSize = 8.75f;
    float zoomSpeed = 5f;
    float minDistance = 0f, maxDistance = 30f;
    float minYPosition = -1f, maxYPosition = 1f;
    Camera cam;
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        distancePlayer = Vector3.Distance(firstPlayer.position, secondPlayer.position);
        float distanceRatio = Mathf.InverseLerp(minDistance, maxDistance, distancePlayer);
        CameraMove(distanceRatio);
        CameraZoom(distanceRatio);
    }
    
    void CameraZoom(float distanceRatio)
    {
        
        float cameraZoom = Mathf.Lerp(minSize,maxSize, distanceRatio);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize,cameraZoom,Time.deltaTime*zoomSpeed);
    }
    void CameraMove(float distanceRatio)
    {
        float firstPlayerXPosition = firstPlayer.position.x;
        float secondPlayerXPosition = secondPlayer.position.x;
        float betweenPlayer = firstPlayerXPosition - secondPlayerXPosition;
        cameraXPosition = (firstPlayerXPosition + secondPlayerXPosition) / 2;

        float cameraYPosition = Mathf.Lerp(minYPosition,maxYPosition, distanceRatio);
        transform.position = new Vector3(cameraXPosition, cameraYPosition, -10);
    }
}
