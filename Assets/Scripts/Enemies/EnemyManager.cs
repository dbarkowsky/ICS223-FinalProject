using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
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
    private List<GameObject> enemies = new List<GameObject>();
    [SerializeField] private List<GameObject> spawnPoints;
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private GameObject explosion;
    [SerializeField] private GameObject crater;
    [SerializeField] private CrabController crab;

    [SerializeField] Camera cam;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
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
    }

    private void OnDestroy()
    {
        Messenger<GameObject>.RemoveListener(GameEvent.ENEMY_DESTROYED, OnEnemyDestroyed);
        Messenger<GameObject>.RemoveListener(GameEvent.ENEMY_DESTROYED_SELF, OnEnemyDestroyed);
        Messenger<TriggerType>.RemoveListener(GameEvent.ENEMY_TRIGGER_REACHED, OnEnemyTriggerReached);
        Messenger.RemoveListener(GameEvent.START_BOSS_BATTLE, OnStartBossBattle);
    }

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

    private void OnEnemyDestroyed(GameObject enemy)
    {
        if (enemy != null)
        {
            Debug.Log(this + " Enemy destroyed.");
            GameObject exp = Instantiate(explosion, enemy.transform.position + enemy.transform.localScale / 2, enemy.transform.rotation);
            float explosionSize = enemy.GetComponent<EnemyController>().explosionSize;
            exp.transform.localScale = new Vector3(explosionSize, explosionSize, 1);
            
            if (enemy.CompareTag("EnemyLand"))
            {
                GameObject burnMark = Instantiate(crater, enemy.transform.position + enemy.transform.localScale / 2, enemy.transform.rotation);
                float burnMarkSize = enemy.GetComponent<EnemyController>().explosionSize;
                burnMark.transform.localScale = new Vector3(burnMarkSize, burnMarkSize, 1);
            }
            DestroyEnemy(enemy); 
        }
    }

    private void OnStartBossBattle()
    {
        crab.Fight(true);
    }

    private void DestroyEnemy(GameObject enemy)
    {
        enemies.RemoveAt(enemies.IndexOf(enemy));
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
        yield return new WaitForSecondsRealtime(0.5f);
        enemy = Instantiate(enemyPrefabs[(int)EnemyPrefabs.GoldfishDownUp], this.transform);
        enemy.transform.position = spawnPoints[6].transform.position;
        enemies.Add(enemy);
        yield return new WaitForSecondsRealtime(0.5f);
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
            yield return new WaitForSecondsRealtime(0.7f);
        }
    }
}
