using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePopup : MonoBehaviour
{
    // Start is called before the first frame update
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

    public bool IsActive()
    {
        return gameObject.activeSelf;
    }
}
