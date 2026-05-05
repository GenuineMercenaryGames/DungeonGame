using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    #region Variables

    [Header("UI Controller References")]
    [SerializeField] private PlayerUIController playerUI;
    [SerializeField] private PauseUIController pauseUI;
    [SerializeField] private DefeatUIController defeatUI;
    [SerializeField] private VictoryUIController victoryUI;

    #endregion

    #region Properties

    public PlayerUIController PlayerUI { get { return playerUI; } }
    public PauseUIController PauseUI { get { return pauseUI; } }
    public DefeatUIController DefeatUI { get { return defeatUI; } }
    public VictoryUIController VictoryUI { get { return victoryUI; } }

    #endregion
}
