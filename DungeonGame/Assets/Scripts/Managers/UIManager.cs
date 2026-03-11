using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    #region Variables

    [Header("UI Controller References")]
    [SerializeField] private PlayerUIController playerUI;
    [SerializeField] private PauseUIController pauseUI;
    [SerializeField] private GameOverUIController gameOverUI;

    #endregion

    #region Properties

    public PlayerUIController PlayerUI { get { return playerUI; } }
    public PauseUIController PauseUI { get { return pauseUI; } }
    public GameOverUIController GameOverUI { get { return gameOverUI; } }

    #endregion
}
