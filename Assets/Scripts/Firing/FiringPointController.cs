using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FiringPattern
{
    SingleShot,
    TripleShot,
    TripleSpread,
    VSpread,
    Spiral,
    DoubleSpiral,
    FullScreenSpread,
    Burst,
    SingleShotAtPlayer,
    BurstAtPlayer,
    Pulse,
    Focus,
    CrabMouthSpray,
    CrabMouthSprinkler,
    Laser
}

public class FiringPointController : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] FiringPattern pattern;
    public bool isActive = true;
    private bool canShoot = true; // used to handle cooldown between shots
    [SerializeField] float cooldown = 0.125f;
    [SerializeField] int repetitions = 1;
    private Vector2 playerPosition;
    private float laserAngle = 0f;


    private void Awake()
    {
        Messenger<Vector2>.AddListener(GameEvent.PLAYER_LOCATION, OnPlayerLocationReceived);
    }

    private void OnDestroy()
    {
        Messenger<Vector2>.RemoveListener(GameEvent.PLAYER_LOCATION, OnPlayerLocationReceived);
    }

    private void OnPlayerLocationReceived(Vector2 playerPosition)
    {
        this.playerPosition = playerPosition;
    }

    public void SetPattern(FiringPattern newPattern)
    {
        pattern = newPattern;
    }

    public void SetRepetitions(int repetitions)
    {
        this.repetitions = repetitions;
    }

    public void SetLaserAngle(float angle)
    {
        laserAngle = ConvertRadiansToDegrees(angle);
    }

    public void AdjustCooldown(float delta)
    {
        cooldown += delta;
        if (cooldown < 0.1f)
        {
            cooldown = 0.1f;
        }
    }

    public void Fire()
    {
        if (isActive && canShoot)
        {
            switch (pattern)
            {
                case FiringPattern.SingleShot:
                    StartCoroutine(SingleShot());
                    break;
                case FiringPattern.TripleShot:
                    StartCoroutine(TripleShot());
                    break;
                case FiringPattern.Focus:
                    StartCoroutine(Focus());
                    break;
                case FiringPattern.TripleSpread:
                    StartCoroutine(TripleSpread());
                    break;
                case FiringPattern.VSpread:
                    StartCoroutine(VSpread());
                    break;
                case FiringPattern.Spiral:
                    StartCoroutine(Spiral());
                    break;
                case FiringPattern.DoubleSpiral:
                    StartCoroutine(DoubleSpiral());
                    break;
                case FiringPattern.FullScreenSpread:
                    StartCoroutine(FullScreenSpread());
                    break;
                case FiringPattern.Burst:
                    StartCoroutine(Burst());
                    break;
                case FiringPattern.SingleShotAtPlayer:
                    StartCoroutine(SingleShotAtPlayer());
                    break;
                case FiringPattern.BurstAtPlayer:
                    StartCoroutine(BurstAtPlayer());
                    break;
                case FiringPattern.Pulse:
                    StartCoroutine(Pulse());
                    break;
                case FiringPattern.CrabMouthSpray:
                    StartCoroutine(CrabMouthSpray());
                    break;
                case FiringPattern.CrabMouthSprinkler:
                    StartCoroutine(CrabMouthSprinkler());
                    break;
                case FiringPattern.Laser:
                    StartCoroutine(Laser());
                    break;
                default:
                    break;
            }
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
        Instantiate(bullet, pos, rotation);
        yield return new WaitForSecondsRealtime(cooldown);
        canShoot = true;
    }

    private IEnumerator Focus()
    {
        canShoot = false;
        Vector3 pos = transform.position;
        Quaternion rotation = transform.rotation;
        float bulletOffset = 0.25f;
        Instantiate(bullet, pos + new Vector3(bulletOffset, 0, 0), rotation);
        Instantiate(bullet, pos - new Vector3(bulletOffset, 0, 0), rotation);
        yield return new WaitForSecondsRealtime(cooldown / 2f);
        canShoot = true;
    }

    private IEnumerator Laser()
    {
        Debug.Log(laserAngle);
        canShoot = false;
        Vector3 pos = transform.position;
        Quaternion rotation = transform.rotation;
        GameObject newBullet = Instantiate(bullet, pos, rotation);
        newBullet.GetComponent<BulletController>().SetAngle(laserAngle - 290);
        yield return new WaitForSecondsRealtime(cooldown / 2f);
        canShoot = true;
    }

    private IEnumerator TripleShot()
    {
        canShoot = false;
        Vector3 pos = transform.position;
        Quaternion rotation = transform.rotation;
        float bulletOffset = 0.5f;
        Instantiate(bullet, pos, rotation);
        Instantiate(bullet, pos + new Vector3(bulletOffset, 0, 0), rotation);
        Instantiate(bullet, pos - new Vector3(bulletOffset, 0, 0), rotation);
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
        GameObject bullet1 = Instantiate(bullet, pos, rotation);
        bullet1.GetComponent<BulletController>().SetAngle(0);
        rotation.z += 2.5f;
        GameObject bullet2 = Instantiate(bullet, pos, rotation);
        bullet2.GetComponent<BulletController>().SetAngle(spread);
        rotation.z -= 5f;
        GameObject bullet3 = Instantiate(bullet, pos, rotation);
        bullet3.GetComponent<BulletController>().SetAngle(360-spread);
        yield return new WaitForSecondsRealtime(cooldown);
        canShoot = true;
    }

    private IEnumerator VSpread()
    {
        float spread = 35f;
        canShoot = false;
        Vector3 pos = transform.position;
        Quaternion rotation = transform.rotation;
        // Have to make separate instances here, or it functions like a shallow copy
        GameObject bullet2 = Instantiate(bullet, pos, rotation);
        bullet2.GetComponent<BulletController>().SetAngle(spread);
        GameObject bullet3 = Instantiate(bullet, pos, rotation);
        bullet3.GetComponent<BulletController>().SetAngle(360 - spread);
        yield return new WaitForSecondsRealtime(cooldown);
        canShoot = true;
    }

    private IEnumerator Spiral()
    {
        canShoot = false;
        float secondsBetweenBullets = 0.125f;
        int projectileSpreadDegrees = 25;
        int projectilesPerRotation = 360 / projectileSpreadDegrees;
        for (int round = 0; round < repetitions; round++)
        {
            for (int projectile = 0; projectile < projectilesPerRotation; projectile++)
            {
                Vector3 pos = transform.position;
                Quaternion rotation = transform.rotation;
                GameObject newBullet = Instantiate(bullet, pos, rotation);
                newBullet.GetComponent<BulletController>().SetAngle(projectile * projectileSpreadDegrees);
                yield return new WaitForSecondsRealtime(secondsBetweenBullets);
            }
        }
        yield return new WaitForSecondsRealtime(cooldown); 
        canShoot = true;
    }

    private IEnumerator DoubleSpiral()
    {
        canShoot = false;
        float secondsBetweenBullets = 0.125f;
        int projectileSpreadDegrees = 30;
        int projectilesPerRotation = 360 / projectileSpreadDegrees;
        for (int round = 0; round < repetitions; round++)
        {
            for (int projectile = 0; projectile < projectilesPerRotation; projectile++)
            {
                Vector3 pos = transform.position;
                Quaternion rotation = transform.rotation;
                GameObject bullet1 = Instantiate(bullet, pos, rotation);
                bullet1.GetComponent<BulletController>().SetAngle((projectile * projectileSpreadDegrees) % 360);
                GameObject bullet2 = Instantiate(bullet, pos, rotation);
                bullet2.GetComponent<BulletController>().SetAngle(((projectile * projectileSpreadDegrees) + 180) % 360);
                yield return new WaitForSecondsRealtime(secondsBetweenBullets);
            }
        }
        yield return new WaitForSecondsRealtime(cooldown);
        canShoot = true;
    }

    private IEnumerator FullScreenSpread()
    {
        canShoot = false;
        float secondsBetweenBullets = 0.5f;
        int projectileSpreadDegrees = 60;
        for (int round = 0; round < repetitions; round++)
        {
            Vector3 pos = transform.position;
            Quaternion rotation = transform.rotation;
            GameObject bullet1 = Instantiate(bullet, pos, rotation);
            bullet1.GetComponent<BulletController>().SetAngle((round * projectileSpreadDegrees) % 360);
            GameObject bullet2 = Instantiate(bullet, pos, rotation);
            bullet2.GetComponent<BulletController>().SetAngle(((round * projectileSpreadDegrees) + 180) % 360);
            yield return new WaitForSecondsRealtime(secondsBetweenBullets);
        }
        yield return new WaitForSecondsRealtime(cooldown);
        canShoot = true;
    }

    private IEnumerator Pulse()
    {
        canShoot = false;
        int projectileSpreadDegrees = 25;
        int projectilesPerRotation = 360 / projectileSpreadDegrees;
        for (int round = 0; round < repetitions; round++)
        {
            for (int projectile = 0; projectile < projectilesPerRotation; projectile++)
            {
                Vector3 pos = transform.position;
                Quaternion rotation = transform.rotation;
                GameObject newBullet = Instantiate(bullet, pos, rotation);
                newBullet.GetComponent<BulletController>().SetAngle(projectile * projectileSpreadDegrees);
            }
        }
        yield return new WaitForSecondsRealtime(cooldown);
        canShoot = true;
    }

    private IEnumerator Burst()
    {
        canShoot = false;
        float secondsBetweenBullets = 0.1f;
        for (int round = 0; round < repetitions; round++)
        {
            Vector3 pos = transform.position;
            Quaternion rotation = transform.rotation;
            GameObject bullet1 = Instantiate(bullet, pos, rotation);
            bullet1.GetComponent<BulletController>().SetAngle(0);
            yield return new WaitForSecondsRealtime(secondsBetweenBullets);
        }
        yield return new WaitForSecondsRealtime(cooldown);
        canShoot = true;
    }

    private IEnumerator SingleShotAtPlayer()
    {
        canShoot = false;
        Vector3 pos = transform.position;
        Quaternion rotation = transform.rotation;
        Vector2 points2DPosition = new Vector2(pos.x, pos.y);
        GameObject bullet1 = Instantiate(bullet, pos, rotation);
        bullet1.GetComponent<BulletController>().SetAngle(GetAngleToPlayer());
        yield return new WaitForSecondsRealtime(cooldown);
        canShoot = true;
    }

    private IEnumerator BurstAtPlayer()
    {
        canShoot = false;
        float secondsBetweenBullets = 0.1f;
        for (int round = 0; round < repetitions; round++)
        {
            Vector3 pos = transform.position;
            Quaternion rotation = transform.rotation;
            GameObject bullet1 = Instantiate(bullet, pos, rotation);
            bullet1.GetComponent<BulletController>().SetAngle(GetAngleToPlayer());
            yield return new WaitForSecondsRealtime(secondsBetweenBullets);
        }
        yield return new WaitForSecondsRealtime(cooldown);
        canShoot = true;
    }

    private IEnumerator CrabMouthSpray()
    {
        canShoot = false;
        float secondsBetweenBullets = 0.07f;
        int projectileSpreadDegrees = 20;
        int projectilesPerRotation = 360 / projectileSpreadDegrees;
        for (int round = 0; round < repetitions; round++)
        {
            for (int projectile = 0; projectile < projectilesPerRotation; projectile++)
            {
                Vector3 pos = transform.position;
                Quaternion rotation = transform.rotation;
                GameObject bullet1 = Instantiate(bullet, pos, rotation);
                bullet1.GetComponent<BulletController>().SetAngle((projectile * projectileSpreadDegrees) % 360);
                GameObject bullet2 = Instantiate(bullet, pos, rotation);
                bullet2.GetComponent<BulletController>().SetAngle(((projectile * projectileSpreadDegrees) + 180) % 360);
                yield return new WaitForSecondsRealtime(secondsBetweenBullets);
            }
        }
        yield return new WaitForSecondsRealtime(cooldown);
        canShoot = true;
    }

    private IEnumerator CrabMouthSprinkler()
    {
        canShoot = false;
        float secondsBetweenBullets;
        int projectileSpreadDegrees = 7;
        int angleOfSpray = 120;
        int projectilesWithinAngle = angleOfSpray / projectileSpreadDegrees;
        int angleOffset = 60;
        for (int round = 0; round < repetitions; round++)
        {
            secondsBetweenBullets = 0.2f;
            for (int projectile = 0; projectile < projectilesWithinAngle; projectile++)
            {
                Vector3 pos = transform.position;
                Quaternion rotation = transform.rotation;
                GameObject bullet1 = Instantiate(bullet, pos, rotation);
                float angle = (angleOfSpray - angleOffset - (projectile * projectileSpreadDegrees));
                if (angle <= 0) { angle = 359 - Mathf.Abs(angle); }
                bullet1.GetComponent<BulletController>().SetAngle(angle);
                yield return new WaitForSecondsRealtime(secondsBetweenBullets);
            }
            secondsBetweenBullets = 0.07f;
            for (int projectile = projectilesWithinAngle; projectile >= 0; projectile--)
            {
                Vector3 pos = transform.position;
                Quaternion rotation = transform.rotation;
                GameObject bullet1 = Instantiate(bullet, pos, rotation);
                float angle = (angleOfSpray - angleOffset - (projectile * projectileSpreadDegrees));
                if (angle <= 0) { angle = 359 - Mathf.Abs(angle); }
                bullet1.GetComponent<BulletController>().SetAngle(angle);
                yield return new WaitForSecondsRealtime(secondsBetweenBullets);
            }
        }
        yield return new WaitForSecondsRealtime(cooldown);
        canShoot = true;
    }

    private float GetAngleToPlayer()
    {
        float absoluteAngle;
        float near;
        float far;
        float xDifference = playerPosition.x - transform.position.x;
        float yDifference = playerPosition.y - transform.position.y;
        // First 4 directions, then in between
        if (xDifference == 0 && yDifference > 0)
        {
            // straight forward, 0 degrees
            absoluteAngle = 0f;
        } else if (xDifference == 0 && yDifference < 0)
        {
            // straight back, 180 degress
            absoluteAngle = 180f;
        } else if (yDifference == 0 && xDifference < 0)
        {
            // straight left, 270 degrees
            absoluteAngle = 270f;
        }
        else if (yDifference == 0 && xDifference > 0)
        {
            // straight right, 90 degrees
            absoluteAngle = 90f;
        }
        // In between directions
        else if (xDifference > 0 && yDifference > 0)
        {
            // between 0 and 90 degrees
            far = Mathf.Abs(xDifference);
            near = Mathf.Abs(yDifference);
            absoluteAngle = ConvertRadiansToDegrees(Mathf.Atan2(far, near));
        } else if (xDifference > 0 && yDifference < 0)
        {
            // between 90 and 180 degrees
            far = Mathf.Abs(yDifference);
            near = Mathf.Abs(xDifference);
            absoluteAngle = ConvertRadiansToDegrees(Mathf.Atan2(far, near)) + 90;
        } else if (xDifference < 0 && yDifference < 0)
        {
            // between 180 and 270 degrees
            far = Mathf.Abs(xDifference);
            near = Mathf.Abs(yDifference);
            absoluteAngle = ConvertRadiansToDegrees(Mathf.Atan2(far, near)) + 180;
        } else
        {
            // between 270 and 360 degrees
            far = Mathf.Abs(yDifference);
            near = Mathf.Abs(xDifference);
            absoluteAngle = ConvertRadiansToDegrees(Mathf.Atan2(far, near)) + 270;
        }
        return (absoluteAngle + 180) % 360; // because otherwise it fires the wrong way...
    }

    private float ConvertRadiansToDegrees(float radians)
    {
        // Angle in Radians × 180°/ PI
        return (radians * (180 / Mathf.PI));
    }
}
