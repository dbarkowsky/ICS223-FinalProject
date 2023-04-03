using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
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
        Messenger<int>.AddListener(GameEvent.ENEMY_TRIGGER_REACHED, OnEnemyTriggerReached);
    }

    private void OnDestroy()
    {
        Messenger<GameObject>.RemoveListener(GameEvent.ENEMY_DESTROYED, OnEnemyDestroyed);
        Messenger<GameObject>.RemoveListener(GameEvent.ENEMY_DESTROYED_SELF, OnEnemyDestroyedSelf);
        Messenger<int>.RemoveListener(GameEvent.ENEMY_TRIGGER_REACHED, OnEnemyTriggerReached);
    }

    private void OnEnemyTriggerReached(int triggerID)
    {
        Debug.Log(triggerID.ToString());
        int spawnPoint;
        switch (triggerID)
        {
            case 0:
                spawnPoint = Random.Range(3, 7);
                GameObject enemy = Instantiate(enemyPrefabs[0], this.transform);
                enemy.transform.position = spawnPoints[spawnPoint].transform.position;
                enemies.Add(enemy);
                break;
            case 1:
                spawnPoint = 3;
                GameObject enemy1 = Instantiate(enemyPrefabs[1], this.transform);
                enemy1.transform.position = spawnPoints[spawnPoint].transform.position;
                enemies.Add(enemy1);
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
}
