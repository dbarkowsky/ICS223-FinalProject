using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controlls the behaviour of a single crab arm
public class CrabArmController : MonoBehaviour
{
    [SerializeField] private float armSpeed = 0.1f;
    [SerializeField] private GameObject laser;
    [SerializeField] private GameObject laserWarning;
    public bool canShoot = true;

    // Fires the laser beams
    public void Fire()
    {
        if (canShoot)
        {
            StartCoroutine(FireLaser());
        } else
        {
            StopAllCoroutines();
        }
    }

    // Actually generates the lazer beams
    private IEnumerator FireLaser()
    {
        canShoot = false;
        laserWarning.SetActive(true);
        Messenger.Broadcast(GameEvent.LASER_CHARGE);
        yield return new WaitForSeconds(0.5f);
        laser.SetActive(true);
        laserWarning.SetActive(false);
        Messenger.Broadcast(GameEvent.LASER_SHOOT);
        yield return new WaitForSeconds(1f);
        laser.SetActive(false);
        canShoot = true;
    }

    // Handle collisions with other objects
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            Destroy(other.gameObject);
            Messenger.Broadcast(GameEvent.CLAW_HIT);
        }
        if (other.CompareTag("Player"))
        {
            Messenger.Broadcast(GameEvent.PLAYER_DEAD);
        }
    }

    // Starts coroutine to move arm
    public void RotateArm(float degrees, float speed)
    {
        StartCoroutine(RotateArmEnum(degrees, speed));
        
    }

    // Moves the arm to a specified degree
    // 0 is right, 90 is up, 180 is left, 270 down
    public IEnumerator RotateArmEnum(float degrees, float speed)
    {
        while (transform.rotation.z < degrees)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, degrees), speed * armSpeed * Time.deltaTime);
            yield return null;
        }
        transform.rotation = Quaternion.Euler(0, 0, degrees);
        yield return null;
    }
}
