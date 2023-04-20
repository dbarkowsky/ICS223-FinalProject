using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Types of triggers
public enum GenericTrigger
{
    StartBossMusic,
    ArriveAtBossArea,
    PickupNotification
}

// Controls the generic trigger objects
public class GenericTriggerController : MonoBehaviour
{
    [SerializeField] GenericTrigger triggerType = 0;
    private bool triggered = false;

    // When collision with the trigger-er, announce the event
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!triggered)
        {
            if (collision.CompareTag("EnemyTriggerer"))
            {
                triggered = true;
                AnnounceTrigger();
            }
        }
    }

    // Announces the event/trigger type and destroys itself
    void AnnounceTrigger()
    {
        switch (triggerType)
        {
            case GenericTrigger.StartBossMusic:
                Messenger.Broadcast(GameEvent.START_BOSS_MUSIC);
                break;
            case GenericTrigger.ArriveAtBossArea:
                Messenger.Broadcast(GameEvent.STOP_CAMERA);
                Messenger.Broadcast(GameEvent.START_BOSS_BATTLE);
                break;
            case GenericTrigger.PickupNotification:
                Messenger.Broadcast(GameEvent.PICKUP_NOTIFICATION);
                break;
        }
        Destroy(this.gameObject);
    }
}
