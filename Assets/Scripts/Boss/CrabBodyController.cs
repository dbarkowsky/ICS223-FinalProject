using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabBodyController : MonoBehaviour
{
    [SerializeField] int hp = 300;
    [SerializeField] FiringPointController firingPoint;
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
        firingPoint.Fire();
    }

    public void SetFiringPattern(FiringPattern pattern)
    {
        firingPoint.SetPattern(pattern);
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
                Messenger<GameObject>.Broadcast(GameEvent.ENEMY_DESTROYED, this.gameObject);
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
