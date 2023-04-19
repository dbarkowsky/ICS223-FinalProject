using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeechBubbleController : MonoBehaviour
{
    private TextMeshProUGUI bubble;
    private Coroutine currentScript;
    [SerializeField] float typingSpeed = 0.1f;
    [SerializeField] Animator anim;

    private float timeSinceLastTalk = 0f;
    private float deadAirTimeLimit = 20f;

    private bool currentlyTalking = false;
    // Start is called before the first frame update
    void Start()
    {
        bubble = this.gameObject.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        timeSinceLastTalk += Time.deltaTime;
        if (timeSinceLastTalk > deadAirTimeLimit)
        {
            timeSinceLastTalk = 0f;
            int option = Random.Range(0, other.Length);
            CatSays(other[option]);
        }
    }

    private void Awake()
    {
        Messenger.AddListener(GameEvent.START_LEVEL_MUSIC, OnStartLevel);
        Messenger.AddListener(GameEvent.START_BOSS_MUSIC, OnStartBossMusic);
        Messenger.AddListener(GameEvent.CRAB_DESTROYED, OnCrabDestroyed);
        Messenger.AddListener(GameEvent.PICKUP_NOTIFICATION, OnPickupSeen);
        Messenger<PickupController>.AddListener(GameEvent.PICKUP_TOUCHED, OnPickupTouched);
        Messenger.AddListener(GameEvent.PLAYER_DEAD, OnPlayerDead);
        Messenger.AddListener(GameEvent.START_BOSS_BATTLE, OnStartBossBattle);
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.START_LEVEL_MUSIC, OnStartLevel);
        Messenger.RemoveListener(GameEvent.START_BOSS_MUSIC, OnStartBossMusic);
        Messenger.RemoveListener(GameEvent.CRAB_DESTROYED, OnCrabDestroyed);
        Messenger.RemoveListener(GameEvent.PICKUP_NOTIFICATION, OnPickupSeen);
        Messenger<PickupController>.RemoveListener(GameEvent.PICKUP_TOUCHED, OnPickupTouched);
        Messenger.RemoveListener(GameEvent.PLAYER_DEAD, OnPlayerDead);
        Messenger.RemoveListener(GameEvent.START_BOSS_BATTLE, OnStartBossBattle);
    }

    void OnStartLevel()
    {
        CatSays(pressZ);
    }

    void OnStartBossMusic()
    {
        CatSays(bossStart);
    }

    void OnPickupSeen()
    {
        CatSays(pickups);
    }

    void OnPickupTouched(PickupController pickup)
    {
        CatSays(pickupTouched);
    }

    void OnCrabDestroyed()
    {
        CatSays(bossDeath);
    }

    void OnPlayerDead()
    {
        int option = Random.Range(0, death.Length);
        CatSays(death[option]);
    }

    void OnStartBossBattle()
    {
        CatSays(bossSeen);
    }

    private void CatSays(string phrase)
    {
        if (!currentlyTalking)
        {
            currentlyTalking = true;
            if (currentScript != null)
            {
                StopCoroutine(currentScript);
                anim.SetBool("isTalking", false);
            }
            timeSinceLastTalk = 0f;
            currentScript = StartCoroutine(WriteToBubble(phrase));
        }     
    }

    private IEnumerator WriteToBubble(string contents)
    {
        anim.SetBool("isTalking", true);
        for (int i = 0; i <= contents.Length; i++)
        {
            bubble.text = contents.Substring(0, i);
            Messenger.Broadcast(GameEvent.MEOW);
            yield return new WaitForSecondsRealtime(typingSpeed);
        }
        currentlyTalking = false;
        anim.SetBool("isTalking", false);
    }

    private string[] death = {
        "Try hitting them before they hit you.",
        "I believe in you! Don't give up!",
        "Snake!? Snaaaaaaaake!", 
        "No, no! Avoid the bullets!",
        "Did you forget your goggles?",
        "Don't worry. Cats have nine lives.",
        "I've seen better flying from a penguin.",
        "Just a minor setback.",
        "This must be revenge for all the tuna I ate."
    };

    private string[] other =
    {
        "Do a barrel roll!",
        "You've switched off your targeting computer. What's wrong?",
        "Do you ever wonder why we're here?",
        "Robot fish? Are they delicious?",
        "I'd kill for a bit of catnip right now."
    };

    private string pressZ = "Press Z to shoot 'em!";
    private string pickups = "Grab those pickups for better guns.";
    private string pickupTouched = "That's right. Blast them!";
    private string bossStart = "What's that up ahead? Sounds big.";
    private string bossSeen = "Shoot him in the head!";
    private string bossDeath = "Yum. That should be enough crab for dinner.";
}
