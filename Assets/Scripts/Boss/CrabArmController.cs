using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabArmController : MonoBehaviour
{
    private enum ClawStates { 
        Protect,
        FollowPlayer,
        Freeze,
        Open
    }

    [SerializeField] private float armSpeed = 0.1f;
    [SerializeField] private GameObject laser;
    [SerializeField] private GameObject laserWarning;
    public bool canShoot = true;


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

    private IEnumerator FireLaser()
    {
        canShoot = false;
        laserWarning.SetActive(true);
        yield return new WaitForSecondsRealtime(0.5f);
        laser.SetActive(true);
        laserWarning.SetActive(false);
        yield return new WaitForSecondsRealtime(1f);
        laser.SetActive(false);
        canShoot = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Player"))
        {
            Messenger.Broadcast(GameEvent.PLAYER_DEAD);
        }
    }

    public void RotateArm(float degrees, float speed)
    {
        StartCoroutine(RotateArmEnum(degrees, speed));
        
    }
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
