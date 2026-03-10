using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [Header("UI Controller References")]
    // [SerializeField] private ConsoleUIController consoleUI;
    [SerializeField] private PlayerUIController playerUI;
    [SerializeField] private PauseUIController pauseUI;

    // NOTE : These seemingly redundant getter properties are used so as to disallow public setting of these controller references.
    // This may not be a good idea tho, so it is bound to change in the future.
    public PlayerUIController PlayerUI { get { return playerUI; } }
    public PauseUIController PauseUI { get { return pauseUI; } }
    // public ConsoleUIController ConsoleUI { get { return consoleUI; } }
}
