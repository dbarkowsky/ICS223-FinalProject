using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector2 currentPosition = transform.position;
        Vector2 endPosition = new Vector2(currentPosition.x, 147);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 currentPosition = transform.position;
        transform.position += new Vector3(0, Constants.scrollSpeed * Time.deltaTime, 0);
    }
}
