using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip levelSong;
    [SerializeField] AudioClip bossSong;
    [SerializeField] AudioClip playerShooting;
    [SerializeField] AudioClip explosion;
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.PlayMusic(levelSong);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        Messenger.AddListener(GameEvent.PLAYER_SHOOTS, OnPlayerShoots);
        Messenger.AddListener(GameEvent.EXPLOSION, OnExplosion);
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.PLAYER_SHOOTS, OnPlayerShoots);
        Messenger.RemoveListener(GameEvent.EXPLOSION, OnExplosion);
    }

    void OnPlayerShoots()
    {
        SoundManager.Instance.PlaySfx(playerShooting);
    }
    void OnExplosion()
    {
        SoundManager.Instance.PlaySfx(explosion);
    }
}
