using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiringPointController : MonoBehaviour
{
    [SerializeField] GameObject bullet;

    public bool canShoot = true; // used to handle cooldown between shots
    public float cooldown = 0.125f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Fire()
    {
        Vector3 pos = transform.position;
        Quaternion rotation = transform.rotation;
        Instantiate(bullet, pos, rotation);
        yield return new WaitForSecondsRealtime(cooldown);
        canShoot = true;
    }
}
