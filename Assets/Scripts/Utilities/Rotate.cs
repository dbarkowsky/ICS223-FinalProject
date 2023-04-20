using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attach to objects to rotate them
public class Rotate : MonoBehaviour
{
    [SerializeField] private float speed = 0.5f;

    private void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, 0, -360) * speed * Time.deltaTime);
    }
}
