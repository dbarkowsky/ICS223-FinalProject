using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour
{
    private CharacterController cc;
    private float speed = 8f;
    [SerializeField] FiringPointController[] firingPoints;
    FiringPointController mainFiringPoint;
    

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        mainFiringPoint = firingPoints[0];
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
            if (firing & point.canShoot)
            {
                point.canShoot = false;
                StartCoroutine(point.Fire());
            }
        }
        
    }


}
