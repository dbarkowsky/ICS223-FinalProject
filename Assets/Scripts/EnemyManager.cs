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
        Messenger<int>.AddListener(GameEvent.ENEMY_TRIGGER_REACHED, OnEnemyTriggerReached);
    }

    private void OnDestroy()
    {
        Messenger<GameObject>.RemoveListener(GameEvent.ENEMY_DESTROYED, OnEnemyDestroyed);
    }

    private void OnEnemyTriggerReached(int triggerID)
    {
        Debug.Log(triggerID.ToString());
        switch (triggerID)
        {
            case 0:
                int spawnPoint = Random.Range(3, 7);
                GameObject enemy = Instantiate(enemyPrefabs[0], this.transform);
                enemy.transform.position = spawnPoints[spawnPoint].transform.position;
                enemies.Add(enemy);
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

    private void DestroyEnemy(GameObject enemy)
    {
        enemies.RemoveAt(enemies.IndexOf(enemy));
        Destroy(enemy);
    }
}
