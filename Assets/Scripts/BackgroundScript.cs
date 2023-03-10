using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Note: ideal image is 500px wide. Every 1000 pixels tall at scroll speed 0.025 takes 40 seconds to loop
// Goal is for 4 minutes of play, so 6000x500 is ideal height + beginning and ending segments
public class BackgroundScript : MonoBehaviour
{
    public float scrollSpeed;
    [SerializeField] private Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rend.material.mainTextureOffset += new Vector2(0, scrollSpeed * Time.deltaTime);
    }
}
