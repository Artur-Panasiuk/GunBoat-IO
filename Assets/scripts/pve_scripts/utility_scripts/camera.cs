using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    public Player player;
    public float cameraBaseZoom = 12f;
    public float cameraSigleZoomOutValue = 3f;
    public float cameraSmoothness = 6f;
    private Camera playerCamera;
    private float currentCameraZoom = 0f;
    private float oldCameraZoom = 0f;

    private void UpdateCameraPosition()
    {
        playerCamera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, playerCamera.transform.position.z);
    }

    private void UpdateCameraZoom()
    {
        currentCameraZoom = cameraBaseZoom + (cameraSigleZoomOutValue * (int)player.getSpeedLevel());

        float smoothZoom = Mathf.Lerp(oldCameraZoom, currentCameraZoom, Time.deltaTime * cameraSmoothness);

        oldCameraZoom = smoothZoom;

        playerCamera.orthographicSize = smoothZoom;
    }

    void Start()
    {
        playerCamera = Camera.main;
    }

    void Update()
    {
        UpdateCameraPosition();
        UpdateCameraZoom();
    }
}
