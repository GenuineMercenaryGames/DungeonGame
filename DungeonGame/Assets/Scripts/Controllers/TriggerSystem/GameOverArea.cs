using UnityEngine;

public class GameOverArea : MonoBehaviour
{
    public enum GameOverAreaType
    {
        Victory = 0,
        Defeat,
    }

    [SerializeField] public bool isActive = true;
    [SerializeField] public GameOverAreaType gameOverType = GameOverAreaType.Victory;

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out var player))
        {
            if (!isActive) return;
            if (gameOverType == GameOverAreaType.Victory)
                GameManager.Instance.StartVictory();
            else
                GameManager.Instance.StartDefeat();
        }
    }
}
