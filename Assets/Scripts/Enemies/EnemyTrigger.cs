using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TriggerType
{
    SingleOrca,
    SingleGoldfishTop,
    SingleGoldfishLeft,
    SingleGoldfishRight,
    SingleJellyfish,
    SingleOctopus,
    SingleManta,
    SingleAnenome,
    SingleBarnacle,
    SingleStingRay,
    SingleTurtle,
    SingleNarwhal,
    TwoMantas,
    ThreeGoldfishTop,
    GoldfishWave
}
public class EnemyTrigger : MonoBehaviour
{
    [SerializeField] TriggerType triggerType = 0;
    [SerializeField] int repetitions = 1;
    [SerializeField] float cooldown = 1f;
    private bool triggered = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!triggered)
        {
            if (collision.CompareTag("EnemyTriggerer"))
            {
                triggered = true;
                StartCoroutine(AnnounceTrigger());
            }
        }
    }

    IEnumerator AnnounceTrigger()
    {
        for (int round = 0; round < repetitions; round++)
        {
            Messenger<TriggerType>.Broadcast(GameEvent.ENEMY_TRIGGER_REACHED, triggerType);
            yield return new WaitForSecondsRealtime(cooldown);
        }
        Destroy(this.gameObject);
    }
}
