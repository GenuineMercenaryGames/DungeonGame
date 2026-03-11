using UnityEngine;

public class PauseUIController : MonoBehaviour
{
    public void Start()
    {
        gameObject.SetActive(false); // Temporary hack to hide the UI. Once animations are added, this will not be required.
        // All set active calls in this script are actually fucking hacks tbh. Again, once animations are added, these will no longer be required.
    }

    public void Pause()
    {
        if (!GameTime.CanPause)
            return;
        GameTime.IsPaused = true;
        gameObject.SetActive(true);
    }

    public void Resume()
    {
        GameTime.IsPaused = false;
        gameObject.SetActive(false);
    }
}
