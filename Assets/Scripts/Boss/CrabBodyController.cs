using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Controls only the body portion of the crab
public class CrabBodyController : MonoBehaviour
{
    [SerializeField] public int hp = 300;
    [SerializeField] FiringPointController firingPoint;
    [SerializeField] Animator anim;
    public float animationTime = 4f;
    public bool canShoot = true;

    // Fires the mouth firing point
    public IEnumerator Fire()
    {
        anim.SetBool("mouthFiring", true);
        Messenger.Broadcast(GameEvent.BOSS_YELL);
        yield return new WaitForSeconds(1f);  
        if (canShoot) firingPoint.Fire();
        yield return new WaitForSeconds(animationTime);
        anim.SetBool("mouthFiring", false);
    }

    // Sets the firing pattern of the mouth
    public void SetFiringPattern(FiringPattern pattern)
    {
        firingPoint.SetPattern(pattern);
    }

    // Sets the number of repetitions for each firing phase
    public void SetFiringRepetitions(int repetitions)
    {
        firingPoint.SetRepetitions(repetitions);
    }

    // Handles collisions with other game objects
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Reduce hp if object is a player bullet
        if (other.CompareTag("PlayerBullet"))
        {
            Destroy(other.gameObject);
            hp--;
            StartCoroutine(StrobeOnHit());
            // Kill if at 0
            if (hp <= 0)
            {
                Messenger.Broadcast(GameEvent.CRAB_DESTROYED);
            }
        }
        // Kill the player if they collide
        if (other.CompareTag("Player"))
        {
            Messenger.Broadcast(GameEvent.PLAYER_DEAD);
        }
    }

    // Flash the sprite to indicate a hit was made
    IEnumerator StrobeOnHit()
    {
        SpriteRenderer sprite = gameObject.GetComponentInChildren<SpriteRenderer>();
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.5f);
        yield return new WaitForSeconds(0.1f);
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f);
    }
}
