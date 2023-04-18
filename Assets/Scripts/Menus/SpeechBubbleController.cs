using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeechBubbleController : MonoBehaviour
{
    private TextMeshProUGUI bubble;
    private Coroutine currentScript;
    [SerializeField] float typingSpeed = 0.05f;
    [SerializeField] Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        bubble = this.gameObject.GetComponent<TextMeshProUGUI>();
        CatSays("Press Z to shoot 'em!");
    }

    private void Awake()
    {

    }

    private void OnDestroy()
    {

    }

    private void CatSays(string phrase)
    {
        if (currentScript != null)
        {
            StopCoroutine(currentScript);
            anim.SetBool("isTalking", false);
        }
        currentScript = StartCoroutine(WriteToBubble(phrase));
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
        anim.SetBool("isTalking", false);
    }

    private string[] death = {
        "Try hitting them before they hit you.",
        "I believe in you! Don't give up!",
        "Snake! Snaaaaaaaake!"
    };

    private string[] other =
    {
        "Do a barrel roll!",
        "You've switched off your targeting computer. What's wrong?"
    };

    private string pressZ = "Press Z to shoot 'em!";
    private string pickups = "Grab those pickups for better guns.";
    private string bossStart = "What's that up ahead? Sounds big.";
    private string bossDeath = "Yum. That should be enough crab for dinner.";
}
