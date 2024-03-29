using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls interaction with the SoundManager
// Essentially a single point to store audio, catch events, ask SoundManager to play
public class AudioManager : MonoBehaviour
{
    // General Sounds
    [SerializeField] AudioClip playerShooting;
    [SerializeField] AudioClip explosion;
    [SerializeField] AudioClip meow;
    [SerializeField] AudioClip pickup;

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
        Messenger.AddListener(GameEvent.MEOW, OnMeow);
        Messenger<PickupController>.AddListener(GameEvent.PICKUP_TOUCHED, OnPickup);
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
        Messenger.RemoveListener(GameEvent.MEOW, OnMeow);
        Messenger<PickupController>.RemoveListener(GameEvent.PICKUP_TOUCHED, OnPickup);
    }

    void OnPlayerShoots()
    {
        SoundManager.Instance.PlaySfx(playerShooting);
    }
    void OnExplosion()
    {
        SoundManager.Instance.PlaySfx(explosion);
    }

    void OnPickup(PickupController pickupScript)
    {
        SoundManager.Instance.PlaySfx(pickup);
    }

    void OnMeow()
    {
        SoundManager.Instance.PlaySfx(meow);
    }

    void OnStartBossMusic()
    {
        SoundManager.Instance.StartSmoothly(bossSong);
    }

    void OnStartLevelMusic()
    {
        SoundManager.Instance.PlayMusic(levelSong);
    }

    void OnStartResultsMusic()
    {
        SoundManager.Instance.StartSmoothly(resultsTheme);
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
