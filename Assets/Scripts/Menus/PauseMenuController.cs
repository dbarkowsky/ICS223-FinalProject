using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Controls the pause menu
public class PauseMenuController : BasePopup
{
    // Close the popup
    public void OnResume()
    {
        Close();
    }

    // Reload the scene
    public void OnRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Messenger.Broadcast(GameEvent.POPUP_CLOSED);
    }

    // Exit the app
    public void OnQuit()
    {
        Application.Quit();
    }
}
