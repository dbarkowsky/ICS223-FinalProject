using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Controls the starting splash page
public class SplashPageController : MonoBehaviour
{
    [SerializeField] CanvasRenderer splash;
    [SerializeField] AudioClip titleTheme;
    [SerializeField] AudioClip startSound;
    private bool alreadyTriggered = false;
    
    // Start the title theme
    void Start()
    {
        SoundManager.Instance.PlayMusic(titleTheme);
    }

    // IF the user hits Enter, fade out
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !alreadyTriggered)
        {
            alreadyTriggered = true;
            StartCoroutine(FadeToBlack());
            SoundManager.Instance.StopMusic();
            SoundManager.Instance.PlaySfx(startSound);
        }
    }

    // Fades the screen to black, then loads the next scene
    // Colour should be all 255 to start, then reduces to all 0
    IEnumerator FadeToBlack()
    {
        float originalValue = splash.GetColor().g; // only checking green.
        float targetValue = 0f;
        float currentTime = 0f;
        float duration = 4f;
        while (currentTime < duration)
        {
            yield return null;
            float currentValue = Mathf.Lerp(originalValue, targetValue, currentTime / duration);
            splash.SetColor(new Color(currentValue, currentValue, currentValue));
            currentTime += Time.deltaTime;
        }
        SceneManager.LoadScene("Level1");
    }
}
