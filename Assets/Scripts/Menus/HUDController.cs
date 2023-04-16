using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timer;
    private float timePassed = 0; // time in seconds

    [SerializeField] private TextMeshProUGUI deaths;
    private uint deathCount = 0;

    [SerializeField] private TextMeshProUGUI score;
    private uint scoreValue = 0;

    [SerializeField] private PauseMenuController pauseMenu;
    [SerializeField] private Image endScreen;
    [SerializeField] private TextMeshProUGUI endTitle;
    [SerializeField] private TextMeshProUGUI endStats;
    [SerializeField] private TextMeshProUGUI endPrompt;
    private bool canRestart = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        timer.text = "Time: " + CalculateTime();

        if (Input.GetKeyDown(KeyCode.Escape) && !pauseMenu.IsActive())
        {
            SetGameActive(false);
            pauseMenu.Open();
        }

        if (Input.GetKeyDown(KeyCode.Return) && canRestart)
        {
            canRestart = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Color transparent = endScreen.color;
            transparent.a = 0;
            endScreen.color = transparent;
            endTitle.gameObject.SetActive(false);
            endStats.gameObject.SetActive(false);
            endPrompt.gameObject.SetActive(false);
            endScreen.gameObject.SetActive(false);
        }
    }

    public void SetGameActive(bool active)
    {
        if (active)
        {
            Time.timeScale = 1;     // unpause the game
            Cursor.lockState = CursorLockMode.Locked; // lock cursor at centre
            Cursor.visible = false; // hide cursor
        }
        else
        {
            Time.timeScale = 0; // pause the game
            Cursor.lockState = CursorLockMode.None; // cursor moves freely
            Cursor.visible = true; // show the cursor
        }
    }

    private string CalculateTime()
    {
        float minutes = Mathf.FloorToInt(timePassed / 60);
        float seconds = Mathf.FloorToInt(timePassed % 60);
        return string.Format("{0:0}:{1:00}", minutes, seconds);
    }

    private void UpdateFinalScore()
    {
        endStats.text = "Time: " + CalculateTime() + "\nScore: " + scoreValue + "\nDeaths: " + deathCount;
    }

    private IEnumerator FadeToScoreEnum()
    {
        UpdateFinalScore();
        endScreen.color = new Color(endScreen.color.r, endScreen.color.g, endScreen.color.b, 0f);
        endScreen.gameObject.SetActive(true);
        endTitle.gameObject.SetActive(false);
        endStats.gameObject.SetActive(false);
        endPrompt.gameObject.SetActive(false);

        float duration = 2f; //Fade in over 2 seconds.
        float currentTime = 0f;
        while (currentTime < duration)
        {
            float alpha = Mathf.Lerp(0f, 1f, currentTime / duration);
            endScreen.color = new Color(endScreen.color.r, endScreen.color.g, endScreen.color.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }
        endScreen.color = new Color(endScreen.color.r, endScreen.color.g, endScreen.color.b, 1f);
        endTitle.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.5f);
        endStats.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        endPrompt.gameObject.SetActive(true);
        canRestart = true;
        yield break;
    }

    private void Awake()
    {
        Messenger.AddListener(GameEvent.PLAYER_DEAD, this.OnPlayerDead);
        Messenger<GameObject>.AddListener(GameEvent.ENEMY_DESTROYED, this.OnEnemyDestroyed);
        Messenger.AddListener(GameEvent.POPUP_OPENED, OnPopupOpened);
        Messenger.AddListener(GameEvent.POPUP_CLOSED, OnPopupClosed);
        Messenger.AddListener(GameEvent.FADE_TO_SCORE, OnFadeToScore);
        Messenger.AddListener(GameEvent.CRAB_DESTROYED, this.OnCrabDestroyed);
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.PLAYER_DEAD, this.OnPlayerDead);
        Messenger<GameObject>.RemoveListener(GameEvent.ENEMY_DESTROYED, this.OnEnemyDestroyed);
        Messenger.RemoveListener(GameEvent.POPUP_OPENED, OnPopupOpened);
        Messenger.RemoveListener(GameEvent.POPUP_CLOSED, OnPopupClosed);
        Messenger.RemoveListener(GameEvent.FADE_TO_SCORE, OnFadeToScore);
        Messenger.RemoveListener(GameEvent.CRAB_DESTROYED, this.OnCrabDestroyed);
    }

    private void OnCrabDestroyed()
    {
        scoreValue += 10000;
    }

    private void OnFadeToScore()
    {
        StartCoroutine(FadeToScoreEnum());
    }

    private void OnPopupOpened()
    {
        SetGameActive(false);
    }

    private void OnPopupClosed()
    {
        SetGameActive(true);
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
