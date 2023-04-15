using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector2 currentPosition;
    private Vector2 endPosition;
    private bool isMoving = true;
    // Start is called before the first frame update
    void Start()
    {
        // Was used prior to triggering system.
        //currentPosition = transform.position;
        //endPosition = new Vector2(currentPosition.x, Constants.endOfLevelY);
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            transform.position += new Vector3(0, Constants.scrollSpeed * Time.deltaTime, 0);
        }
    }

    public void Stop()
    {
        isMoving = false;
    }

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
