using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // General Sounds
    [SerializeField] AudioClip playerShooting;
    [SerializeField] AudioClip explosion;

    // Songs
    [SerializeField] AudioClip levelSong;
    [SerializeField] AudioClip bossSong;
    [SerializeField] AudioClip resultsTheme;

    // Boss sounds
    [SerializeField] AudioClip clawClink;
    [SerializeField] AudioClip laserCharge;
    [SerializeField] AudioClip laserShoot;
    [SerializeField] AudioClip bossYell;
    [SerializeField] AudioClip clawSwipe;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        Messenger.AddListener(GameEvent.PLAYER_SHOOTS, OnPlayerShoots);
        Messenger.AddListener(GameEvent.EXPLOSION, OnExplosion);
        Messenger.AddListener(GameEvent.START_BOSS_MUSIC, OnStartBossMusic);
        Messenger.AddListener(GameEvent.START_RESULTS_MUSIC, OnStartResultsMusic);
        Messenger.AddListener(GameEvent.START_LEVEL_MUSIC, OnStartLevelMusic);
        Messenger.AddListener(GameEvent.CLAW_HIT, OnClawHit);
        Messenger.AddListener(GameEvent.CLAW_SWIPE, OnClawSwipe);
        Messenger.AddListener(GameEvent.LASER_CHARGE, OnLaserCharge);
        Messenger.AddListener(GameEvent.LASER_SHOOT, OnLaserShoot);
        Messenger.AddListener(GameEvent.BOSS_YELL, OnBossYell);
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.PLAYER_SHOOTS, OnPlayerShoots);
        Messenger.RemoveListener(GameEvent.EXPLOSION, OnExplosion);
        Messenger.RemoveListener(GameEvent.START_BOSS_MUSIC, OnStartBossMusic);
        Messenger.RemoveListener(GameEvent.START_LEVEL_MUSIC, OnStartLevelMusic);
        Messenger.RemoveListener(GameEvent.CLAW_HIT, OnClawHit);
        Messenger.RemoveListener(GameEvent.CLAW_SWIPE, OnClawSwipe);
        Messenger.RemoveListener(GameEvent.LASER_CHARGE, OnLaserCharge);
        Messenger.RemoveListener(GameEvent.LASER_SHOOT, OnLaserShoot);
        Messenger.RemoveListener(GameEvent.BOSS_YELL, OnBossYell);
    }

    void OnPlayerShoots()
    {
        SoundManager.Instance.PlaySfx(playerShooting);
    }
    void OnExplosion()
    {
        SoundManager.Instance.PlaySfx(explosion);
    }

    void OnStartBossMusic()
    {
        SoundManager.Instance.PlayMusic(bossSong);
    }

    void OnStartLevelMusic()
    {
        SoundManager.Instance.PlayMusic(levelSong);
    }

    void OnStartResultsMusic()
    {
        SoundManager.Instance.PlayMusic(resultsTheme);
    }

    void OnClawHit()
    {
        SoundManager.Instance.PlaySfx(clawClink);
    }

    void OnClawSwipe()
    {
        SoundManager.Instance.PlaySfx(clawSwipe);
    }

    void OnLaserCharge()
    {
        SoundManager.Instance.PlaySfx(laserCharge);
    }

    void OnLaserShoot()
    {
        SoundManager.Instance.PlaySfx(laserShoot);
    }

    void OnBossYell()
    {
        SoundManager.Instance.PlaySfx(bossYell);
    }
}
