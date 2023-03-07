using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController cc;
    private float speed = 8f;
    [SerializeField] GameObject bullet;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float xMove = Input.GetAxis("Horizontal");
        float yMove = Input.GetAxis("Vertical");
        bool firing = Input.GetButton("Fire");

        Vector2 direction = new Vector2(xMove, yMove) * speed * Time.deltaTime;
        cc.Move(Vector2.ClampMagnitude(direction, 1));

        if (firing)
        {
            Instantiate(bullet, transform);
        }
    }
}
