using UnityEngine;

public class UIManager : SingletonPersistent<UIManager>
{
    [Header("UI Controller References")]
    [SerializeField] private PlayerUIController playerUI;
    [SerializeField] private ConsoleUIController consoleUI;

    // NOTE : These seemingly redundant getter properties are used so as to disallow public setting of these controller references.
    // This may not be a good idea tho, so it is bound to change in the future.
    public PlayerUIController PlayerUI { get { return playerUI; } }
    public ConsoleUIController ConsoleUI { get { return consoleUI; } }
}
