using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector2 currentPosition;
    private Vector2 endPosition;
    // Start is called before the first frame update
    void Start()
    {
        currentPosition = transform.position;
        endPosition = new Vector2(currentPosition.x, Constants.endOfLevelY);
    }

    // Update is called once per frame
    void Update()
    {
        currentPosition = transform.position;

        if (currentPosition.y < endPosition.y)
        {
            transform.position += new Vector3(0, Constants.scrollSpeed * Time.deltaTime, 0);
        }
    }
}
