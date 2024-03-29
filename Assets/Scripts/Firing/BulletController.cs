using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType
{
    Player,
    Enemy
}

// Controls a single bullet instance
public class BulletController : MonoBehaviour
{
    public float speed = 30f;
    private float rotateSpeed = 360 * 5;
    Vector3 direction = Vector3.up; // default is upwards
    private float angle = 0f; // IN DEGREES! TRIG FUNCTIONS DON'T WORK LIKE THIS, CONVERT TO RADIANS BEFORE TRIG
    [SerializeField] BulletType type;
    [SerializeField] float timeToLive = 5f;

    // Determine direction and start self-destruct coroutine
    void Start()
    {
        SetDirection();
        StartCoroutine(DestroyMe());
    }

    // Move the bullet. Rotate if enemy bullet
    void Update()
    {
        transform.position += (direction * speed * Time.deltaTime);
        if (type == BulletType.Enemy)
        {
            transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime); // Quaternion.Euler(Vector3.forward * rotateSpeed * Time.deltaTime);
        } 
    }

    // Destroys the object after some time. Helps keep object under control
    IEnumerator DestroyMe()
    {
        yield return new WaitForSeconds(timeToLive);
        Destroy(gameObject);
    }

    // Uses a provided angle to determine the appropriate x and y velocities that would create that vector
    private void SetDirection()
    {
        // determine x and y velocities based on angle
        float absoluteAngle = Mathf.Abs(angle);
        float relativeAngle = (absoluteAngle) % 90;
        float xVelocity;
        float yVelocity;

        // non-right angles need to be specified first
        if (absoluteAngle == 0)
        {
            xVelocity = 0;
            yVelocity = 1;
        } 
        else if (absoluteAngle == 90)
        {
            xVelocity = 1;
            yVelocity = 0;
        } 
        else if (absoluteAngle == 180)
        {
            xVelocity = 0;
            yVelocity = -1;
        } 
        else if (absoluteAngle == 270)
        {
            xVelocity = -1;
            yVelocity = 0;
        }
        if (absoluteAngle < 90)
        {
            // x and y positive
            xVelocity = Mathf.Sin(ConvertDegreesToRadians(relativeAngle));
            yVelocity = Mathf.Cos(ConvertDegreesToRadians(relativeAngle));
        }
        else if (absoluteAngle < 180)
        {
            // x positive, y negative
            xVelocity = Mathf.Cos(ConvertDegreesToRadians(relativeAngle));
            yVelocity = Mathf.Sin(ConvertDegreesToRadians(relativeAngle)) * -1;
        }
        else if (absoluteAngle < 270)
        {
            // x and y negative
            xVelocity = Mathf.Sin(ConvertDegreesToRadians(relativeAngle)) * -1;
            yVelocity = Mathf.Cos(ConvertDegreesToRadians(relativeAngle)) * -1;
        }
        else
        {
            // x negative, y positive
            xVelocity = Mathf.Cos(ConvertDegreesToRadians(relativeAngle)) * -1;
            yVelocity = Mathf.Sin(ConvertDegreesToRadians(relativeAngle));
        }

        // flip if it's an enemy bullet
        if (type == BulletType.Enemy)
        {
            xVelocity *= -1;
            yVelocity *= -1;
        }

        // save as the direction
        direction = new Vector3(xVelocity, yVelocity, 0);
    }

    // Sets the angle of the bullet
    public void SetAngle(float newAngle)
    {
        angle = newAngle;
        if (angle > 359 || angle < 0)
        {
            angle = 0;
        }
        SetDirection();
    }

    // Converts degrees to radians, because the Mathf functions only use radians.
    private float ConvertDegreesToRadians(float degrees)
    {
        float radians = (Mathf.PI / 180) * degrees;
        return (radians);
    }
}
