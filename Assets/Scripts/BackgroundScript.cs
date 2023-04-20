using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Note: ideal image is 500px wide. Every 1000 pixels tall at scroll speed 0.025 takes 40 seconds to loop
// Goal is for 4 minutes of play, so 6000x500 is ideal height + beginning and ending segments

// Controls the behaviour of scrolling backgrounds
public class BackgroundScript : MonoBehaviour
{
    public float scrollSpeed;
    [SerializeField] private Renderer rend;
    private bool isScrolling = false;

    // Scroll the background if active
    void Update()
    {
        if (isScrolling)
        {
            rend.material.mainTextureOffset += new Vector2(0, scrollSpeed * Time.deltaTime);
        }
    }

    private void Awake()
    {
        Messenger.AddListener(GameEvent.START_BOSS_BATTLE, OnStartBossBattle);
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.START_BOSS_BATTLE, OnStartBossBattle);
    }

    void OnStartBossBattle()
    {
        isScrolling = true;
    }
}
