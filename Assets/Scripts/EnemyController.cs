using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementPatterns
{
    Stationary,
    StationaryLand,
    EnterFromRight,
    EnterFromLeft,
    DropDownPauseContinue,
    MoveDownSlowly,
    DropDownWiggleContinue,
    DropDownFloatUp
}

public class EnemyController : MonoBehaviour
{
    [SerializeField] private MovementPatterns movementPattern;
    [SerializeField] private float pauseDuration = 1f;
    [SerializeField] private float moveDuration = 2f;
    private BoxCollider2D collider;
    private bool enemyCanShoot = true;

    [SerializeField] FiringPointController[] firingPoints;
    FiringPointController mainFiringPoint;

    // Start is called before the first frame update
    void Start()
    {
        collider = gameObject.GetComponent<BoxCollider2D>();
        mainFiringPoint = firingPoints[0];
        switch (movementPattern)
        {
            case MovementPatterns.Stationary:
                break;
            case MovementPatterns.StationaryLand:
                StartCoroutine(StationaryLand());
                break;
            case MovementPatterns.DropDownPauseContinue:
                StartCoroutine(DropDownPauseContinue());
                break;
            case MovementPatterns.DropDownWiggleContinue:
                StartCoroutine(DropDownWiggleContinue());
                break;
            case MovementPatterns.MoveDownSlowly:
                StartCoroutine(MoveDownSlowly());
                break;
            case MovementPatterns.DropDownFloatUp:
                StartCoroutine(DropDownFloatUp());
                break;
            case MovementPatterns.EnterFromRight:
                StartCoroutine(EnterFromRight());
                break;
            case MovementPatterns.EnterFromLeft:
                StartCoroutine(EnterFromLeft());
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
            if (point.pointCanShoot() && enemyCanShoot)
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
            Messenger<GameObject>.Broadcast(GameEvent.ENEMY_DESTROYED_SELF, this.gameObject);
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
            StartCoroutine(CanShootSoon());
        } else
        {
            enemyCanShoot = false;
        }
    }

    private IEnumerator CanShootSoon()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        enemyCanShoot = true;
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

    IEnumerator StationaryLand()
    {
        while (true)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - Constants.scrollSpeed * Time.deltaTime);
            yield return null;
        }
    }
    IEnumerator DropDownPauseContinue()
    {
        float timeElapsed = 0;
        float durationPerMove = moveDuration;

        while (timeElapsed < durationPerMove)
        {
            Vector2 currentPosition = transform.position;
            float time = timeElapsed / durationPerMove;
            transform.position = Vector2.Lerp(currentPosition, currentPosition - new Vector2(0f, 4f), time * Time.deltaTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSecondsRealtime(pauseDuration);
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

    IEnumerator EnterFromRight()
    {
        float timeElapsed = 0;
        float durationPerMove = moveDuration;

        while (timeElapsed < durationPerMove)
        {
            Vector2 currentPosition = transform.position;
            float time = timeElapsed / durationPerMove;
            transform.position = Vector2.Lerp(currentPosition, currentPosition - new Vector2(3.5f, 0f), time * Time.deltaTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        // Rotate
        float duration = 0.5f;
        float t = 0;
        Quaternion startRotation = transform.rotation;
        float endZRot = 45f;

        while (t < 1f)
        {
            t = Mathf.Min(1f, t + Time.deltaTime / duration);
            Vector3 newEulerOffset = Vector3.forward * (endZRot * t);
            // global z rotation
            transform.rotation = Quaternion.Euler(newEulerOffset) * startRotation;
            yield return null;
        }
        timeElapsed = 0;
        while (timeElapsed < durationPerMove)
        {
            Vector2 currentPosition = transform.position;
            float time = timeElapsed / durationPerMove;
            transform.position = Vector2.Lerp(currentPosition, currentPosition - new Vector2(12f, 12f), time * Time.deltaTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator EnterFromLeft()
    {
        float timeElapsed = 0;
        float durationPerMove = moveDuration;

        while (timeElapsed < durationPerMove)
        {
            Vector2 currentPosition = transform.position;
            float time = timeElapsed / durationPerMove;
            transform.position = Vector2.Lerp(currentPosition, currentPosition - new Vector2(-3.5f, 0f), time * Time.deltaTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        // Rotate
        float duration = 0.5f;
        float t = 0;
        Quaternion startRotation = transform.rotation;
        float endZRot = -45f;

        while (t < 1f)
        {
            t = Mathf.Min(1f, t + Time.deltaTime / duration);
            Vector3 newEulerOffset = Vector3.forward * (endZRot * t);
            // global z rotation
            transform.rotation = Quaternion.Euler(newEulerOffset) * startRotation;
            yield return null;
        }
        timeElapsed = 0;
        while (timeElapsed < durationPerMove)
        {
            Vector2 currentPosition = transform.position;
            float time = timeElapsed / durationPerMove;
            transform.position = Vector2.Lerp(currentPosition, currentPosition - new Vector2(-12f, 12f), time * Time.deltaTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator DropDownFloatUp()
    {
        float timeElapsed = 0;
        float durationPerMove = moveDuration;

        while (timeElapsed < durationPerMove)
        {
            Vector2 currentPosition = transform.position;
            float time = timeElapsed / durationPerMove;
            transform.position = Vector2.Lerp(currentPosition, currentPosition - new Vector2(0f, 5f), time * Time.deltaTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        timeElapsed = 0;
        while (timeElapsed < durationPerMove)
        {
            Vector2 currentPosition = transform.position;
            float time = timeElapsed / durationPerMove;
            transform.position = Vector2.Lerp(currentPosition, currentPosition - new Vector2(0f, -12f), time * Time.deltaTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        Messenger<GameObject>.Broadcast(GameEvent.ENEMY_DESTROYED_SELF, this.gameObject);
    }

    IEnumerator DropDownWiggleContinue()
    {
        float timeElapsed = 0;
        float durationPerMove = moveDuration;

        while (timeElapsed < durationPerMove)
        {
            Vector2 currentPosition = transform.position;
            float time = timeElapsed / durationPerMove;
            transform.position = Vector2.Lerp(currentPosition, currentPosition - new Vector2(0f, 6f), time * Time.deltaTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        timeElapsed = 0;
        while (timeElapsed < durationPerMove)
        {
            Vector2 currentPosition = transform.position;
            float time = timeElapsed / durationPerMove;
            transform.position = Vector2.Lerp(currentPosition, currentPosition - new Vector2(1f, 0f), time * Time.deltaTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        timeElapsed = 0;
        while (timeElapsed < durationPerMove)
        {
            Vector2 currentPosition = transform.position;
            float time = timeElapsed / durationPerMove;
            transform.position = Vector2.Lerp(currentPosition, currentPosition - new Vector2(-2f, 0f), time * Time.deltaTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        timeElapsed = 0;
        while (timeElapsed < durationPerMove)
        {
            Vector2 currentPosition = transform.position;
            float time = timeElapsed / durationPerMove;
            transform.position = Vector2.Lerp(currentPosition, currentPosition - new Vector2(1f, 0f), time * Time.deltaTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
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

    private IEnumerator MoveDownSlowly()
    {
        while (true)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - 0.005f);
            yield return null;
        }
    }
}
