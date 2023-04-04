using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour
{
    // Character control
    private float speed = 8f;

    // Character shooting
    [SerializeField] FiringPointController[] firingPoints;
    FiringPointController mainFiringPoint;
    Rigidbody2D rb;
    [SerializeField] Camera cam;

    // Character attributes
    private bool isDead = false;
    public bool canBeHit = true;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainFiringPoint = firingPoints[0];

        // disable all firing points to start. They are activated upon pickups
        foreach (FiringPointController point in firingPoints)
        {
            point.isActive = false;
        }
        // Activate main firing point
        mainFiringPoint.isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        float xMove = Input.GetAxis("Horizontal");
        float yMove = Input.GetAxis("Vertical");
        bool firing = Input.GetButton("Fire");

        // Player control loop
        if (!isDead)
        {
            // don't let player leave view!
            // bottom left is [0,0], top right is [1,1]
            Vector3 positionRelativeToCamera = cam.WorldToViewportPoint(transform.position);
            if (positionRelativeToCamera.y < 0.05 && yMove < 0) { yMove = 0; }
            if (positionRelativeToCamera.y > 0.95 && yMove > 0) { yMove = 0; }
            if (positionRelativeToCamera.x < 0.03 && xMove < 0) { xMove = 0; }
            if (positionRelativeToCamera.x > 0.70 && xMove > 0) { xMove = 0; }

            Vector3 direction = new Vector3(xMove, yMove) * speed * Time.deltaTime;
            transform.position += Vector3.ClampMagnitude(new Vector3(direction.x, direction.y, 0), 1);

            foreach (FiringPointController point in firingPoints)
            {
                if (firing && point.pointCanShoot())
                {
                    point.Fire();
                }
            }
        }
    }

    public void Respawn()
    {
        transform.position = new Vector3(cam.transform.position.x - 1.8571f, cam.transform.position.y - 10, transform.position.z);
        StartCoroutine(MoveIntoView());
    }

    IEnumerator MoveIntoView()
    {
        this.canBeHit = false;
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        yield return new WaitForSecondsRealtime(2);
        float timeElapsed = 0;
        float durationPerMove = 1;

        while (timeElapsed < durationPerMove)
        {
            Vector2 currentPosition = transform.position;
            float time = timeElapsed / durationPerMove;
            transform.position = Vector2.Lerp(currentPosition, currentPosition + new Vector2(0f, 12f), time * Time.deltaTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        this.isDead = false;
        
        yield return new WaitForSecondsRealtime(2);
        Debug.Log("canbehit again");
        this.canBeHit = true;
        gameObject.GetComponent<CircleCollider2D>().enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyBullet") && !this.isDead && this.canBeHit)
        {
            this.isDead = true;
            Debug.Log("hit player");
            Messenger.Broadcast(GameEvent.PLAYER_DEAD);
            Destroy(other.gameObject);
        }
    }

}
