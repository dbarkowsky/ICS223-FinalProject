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
        Messenger<GameObject>.AddListener(GameEvent.ENEMY_DESTROYED_SELF, OnEnemyDestroyedSelf);
        Messenger<TriggerType>.AddListener(GameEvent.ENEMY_TRIGGER_REACHED, OnEnemyTriggerReached);
    }

    private void OnDestroy()
    {
        Messenger<GameObject>.RemoveListener(GameEvent.ENEMY_DESTROYED, OnEnemyDestroyed);
        Messenger<GameObject>.RemoveListener(GameEvent.ENEMY_DESTROYED_SELF, OnEnemyDestroyedSelf);
        Messenger<TriggerType>.RemoveListener(GameEvent.ENEMY_TRIGGER_REACHED, OnEnemyTriggerReached);
    }

    private void OnEnemyTriggerReached(TriggerType triggerType)
    {
        switch (triggerType)
        {
            case TriggerType.SingleOrca:
                StartCoroutine(SingleOrca());
                break;
            case TriggerType.TwoMantas:
                StartCoroutine(TwoMantas());
                break;
            case TriggerType.ThreeGoldfishTop:
                StartCoroutine(ThreeGoldfish());
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
            DestroyEnemy(enemy);
        }
    }

    private void OnEnemyDestroyedSelf(GameObject enemy)
    {
        if (enemy != null)
        {
            Debug.Log(this + " Enemy destroyed self.");
            DestroyEnemy(enemy);
        }
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
        enemy.transform.position = spawnPoints[Random.Range(3, 7)].transform.position;
        enemies.Add(enemy);
        yield return new WaitForSecondsRealtime(0.5f);
        enemy = Instantiate(enemyPrefabs[(int)EnemyPrefabs.GoldfishDownUp], this.transform);
        enemy.transform.position = spawnPoints[Random.Range(3, 7)].transform.position;
        enemies.Add(enemy);
        yield return new WaitForSecondsRealtime(0.5f);
        enemy = Instantiate(enemyPrefabs[(int)EnemyPrefabs.GoldfishDownUp], this.transform);
        enemy.transform.position = spawnPoints[Random.Range(3, 7)].transform.position;
        enemies.Add(enemy);
    }
}
