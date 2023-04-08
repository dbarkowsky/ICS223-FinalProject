using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickupType
{
    BulletFocus,
    BulletSpread
}

public class PickupController : MonoBehaviour
{
    [SerializeField] Sprite secondarySprite;
    private Sprite primarySprite;
    private int state; // 0 = primary, 1 = secondary
    // Start is called before the first frame update
    [SerializeField] private PickupType type;
    void Start()
    {
        primarySprite = GetComponent<SpriteRenderer>().sprite;
        StartCoroutine(Flash());
    }

    IEnumerator Flash()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(0.5f);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Messenger<PickupController>.Broadcast(GameEvent.PICKUP_TOUCHED, this);
        }
        Destroy(this.gameObject);
    }

    public PickupType GetPickupType()
    {
        return type;
    }
}
