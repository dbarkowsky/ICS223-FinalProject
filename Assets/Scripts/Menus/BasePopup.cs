using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The base class for all popups
public class BasePopup : MonoBehaviour
{
    // Opens the popup
    virtual public void Open()
    {
        if (!IsActive())
        {
            gameObject.SetActive(true);
            Messenger.Broadcast(GameEvent.POPUP_OPENED);
        }
        else
        {
            Debug.LogError(this + ".Open() - trying to open popup that is active!");
        }
    }

    // Closes the popup
    virtual public void Close()
    {
        if (IsActive())
        {
            gameObject.SetActive(false);
            Messenger.Broadcast(GameEvent.POPUP_CLOSED);
        }
        else
        {
            Debug.LogError(this + ".Close() - trying to close popup that is already closed!");
        }
    }

    // Checks if the popup is active
    public bool IsActive()
    {
        return gameObject.activeSelf;
    }
}
