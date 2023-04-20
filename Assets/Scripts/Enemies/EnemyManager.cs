using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages spawning of enemies, their states, and their deaths
public class EnemyManager : MonoBehaviour
{
    // Declaration of possible enemy types
    private enum EnemyPrefabs
    {
        GoldfishDownUp,
        GoldfishLeft,
        GoldfishRight,
        Barnacle,
        Anenome,
        Octopus,
        Jellyfish,
        StingRay,
        Manta,
        Narwhal,
        Orca,
        Turtle
    }

    private List<GameObject> enemies = new List<GameObject>(); // spawned enemies stored here
    [SerializeField] private List<GameObject> spawnPoints;
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private GameObject explosion;
    [SerializeField] private GameObject crater;
    [SerializeField] private CrabController crab;

    [SerializeField] Camera cam; // To determine if enemies are on screen

    // For all spawned enemies, check if they are on screen still (can fire) or below the screen (should be removed)
    void Update()
    {
        List<GameObject> enemiesCopy = new List<GameObject>(enemies); // copy to avoid changing list while iterating through it
        enemiesCopy.ForEach(delegate (GameObject enemy)
        {
            enemy.GetComponent<EnemyController>().AmIOnScreen(cam);
            if (enemy.GetComponent<EnemyController>().AmIBelowScreen(cam))
            {
                DestroyEnemy(enemy);
            }
        });
    }

    private void Awake()
    {
        Messenger<GameObject>.AddListener(GameEvent.ENEMY_DESTROYED, OnEnemyDestroyed);
        Messenger<GameObject>.AddListener(GameEvent.ENEMY_DESTROYED_SELF, OnEnemyDestroyed);
        Messenger<TriggerType>.AddListener(GameEvent.ENEMY_TRIGGER_REACHED, OnEnemyTriggerReached);
        Messenger.AddListener(GameEvent.START_BOSS_BATTLE, OnStartBossBattle);
        Messenger.AddListener(GameEvent.START_BOSS_MUSIC, OnStartBossMusic);
        Messenger.AddListener(GameEvent.CRAB_DESTROYED, this.OnCrabDestroyed);
    }

    private void OnDestroy()
    {
        Messenger<GameObject>.RemoveListener(GameEvent.ENEMY_DESTROYED, OnEnemyDestroyed);
        Messenger<GameObject>.RemoveListener(GameEvent.ENEMY_DESTROYED_SELF, OnEnemyDestroyed);
        Messenger<TriggerType>.RemoveListener(GameEvent.ENEMY_TRIGGER_REACHED, OnEnemyTriggerReached);
        Messenger.RemoveListener(GameEvent.START_BOSS_BATTLE, OnStartBossBattle);
        Messenger.RemoveListener(GameEvent.START_BOSS_MUSIC, OnStartBossMusic);
        Messenger.RemoveListener(GameEvent.CRAB_DESTROYED, this.OnCrabDestroyed);

    }

    // Stop the crab, start the explosions
    private void OnCrabDestroyed()
    {
        crab.Fight(false); // stop attacking
        StartCoroutine(CrabExplode());
    }

    // Randomize some explosions around the crab, then one big explosion before deleting crab and telling game to proceed to score
    private IEnumerator CrabExplode()
    {
        GameObject exp;
        float explosionSize;
        for (int i = 0; i < 25; i++)
        {
            exp = Instantiate(explosion, crab.transform.position + new Vector3(Random.Range(-4, 4), Random.Range(-2, 2), crab.transform.position.z - 1f), crab.transform.rotation);
            explosionSize = Random.Range(1, 4);
            exp.transform.localScale = new Vector3(explosionSize, explosionSize, 1);
            Messenger.Broadcast(GameEvent.EXPLOSION);
            yield return new WaitForSeconds(0.25f);
        }
        // One big explosion
        exp = Instantiate(explosion, crab.transform.position - new Vector3(3.5f, 0, 0), crab.transform.rotation);
        explosionSize = crab.explosionSize;
        exp.transform.localScale = new Vector3(explosionSize, explosionSize, 1);
        Destroy(crab.gameObject);
        Messenger.Broadcast(GameEvent.EXPLOSION);
        Messenger.Broadcast(GameEvent.FADE_TO_SCORE);
    }

