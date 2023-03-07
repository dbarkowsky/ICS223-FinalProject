using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour
{
    private CharacterController cc;
    private float speed = 8f;
    [SerializeField] GameObject bullet;
    private bool canShoot = true; // used to handle cooldown between shots

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

        if (firing & canShoot)
        {
            canShoot = false;
            StartCoroutine(Fire());
        }
    }

    IEnumerator Fire()
    {
        Vector3 pos = transform.position;
        Quaternion rotation = transform.rotation;
        Instantiate(bullet, pos, rotation);
        yield return new WaitForSecondsRealtime(0.125f);
        canShoot = true;
    }
}
