using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject background;
    [SerializeField] Camera view;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        Messenger<PlayerController>.AddListener(GameEvent.PLAYER_DEAD, this.OnPlayerDead); // generic parameter is what's passed to function
        Messenger<EnemyController>.AddListener(GameEvent.ENEMY_DESTROYED, this.OnEnemyDestroyed);
    }

    private void OnDestroy()
    {
        Messenger<PlayerController>.RemoveListener(GameEvent.PLAYER_DEAD, this.OnPlayerDead);
        Messenger<EnemyController>.RemoveListener(GameEvent.ENEMY_DESTROYED, this.OnEnemyDestroyed);
    }

    private void OnPlayerDead(PlayerController player)
    {

    }

    private void OnEnemyDestroyed(EnemyController enemy)
    {

    }
}