    // When an enemy trigger is reached, starts spawning coroutine based on triggerType
    private void OnEnemyTriggerReached(TriggerType triggerType)
    {
        switch (triggerType)
        {
            case TriggerType.SingleOrca:
                StartCoroutine(SingleOrca());
                break;
            case TriggerType.SingleGoldfishTop:
                StartCoroutine(SingleGoldfishTop());
                break;
            case TriggerType.SingleGoldfishLeft:
                StartCoroutine(SingleGoldfishLeft());
                break;
            case TriggerType.SingleGoldfishRight:
                StartCoroutine(SingleGoldfishRight());
                break;
            case TriggerType.SingleJellyfish:
                StartCoroutine(SingleJellyfish());
                break;
            case TriggerType.SingleOctopus:
                StartCoroutine(SingleOctopus());
                break;
            case TriggerType.SingleAnenome:
                StartCoroutine(SingleAnenome());
                break;
            case TriggerType.SingleBarnacle:
                StartCoroutine(SingleBarnacle());
                break;
            case TriggerType.SingleManta:
                StartCoroutine(SingleManta());
                break;
            case TriggerType.SingleStingRay:
                StartCoroutine(SingleStingRay());
                break;
            case TriggerType.SingleTurtle:
                StartCoroutine(SingleTurtle());
                break;
            case TriggerType.SingleNarwhal:
                StartCoroutine(SingleNarwhal());
                break;
            case TriggerType.TwoMantas:
                StartCoroutine(TwoMantas());
                break;
            case TriggerType.ThreeGoldfishTop:
                StartCoroutine(ThreeGoldfish());
                break;
            case TriggerType.GoldfishWave:
                StartCoroutine(GoldfishWave());
                break;
            default:
                break;
        }
    }

    // When an enemy is destroyed...
    private void OnEnemyDestroyed(GameObject enemy)
    {
        // Make sure it's still around
        if (enemy != null)
        {
            // Create an explosion
            GameObject exp = Instantiate(explosion, enemy.transform.position + enemy.transform.localScale / 2, enemy.transform.rotation);
            float explosionSize = enemy.GetComponent<EnemyController>().explosionSize;
            exp.transform.localScale = new Vector3(explosionSize, explosionSize, 1);
            
            // If it's a land enemy, leave a mark
            if (enemy.CompareTag("EnemyLand"))
            {
                GameObject burnMark = Instantiate(crater, enemy.transform.position + enemy.transform.localScale / 2, enemy.transform.rotation);
                float burnMarkSize = enemy.GetComponent<EnemyController>().explosionSize;
                burnMark.transform.localScale = new Vector3(burnMarkSize, burnMarkSize, 1);
            }
            // Destroy the object
            DestroyEnemy(enemy); 
        }
    }

    // Crab can now fight
    private void OnStartBossBattle()
    {
        crab.Fight(true);
    }

    // Turns on the hitboxes
    private void OnStartBossMusic()
    {
        crab.SetHitBoxes(true);
    }

    // Removes enemy from list and destroys object
    private void DestroyEnemy(GameObject enemy)
    {
        int index = enemies.IndexOf(enemy);
        if (index < enemies.Count && index >= 0) enemies.RemoveAt(index);
        Destroy(enemy);
    }

    // Enemy spawning scripts
    IEnumerator SingleOrca()
    {
        int spawnPoint;
        spawnPoint = Random.Range(3, 7);
        GameObject enemy = Instantiate(enemyPrefabs[(int)EnemyPrefabs.Orca], this.transform);
        enemy.transform.position = spawnPoints[spawnPoint].transform.position;
        enemies.Add(enemy);
        yield return null;
    }

    IEnumerator SingleJellyfish()
    {
        int spawnPoint;
        spawnPoint = Random.Range(3, 7);
        GameObject enemy = Instantiate(enemyPrefabs[(int)EnemyPrefabs.Jellyfish], this.transform);
        enemy.transform.position = spawnPoints[spawnPoint].transform.position;
        enemies.Add(enemy);
        yield return null;
    }

    IEnumerator SingleOctopus()
    {
        int spawnPoint;
        spawnPoint = Random.Range(3, 7);
        GameObject enemy = Instantiate(enemyPrefabs[(int)EnemyPrefabs.Octopus], this.transform);
        enemy.transform.position = spawnPoints[spawnPoint].transform.position;
        enemies.Add(enemy);
        yield return null;
    }

    IEnumerator SingleManta()
    {
        int spawnPoint;
        spawnPoint = Random.Range(3, 7);
        GameObject enemy = Instantiate(enemyPrefabs[(int)EnemyPrefabs.Manta], this.transform);
        enemy.transform.position = spawnPoints[spawnPoint].transform.position;
        enemies.Add(enemy);
        yield return null;
    }

    IEnumerator SingleStingRay()
    {
        int spawnPoint;
        spawnPoint = Random.Range(3, 7);
        GameObject enemy = Instantiate(enemyPrefabs[(int)EnemyPrefabs.StingRay], this.transform);
        enemy.transform.position = spawnPoints[spawnPoint].transform.position;
        enemies.Add(enemy);
        yield return null;
    }

