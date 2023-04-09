using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashPageController : MonoBehaviour
{
    [SerializeField] CanvasRenderer splash;
    [SerializeField] float fadeDelta = 0.001f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(FadeToBlack());
        }
    }

    IEnumerator FadeToBlack()
    {
        Color current = splash.GetColor();
        Debug.Log(current);
        while (current.g > 0)
        {
            yield return new WaitForSecondsRealtime(0.00025f);
            current.r -= fadeDelta;
            current.b -= fadeDelta;
            current.g -= fadeDelta;
            splash.SetColor(current);
        }
        SceneManager.LoadScene("Level1");
    }
}
