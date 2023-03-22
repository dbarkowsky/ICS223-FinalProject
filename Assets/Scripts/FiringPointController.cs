using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FiringPattern
{
    SingleShot,
    TripleShot,
    TripleSpread
}

public class FiringPointController : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] FiringPattern pattern;
    public bool isActive = true;
    private bool canShoot = true; // used to handle cooldown between shots
    [SerializeField] float cooldown = 0.125f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fire()
    {
        switch (pattern)
        {
            case FiringPattern.SingleShot:
                StartCoroutine(SingleShot());
                break;
            case FiringPattern.TripleShot:
                StartCoroutine(TripleShot());
                break;
            case FiringPattern.TripleSpread:
                StartCoroutine(TripleSpread());
                break;  
            default:
                break;
        }
    }

    public bool pointCanShoot()
    {
        return canShoot && isActive;
    }

    // Firing Patterns Go Here
    private IEnumerator SingleShot()
    {
        canShoot = false;
        Vector3 pos = transform.position;
        Quaternion rotation = transform.rotation;
        Instantiate(bullet, pos, rotation, this.transform);
        yield return new WaitForSecondsRealtime(cooldown);
        canShoot = true;
    }

    private IEnumerator TripleShot()
    {
        canShoot = false;
        Vector3 pos = transform.position;
        Quaternion rotation = transform.rotation;
        float bulletOffset = 0.5f;
        Instantiate(bullet, pos, rotation);
        Instantiate(bullet, pos + new Vector3(bulletOffset, 0, 0), rotation, this.transform);
        Instantiate(bullet, pos - new Vector3(bulletOffset, 0, 0), rotation, this.transform);
        yield return new WaitForSecondsRealtime(cooldown);
        canShoot = true;
    }

    private IEnumerator TripleSpread()
    {
        float spread = 35f;
        canShoot = false;
        Vector3 pos = transform.position;
        Quaternion rotation = transform.rotation;
        // Have to make separate instances here, or it functions like a shallow copy
        GameObject bullet1 = Instantiate(bullet, pos, rotation, this.transform);
        bullet1.GetComponent<BulletController>().SetAngle(0);
        GameObject bullet2 = Instantiate(bullet, pos, rotation, this.transform);
        bullet2.GetComponent<BulletController>().SetAngle(spread);
        GameObject bullet3 = Instantiate(bullet, pos, rotation, this.transform);
        bullet3.GetComponent<BulletController>().SetAngle(360-spread);
        yield return new WaitForSecondsRealtime(cooldown);
        canShoot = true;
    }
}