    IEnumerator SingleTurtle()
    {
        int spawnPoint;
        spawnPoint = Random.Range(3, 7);
        GameObject enemy = Instantiate(enemyPrefabs[(int)EnemyPrefabs.Turtle], this.transform);
        enemy.transform.position = spawnPoints[spawnPoint].transform.position;
        enemies.Add(enemy);
        yield return null;
    }

    IEnumerator SingleNarwhal()
    {
        int spawnPoint;
        spawnPoint = Random.Range(3, 7);
        GameObject enemy = Instantiate(enemyPrefabs[(int)EnemyPrefabs.Narwhal], this.transform);
        enemy.transform.position = spawnPoints[spawnPoint].transform.position;
        enemies.Add(enemy);
        yield return null;
    }

    IEnumerator SingleGoldfishTop()
    {
        GameObject enemy = Instantiate(enemyPrefabs[(int)EnemyPrefabs.GoldfishDownUp], this.transform);
        enemy.transform.position = spawnPoints[Random.Range(3, 7)].transform.position;
        enemies.Add(enemy);
        yield return null;
    }

    IEnumerator SingleGoldfishLeft()
    {
        GameObject enemy = Instantiate(enemyPrefabs[(int)EnemyPrefabs.GoldfishLeft], this.transform);
        enemy.transform.position = spawnPoints[Random.Range(0, 3)].transform.position;
        enemies.Add(enemy);
        yield return null;
    }

    IEnumerator SingleGoldfishRight()
    {
        GameObject enemy = Instantiate(enemyPrefabs[(int)EnemyPrefabs.GoldfishRight], this.transform);
        enemy.transform.position = spawnPoints[Random.Range(7, 10)].transform.position;
        enemies.Add(enemy);
        yield return null;
    }

    IEnumerator SingleAnenome()
    {
        GameObject enemy = Instantiate(enemyPrefabs[(int)EnemyPrefabs.Anenome], this.transform);
        enemy.transform.position = spawnPoints[Random.Range(3, 7)].transform.position;
        enemies.Add(enemy);
        yield return null;
    }

    IEnumerator SingleBarnacle()
    {
        GameObject enemy = Instantiate(enemyPrefabs[(int)EnemyPrefabs.Barnacle], this.transform);
        enemy.transform.position = spawnPoints[Random.Range(3, 7)].transform.position;
        enemies.Add(enemy);
        yield return null;
    }

    IEnumerator TwoMantas()
    {
        GameObject enemy = Instantiate(enemyPrefabs[(int)EnemyPrefabs.Manta], this.transform);
        enemy.transform.position = spawnPoints[3].transform.position;
        enemies.Add(enemy);
        enemy = Instantiate(enemyPrefabs[(int)EnemyPrefabs.Manta], this.transform);
        enemy.transform.position = spawnPoints[5].transform.position;
        enemies.Add(enemy);
        yield return null;
    }

    IEnumerator ThreeGoldfish()
    {
        GameObject enemy = Instantiate(enemyPrefabs[(int)EnemyPrefabs.GoldfishDownUp], this.transform);
        enemy.transform.position = spawnPoints[3].transform.position;
        enemies.Add(enemy);
        yield return new WaitForSeconds(0.5f);
        enemy = Instantiate(enemyPrefabs[(int)EnemyPrefabs.GoldfishDownUp], this.transform);
        enemy.transform.position = spawnPoints[6].transform.position;
        enemies.Add(enemy);
        yield return new WaitForSeconds(0.5f);
        enemy = Instantiate(enemyPrefabs[(int)EnemyPrefabs.GoldfishDownUp], this.transform);
        enemy.transform.position = spawnPoints[Random.Range(4, 6)].transform.position;
        enemies.Add(enemy);
    }

    IEnumerator GoldfishWave()
    {
        GameObject enemy;
        int firstLeftSpawn = 2;
        int firstRightSpawn = 7;
        for (int i = 0; i < 3; i++)
        {
            enemy = Instantiate(enemyPrefabs[(int)EnemyPrefabs.GoldfishLeft], this.transform);
            enemy.transform.position = spawnPoints[firstLeftSpawn - i].transform.position;
            enemies.Add(enemy);
            enemy = Instantiate(enemyPrefabs[(int)EnemyPrefabs.GoldfishRight], this.transform);
            enemy.transform.position = spawnPoints[firstRightSpawn + i].transform.position;
            enemies.Add(enemy);
            yield return new WaitForSeconds(0.7f);
        }
    }
}
