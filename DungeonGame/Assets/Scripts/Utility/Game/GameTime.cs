using UnityEngine;

public static class GameTime
{
    private static bool isPaused = false;
    private static bool canPause = true;

    public static bool IsPaused { get { return isPaused; } set { SetPaused(value); } }
    public static bool CanPause { get { return canPause; } set { canPause = value; } }

    private static void SetPaused(bool newPaused)
    {
        if (!canPause && newPaused)
            return;

        isPaused = newPaused;

        if (isPaused)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }

}
