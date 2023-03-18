using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour
{
    // Character control
    private CharacterController cc;
    private float speed = 8f;

    // Character shooting
    [SerializeField] FiringPointController[] firingPoints;
    FiringPointController mainFiringPoint;

    // Character attributes
    private uint hp = 1; // character only takes one hit...
    

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
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

        Vector2 direction = new Vector2(xMove, yMove) * speed * Time.deltaTime;
        cc.Move(Vector2.ClampMagnitude(direction + new Vector2(0, Constants.scrollSpeed) * Time.deltaTime, 1));

        foreach(FiringPointController point in firingPoints)
        {
            if (firing && point.pointCanShoot())
            {
                StartCoroutine(point.Fire());
            }
        }
        
        if (hp == 0)
        {
            Messenger<PlayerController>.Broadcast(GameEvent.PLAYER_DEAD, this);
        }
    }


}
