using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : BasePopup
{
    public void OnResume()
    {
        Close();
    }

    public void OnRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Messenger.Broadcast(GameEvent.POPUP_CLOSED);
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
