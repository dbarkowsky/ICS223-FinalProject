using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GenericTrigger
{
    StartBossMusic,
    ArriveAtBossArea
}
public class GenericTriggerController : MonoBehaviour
{
    [SerializeField] GenericTrigger triggerType = 0;
    private bool triggered = false;
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
        }
        Destroy(this.gameObject);
    }
}
