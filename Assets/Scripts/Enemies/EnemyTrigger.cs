using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Declare all types of triggers
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

// Controls the enemy trigger objects, which trigger enemy spawns
public class EnemyTrigger : MonoBehaviour
{
    [SerializeField] TriggerType triggerType = 0;
    [SerializeField] int repetitions = 1;
    [SerializeField] float cooldown = 1f;
    private bool triggered = false;

    // When a collision is detected
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Only do it once...
        if (!triggered)
        {
            // If it's the trigger-er
            if (collision.CompareTag("EnemyTriggerer"))
            {
                triggered = true;
                StartCoroutine(AnnounceTrigger());
            }
        }
    }

    // Broadcasts the trigger event with the TriggerType. Repeats "repetitions" number of times
    IEnumerator AnnounceTrigger()
    {
        for (int round = 0; round < repetitions; round++)
        {
            Messenger<TriggerType>.Broadcast(GameEvent.ENEMY_TRIGGER_REACHED, triggerType);
            yield return new WaitForSeconds(cooldown);
        }
        Destroy(this.gameObject);
    }
}
