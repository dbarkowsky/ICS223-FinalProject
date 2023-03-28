using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvent
{
    public const string PLAYER_DEAD = "PLAYER_DEAD"; // just defines the event tag
    public const string ENEMY_DESTROYED = "ENEMY_DESTROYED";
    public const string OUT_OF_BOUNDS = "OUT_OF_BOUNDS";
    public const string ENEMY_TRIGGER_REACHED = "ENEMY_TRIGGER_REACHED";
}
