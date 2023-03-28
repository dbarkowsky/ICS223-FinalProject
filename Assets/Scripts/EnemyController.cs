using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementPatterns
{
    CurveFromRight,
    CurveFromLeft,
    DropDownPauseContinue,
    MoveDownSlowly,
    DropDownWiggle
}

public class EnemyController : MonoBehaviour
{
    [SerializeField] private MovementPatterns movementPattern;
    private BoxCollider2D collider;
    private float speed = 5f;
    Vector2 pausePos = new Vector2(-6.5f, 0f);
    Vector2 endPos = new Vector2(-6.5f, -15f);
    Transform from;
    Transform to;
    float timeCount = 0.0f;
    private bool canShoot = true;

    [SerializeField] FiringPointController[] firingPoints;
    FiringPointController mainFiringPoint;

    // Start is called before the first frame update
    void Start()
    {
        collider = gameObject.GetComponent<BoxCollider2D>();
        mainFiringPoint = firingPoints[0];
        switch (movementPattern)
        {
            case MovementPatterns.DropDownPauseContinue:
                StartCoroutine(FlyDown_Pause_FlyDown());
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (FiringPointController point in firingPoints)
        {
            if (point.pointCanShoot() && canShoot)
            {
                point.Fire();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            Debug.Log("enemy hit");
            Messenger<GameObject>.Broadcast(GameEvent.ENEMY_DESTROYED, this.gameObject);
            Destroy(other.gameObject);
        }
    }

    // Given a camera, determines if the enemy is visible on screen and sets their ability to shoot
    public void AmIOnScreen(Camera cam)
    {
        // Get the enemy's position relative to the centre of the camera
        // bottom left is [0,0], top right is [1,1]
        Vector3 positionRelativeToCamera = cam.WorldToViewportPoint(transform.position);
        float screenBuffer = 0.1f; // the area on edge of screen where an enemy might be, but shouldn't be shooting from
        if (
            positionRelativeToCamera.x > 0 + screenBuffer && 
            positionRelativeToCamera.x < 0.8 - screenBuffer &&
            positionRelativeToCamera.y > 0 + screenBuffer &&
            positionRelativeToCamera.y < 1 - screenBuffer
        )
        {
            canShoot = true;
        } else
        {
            canShoot = false;
        }
    }

    // Is this enemy below the screen? Delete it
    public bool AmIBelowScreen(Camera cam)
    {
        // Get the enemy's position relative to the centre of the camera
        // bottom left is [0,0], top right is [1,1]
        Vector3 positionRelativeToCamera = cam.WorldToViewportPoint(transform.position);
        if (positionRelativeToCamera.y < -0.1)
        {
            return true;
        }
        return false;
    }

    // All movement patterns go here:
    IEnumerator FlyDown_Pause_FlyDown()
    {
        float timeElapsed = 0;
        float durationPerMove = 2;

        while (timeElapsed < durationPerMove)
        {
            Vector2 currentPosition = transform.position;
            float time = timeElapsed / durationPerMove;
            transform.position = Vector2.Lerp(currentPosition, currentPosition - new Vector2(0f, 6f), time * Time.deltaTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSecondsRealtime(1);
        timeElapsed = 0;
        while (timeElapsed < durationPerMove)
        {
            Vector2 currentPosition = transform.position;
            float time = timeElapsed / durationPerMove;
            transform.position = Vector2.Lerp(currentPosition, currentPosition - new Vector2(0f, 12f), time * Time.deltaTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }

}
