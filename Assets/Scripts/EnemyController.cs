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
    private float speed = 5f;
    Vector2 startPos = new Vector2(-6.5f, 7f);
    Vector2 pausePos = new Vector2(-6.5f, 0f);
    Vector2 endPos = new Vector2(-6.5f, -15f);
    Transform from;
    Transform to;
    float timeCount = 0.0f;

    [SerializeField] FiringPointController[] firingPoints;
    FiringPointController mainFiringPoint;

    // Start is called before the first frame update
    void Start()
    {
        mainFiringPoint = firingPoints[0];
        StartCoroutine(FlyDown_Pause_FlyDown(startPos, endPos, pausePos));
    }

    // Update is called once per frame
    void Update()
    {
        foreach (FiringPointController point in firingPoints)
        {
            if (point.pointCanShoot())
            {
                point.Fire();
            }
        }
    }

    IEnumerator FlyDown_Pause_FlyDown(Vector2 startPos, Vector2 endPos, Vector2 pausePos)
    {
        float timeElapsed = 0;
        float durationPerMove = 2;

        transform.position = startPos;
        while (timeElapsed < durationPerMove)
        {
            Vector2 currentPosition = transform.position;
            float time = timeElapsed / durationPerMove;
            transform.position = Vector2.Lerp(currentPosition, pausePos, time * Time.deltaTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSecondsRealtime(1);
        timeElapsed = 0;
        while (timeElapsed < durationPerMove)
        {
            Vector2 currentPosition = transform.position;
            float time = timeElapsed / durationPerMove;
            transform.position = Vector2.Lerp(currentPosition, endPos, time * Time.deltaTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = endPos;
    }

}
