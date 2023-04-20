using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Main game manager. Controls high-level game functions.
public class GameManager : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] GameObject background;
    [SerializeField] CameraController cam;
    [SerializeField] GameObject explosion;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BroadcastPlayerLocation());
        Messenger.Broadcast(GameEvent.START_LEVEL_MUSIC);
    }

    private void Awake()
    {
        Messenger.AddListener(GameEvent.PLAYER_DEAD, this.OnPlayerDead); // generic parameter is what's passed to function
        Messenger.AddListener(GameEvent.CRAB_DESTROYED, this.OnCrabDestroyed);
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.PLAYER_DEAD, this.OnPlayerDead);
        Messenger.RemoveListener(GameEvent.CRAB_DESTROYED, this.OnCrabDestroyed);
    }

    private void OnCrabDestroyed()
    {
        StartCoroutine(EndGame());
    }

    // Disable the player and push them out of the screen
    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(3f);
        player.Disable();
        float exitDuration = 6f;
        StartCoroutine(player.ExitTopOfScreen(exitDuration));
        yield return new WaitForSeconds(exitDuration);
    }

    // Creates explosion on player's location and respawns player
    private void OnPlayerDead()
    {
        if (player.canBeHit)
        {
            GameObject exp = Instantiate(explosion, player.transform.position, player.transform.rotation);
            exp.transform.localScale = new Vector3(1, 1, 1);
            Messenger.Broadcast(GameEvent.EXPLOSION);
            player.Respawn();
        }
    }

    // Broadcasts the player's location on repeat
    private IEnumerator BroadcastPlayerLocation()
    {
        while (true)
        {
            Vector2 playerCoordinates = new Vector2(player.transform.position.x, player.transform.position.y);
            Messenger<Vector2>.Broadcast(GameEvent.PLAYER_LOCATION, playerCoordinates);
            yield return new WaitForSeconds(0.3f);
        }
    }

}
