using TMPro;
using UnityEngine;

public class GameOverUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text victoryText;
    [SerializeField] private TMP_Text defeatText;

    public void Start()
    {
        gameObject.SetActive(false); // Temporary hack to hide the UI. Once animations are added, this will not be required.
    }

    public void ShowGameOver(bool isVictory)
    {
        gameObject.SetActive(true); // Temporary hack. Will add animations later. There will not be any need for this by then.
        victoryText.gameObject.SetActive(isVictory);
        defeatText.gameObject.SetActive(!isVictory);
    }
}
