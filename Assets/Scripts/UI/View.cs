using UnityEngine;

public abstract class View : MonoBehaviour
{
    public abstract void Initialize();

    public virtual void Hide()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameObject.SetActive(false);
        ViewManager.SetIsOpen();
    }

    public virtual void Show()
    {        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameObject.SetActive(true);
        ViewManager.SetIsOpen();
    }
}
