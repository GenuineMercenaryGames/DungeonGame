using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCommonsUIController : MonoBehaviour
{
    public static void QuitToMainMenu()
    {
        GameTime.CanPause = true;
        GameTime.IsPaused = false;
        SceneManager.LoadScene(0);
    }

    public static void QuitToDesktop()
    {
        GameTime.CanPause = true;
        GameTime.IsPaused = false;
        Application.Quit();
    }

    public static void Retry()
    {
        GameTime.CanPause = true;
        GameTime.IsPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
