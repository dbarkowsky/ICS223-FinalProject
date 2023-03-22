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
        Quaternion rotation = transform.rotation;
        GameObject enemy = Instantiate(enemyPrefabs[0], this.transform);
        enemy.transform.position = spawnPoints[5].transform.position;
        enemies.Add(enemy);
    }

    // Update is called once per frame
    void Update()
    {
        enemies.ForEach(delegate (GameObject enemy)
        {
            enemy.GetComponent<EnemyController>().AmIOnScreen(cam);
        });
    }

    private void Awake()
    {
        Messenger<GameObject>.AddListener(GameEvent.ENEMY_DESTROYED, OnEnemyDestroyed);
    }

    private void OnDestroy()
    {
        Messenger<GameObject>.RemoveListener(GameEvent.ENEMY_DESTROYED, OnEnemyDestroyed);
    }

    private void OnEnemyDestroyed(GameObject enemy)
    {
        if (enemy)
        {
            Debug.Log(this + " Enemy destroyed.");
            enemies.RemoveAt(enemies.IndexOf(enemy));
            Destroy(enemy);
        }
    }
}
