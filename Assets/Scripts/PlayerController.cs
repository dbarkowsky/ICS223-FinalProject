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
    private uint hp = 1; // character only takes one hit...
    

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

        // don't let player leave view!
        // bottom left is [0,0], top right is [1,1]
        Vector3 positionRelativeToCamera = cam.WorldToViewportPoint(transform.position);
        if (positionRelativeToCamera.y < 0.05 && yMove < 0) { yMove = 0; }
        if (positionRelativeToCamera.y > 0.95 && yMove > 0) { yMove = 0; }
        if (positionRelativeToCamera.x < 0.03 && xMove < 0) { xMove = 0; }
        if (positionRelativeToCamera.x > 0.70 && xMove > 0) { xMove = 0; }

        Vector3 direction = new Vector3(xMove, yMove) * speed * Time.deltaTime;
        transform.position += Vector3.ClampMagnitude(new Vector3(direction.x, direction.y, 0), 1);
        //rb.position += Vector2.ClampMagnitude(new Vector2(direction.x, direction.y), 1);
        //rb.MovePosition(Vector2.ClampMagnitude(new Vector2(direction.x, direction.y), 1));

        foreach (FiringPointController point in firingPoints)
        {
            if (firing && point.pointCanShoot())
            {
                point.Fire();
            }
        }
        
        //if (hp <= 0)
        //{
        //    Messenger.Broadcast(GameEvent.PLAYER_DEAD);
        //    Debug.Log("player hit");
        //}
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyBullet"))
        {
            Debug.Log("hit player");
            Messenger.Broadcast(GameEvent.PLAYER_DEAD);
            Destroy(other.gameObject);
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit");
        if (other.CompareTag("EnemyBullet"))
        {
            hp -= 1;
            Destroy(other.gameObject);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("wow");
    }
}
