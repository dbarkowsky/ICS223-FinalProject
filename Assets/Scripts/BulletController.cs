using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private float speed = 30f;
    private float rotateSpeed = 360 * 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * speed * Time.deltaTime;
        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);// Quaternion.Euler(Vector3.forward * rotateSpeed * Time.deltaTime);
        StartCoroutine(DestroyMe());
    }

    IEnumerator DestroyMe()
    {
        yield return new WaitForSecondsRealtime(Constants.bulletLifeTime);
        Destroy(gameObject);
    }
}
