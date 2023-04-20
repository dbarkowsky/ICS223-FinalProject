using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickupType
{
    BulletFocus,
    BulletSpread
}

// Controls the behaviour of pickup items
public class PickupController : MonoBehaviour
{
    [SerializeField] Sprite secondarySprite;
    private Sprite primarySprite;
    private int state; // 0 = primary, 1 = secondary
    [SerializeField] private PickupType type;

    // Start flashing
    void Start()
    {
        primarySprite = GetComponent<SpriteRenderer>().sprite;
        StartCoroutine(Flash());
    }

    // Animates the pickup by alternating between two sprites
    // Could have used an animation for this, but...  
    IEnumerator Flash()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            switch (state)
            {
                case 0:
                    state = 1;
                    GetComponent<SpriteRenderer>().sprite = secondarySprite;
                    break;
                case 1:
                    state = 0;
                    GetComponent<SpriteRenderer>().sprite = primarySprite;
                    break;
                default:
                    break;
            }
        }
    }

    // When the player collides 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Messenger<PickupController>.Broadcast(GameEvent.PICKUP_TOUCHED, this);
            Destroy(this.gameObject);
        }
    }

    // What type is it?
    public PickupType GetPickupType()
    {
        return type;
    }
}
