using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Controls the starting splash page
public class SplashPageController : MonoBehaviour
{
    [SerializeField] CanvasRenderer splash;
    [SerializeField] float fadeDelta = 0.001f; // how fast the screen will fade. Smaller is slower
    [SerializeField] AudioClip titleTheme;
    [SerializeField] AudioClip startSound;
    
    // Start the title theme
    void Start()
    {
        SoundManager.Instance.PlayMusic(titleTheme);
    }

    // IF the user hits Enter, fade out
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(FadeToBlack());
            SoundManager.Instance.StopMusic();
            SoundManager.Instance.PlaySfx(startSound);
        }
    }

    // Fades the screen to black, then loads the next scene
    IEnumerator FadeToBlack()
    {
        Color current = splash.GetColor();
        while (current.g > 0)
        {
            yield return new WaitForSeconds(0.00025f);
            current.r -= fadeDelta;
            current.b -= fadeDelta;
            current.g -= fadeDelta;
            splash.SetColor(current);
        }
        SceneManager.LoadScene("Level1");
    }
}
