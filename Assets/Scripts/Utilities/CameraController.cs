using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls the camera's behaviours
public class CameraController : MonoBehaviour
{
    private Vector2 currentPosition;
    private Vector2 endPosition;
    private bool isMoving = true;

    // Move the camera up if active
    void Update()
    {
        if (isMoving)
        {
            transform.position += new Vector3(0, Constants.scrollSpeed * Time.deltaTime, 0);
        }
    }

    // Stops the camera
    public void Stop()
    {
        isMoving = false;
    }

    // Starts moving the camera
    public void Move()
    {
        isMoving = true;
    }

    private void Awake()
    {
        Messenger.AddListener(GameEvent.STOP_CAMERA, this.OnStopCamera);

    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.STOP_CAMERA, this.OnStopCamera);
    }

    private void OnStopCamera()
    {
        this.Stop();
    }
}
