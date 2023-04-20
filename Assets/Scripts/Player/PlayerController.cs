using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

// Controls the player character behaviours
public class PlayerController : MonoBehaviour
{
    // Character control
    private float speed = 5f;

    // Character shooting
    [SerializeField] FiringPointController[] firingPoints;
    FiringPointController mainFiringPoint;
    Rigidbody2D rb;
    [SerializeField] Camera cam;

    // Character attributes
    private bool isDead = false;
    public bool canBeHit = true;

    // Animation Controller
    [SerializeField] Animator anim;
    

    // Set main firing point
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

    // Listen for player input and act on it
    void Update()
    {
        float xMove = Input.GetAxis("Horizontal");
        float yMove = Input.GetAxis("Vertical");
        bool firing = Input.GetButton("Fire");

        // Player control loop
        if (!isDead)
        {
            // Change animation
            anim.SetFloat("xDirection", xMove);
            // don't let player leave view!
            // bottom left is [0,0], top right is [1,1]
            Vector3 positionRelativeToCamera = cam.WorldToViewportPoint(transform.position);
            if (positionRelativeToCamera.y < 0.05 && yMove < 0) { yMove = 0; }
            if (positionRelativeToCamera.y > 0.95 && yMove > 0) { yMove = 0; }
            if (positionRelativeToCamera.x < 0.03 && xMove < 0) { xMove = 0; }
            if (positionRelativeToCamera.x > 0.77 && xMove > 0) { xMove = 0; }

            Vector3 direction = new Vector3(xMove, yMove) * speed * Time.deltaTime;
            transform.position += Vector3.ClampMagnitude(new Vector3(direction.x, direction.y, 0), 1);

            // Fire the guns
            foreach (FiringPointController point in firingPoints)
            {
                if (firing && point.pointCanShoot())
                {
                    Messenger.Broadcast(GameEvent.PLAYER_SHOOTS);
                    point.Fire();
                }
            }
        }
    }

    // Respawns the player offscreen, moves them back up
    public void Respawn()
    {
        transform.position = new Vector3(cam.transform.position.x - 1.8571f, cam.transform.position.y - 10, transform.position.z);
        StartCoroutine(MoveIntoView());
    }

    // Disables control and hitbox
    public void Disable()
    {
        isDead = true;
        canBeHit = false;
        gameObject.GetComponent<PolygonCollider2D>().enabled = false;
    }

    // Enables control and hitbox
    public void Enable()
    {
        isDead = false;
        canBeHit = true;
        gameObject.GetComponent<PolygonCollider2D>().enabled = true;
    }

    // Moves player up and out of screen
    public IEnumerator ExitTopOfScreen(float durationPerMove = 3)
    {
        float timeElapsed = 0;

        while (timeElapsed < durationPerMove)
        {
            Vector2 currentPosition = transform.position;
            float time = timeElapsed / durationPerMove;
            transform.position = Vector2.Lerp(currentPosition, currentPosition + new Vector2(0f, 12f), time * Time.deltaTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }

    // Moves player up and into screen
    IEnumerator MoveIntoView()
    {
        this.Disable();
        yield return new WaitForSeconds(2);
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
        yield return new WaitForSeconds(2);
        this.canBeHit = true;
        gameObject.GetComponent<PolygonCollider2D>().enabled = true;
    }

    // Handle collisions with enemies and bullets
    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.CompareTag("EnemyBullet") || other.CompareTag("EnemyAir")) && !this.isDead && this.canBeHit)
        {
            this.isDead = true;
            Messenger.Broadcast(GameEvent.PLAYER_DEAD);
            Destroy(other.gameObject);
        }
    }

    private void Awake()
    {
        Messenger<PickupController>.AddListener(GameEvent.PICKUP_TOUCHED, OnPickupTouched);
    }

    private void OnDestroy()
    {
        Messenger<PickupController>.RemoveListener(GameEvent.PICKUP_TOUCHED, OnPickupTouched);
    }

    // Adjusts the firing point based on the pickup type
    private void OnPickupTouched(PickupController pickup)
    {
        mainFiringPoint.AdjustCooldown(-0.02f);
        switch (pickup.GetPickupType())
        {
            case PickupType.BulletFocus:
                mainFiringPoint.SetPattern(FiringPattern.Focus);
                break;
            case PickupType.BulletSpread:
                mainFiringPoint.SetPattern(FiringPattern.TripleSpread);
                break;
            default:
                break;
        }
    }
}
