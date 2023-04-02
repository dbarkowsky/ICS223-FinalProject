using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] GameObject background;
    [SerializeField] Camera cam;

    private
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BroadcastPlayerLocation());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        Messenger.AddListener(GameEvent.PLAYER_DEAD, this.OnPlayerDead); // generic parameter is what's passed to function
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.PLAYER_DEAD, this.OnPlayerDead);
    }

    private void OnPlayerDead()
    {
        player.Respawn();
    }

    private IEnumerator BroadcastPlayerLocation()
    {
        while (true)
        {
            Vector2 playerCoordinates = new Vector2(player.transform.position.x, player.transform.position.y);
            Messenger<Vector2>.Broadcast(GameEvent.PLAYER_LOCATION, playerCoordinates);
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }

}
