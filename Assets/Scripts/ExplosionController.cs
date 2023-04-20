using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls explosion behaviours
public class ExplosionController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyAfterAnimation());
    }

    // Destroys sprite after some time
    IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(0.8f);
        Destroy(this.gameObject);
    }
}
