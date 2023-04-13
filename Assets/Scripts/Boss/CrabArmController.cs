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

    // Start is called before the first frame update
    void Start()
    {
        //RotateArm(90f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
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

    IEnumerator RotateArmEnum(float degrees, float speed)
    {
        while (transform.rotation.z < degrees)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, degrees), speed * armSpeed * Time.deltaTime);
            yield return null;
        }
        transform.rotation = Quaternion.Euler(0, 0, degrees);
        yield return null;
    }

    // 0 is right, 90 is up, 180 is left, 270 down
    //IEnumerator RotateArm(Vector3 rotation, float time)
    //{
    //    while (!Mathf.Approximately(transform.rotation.z, rotation.z))
    //    {
    //        transform.rotation = Quaternion.Euler(Vector3.Lerp(transform.rotation.eulerAngles, rotation, time * Time.deltaTime));
    //        yield return null;
    //    }
    //}
}
