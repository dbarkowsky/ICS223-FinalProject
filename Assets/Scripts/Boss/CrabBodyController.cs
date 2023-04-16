using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabBodyController : MonoBehaviour
{
    [SerializeField] public int hp = 300;
    [SerializeField] FiringPointController firingPoint;
    [SerializeField] Animator anim;
    public float animationTime = 4f;
    public bool canShoot = true;
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
        anim.SetBool("mouthFiring", true);
        yield return new WaitForSecondsRealtime(1f);  
        if (canShoot) firingPoint.Fire();
        yield return new WaitForSecondsRealtime(animationTime);
        anim.SetBool("mouthFiring", false);
    }

    public void SetFiringPattern(FiringPattern pattern)
    {
        firingPoint.SetPattern(pattern);
    }

    public void SetFiringRepetitions(int repetitions)
    {
        firingPoint.SetRepetitions(repetitions);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            Destroy(other.gameObject);
            hp--;
            StartCoroutine(StrobeOnHit());
            if (hp <= 0)
            {
                Messenger.Broadcast(GameEvent.CRAB_DESTROYED);
                Debug.Log("Crab dead.");
            }
        }
        if (other.CompareTag("Player"))
        {
            Messenger.Broadcast(GameEvent.PLAYER_DEAD);
        }
    }

    IEnumerator StrobeOnHit()
    {
        SpriteRenderer sprite = gameObject.GetComponentInChildren<SpriteRenderer>();
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.5f);
        yield return new WaitForSecondsRealtime(0.1f);
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f);
    }
}
