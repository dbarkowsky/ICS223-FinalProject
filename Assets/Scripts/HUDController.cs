using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class HUDController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timer;
    private float timePassed = 0; // time in seconds

    [SerializeField] private TextMeshProUGUI deaths;
    private uint deathCount = 0;
    [SerializeField] private TextMeshProUGUI score;
    private uint scoreValue = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        timer.text = "Time: " + CalculateTime();
    }

    private String CalculateTime()
    {
        float minutes = Mathf.FloorToInt(timePassed / 60);
        float seconds = Mathf.FloorToInt(timePassed % 60);
        return string.Format("{0:0}:{1:00}", minutes, seconds);
    }

    private void Awake()
    {
        Messenger.AddListener(GameEvent.PLAYER_DEAD, this.OnPlayerDead);
        Messenger<GameObject>.AddListener(GameEvent.ENEMY_DESTROYED, this.OnEnemyDestroyed);
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.PLAYER_DEAD, this.OnPlayerDead);
        Messenger<GameObject>.RemoveListener(GameEvent.ENEMY_DESTROYED, this.OnEnemyDestroyed);
    }

    private void OnPlayerDead()
    {
        deaths.text = "Deaths: " + (++deathCount).ToString();
    }

    private void OnEnemyDestroyed(GameObject enemy)
    {
        scoreValue += enemy.GetComponent<EnemyController>().scoreValue;
        score.text = "Score: \n" + scoreValue.ToString();
    }
}
