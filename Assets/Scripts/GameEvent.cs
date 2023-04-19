using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines the event tags for GameEvents. Used by Messenger system.
/// </summary>
public static class GameEvent
{
    // Player
    public const string PLAYER_SHOOTS = "PLAYER_SHOOTS";
    public const string PLAYER_DEAD = "PLAYER_DEAD"; 
    public const string PLAYER_LOCATION = "PLAYER_LOCATION";

    // Enemy
    public const string ENEMY_DESTROYED = "ENEMY_DESTROYED";
    public const string ENEMY_DESTROYED_SELF = "ENEMY_DESTROYED_SELF";
    public const string OUT_OF_BOUNDS = "OUT_OF_BOUNDS";
    public const string ENEMY_TRIGGER_REACHED = "ENEMY_TRIGGER_REACHED";
    public const string START_BOSS_BATTLE = "START_BOSS_BATTLE";
    public const string CRAB_DESTROYED = "CRAB_DESTROYED";

    // UI
    public const string POPUP_OPENED = "POPUP_OPENED";
    public const string POPUP_CLOSED = "POPUP_CLOSED";

    // Game Operations
    public const string FADE_TO_SCORE = "FADE_TO_SCORE";
    public const string UPDATE_FINAL_SCORE = "UPDATE_FINAL_SCORE";
    public const string STOP_CAMERA = "STOP_CAMERA";
    public const string GAME_ACTIVE = "GAME_ACTIVE";
    public const string GAME_INACTIVE = "GAME_INACTIVE";

    // Items
    public const string PICKUP_TOUCHED = "PICKUP_TOUCHED";
    public const string PICKUP_NOTIFICATION = "PICKUP_NOTIFICATION";

    // Sound Effects
    public const string EXPLOSION = "EXPLOSION";
    public const string MEOW = "MEOW";

    // Boss Sounds
    public const string CLAW_HIT = "CLAW_HIT";
    public const string LASER_CHARGE = "LASER_CHARGE";
    public const string LASER_SHOOT = "LASER_SHOOT";
    public const string BOSS_YELL = "BOSS_YELL";
    public const string CLAW_SWIPE = "CLAW_SWIPE";

    // Music
    public const string START_BOSS_MUSIC = "START_BOSS_MUSIC";
    public const string START_RESULTS_MUSIC = "START_RESULTS_MUSIC";
    public const string START_LEVEL_MUSIC = "START_LEVEL_MUSIC";
}
