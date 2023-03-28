using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    [SerializeField] int triggerID = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyTriggerer"))
        {
            Messenger<int>.Broadcast(GameEvent.ENEMY_TRIGGER_REACHED, triggerID);
            Destroy(this.gameObject);
        }
    }
}
