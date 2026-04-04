using Assets.Scripts.Generation;
using UnityEngine;

// NOTE : The current implementation is a temporary workaround to get things going for the deadline.
// We will be improving upon this system later on.
// For now, all this does is end the game with victory when all the enemies die. Yeah, it is what it is.
public class GameManager : MonoBehaviour
{
    [SerializeField] private HealthController[] enemies;
    [SerializeField] private GameObject player;
    [SerializeField] private World worldGenerator;

    int deaths;
    int numEnemies;

    void Start()
    {
        deaths = 0;
        numEnemies = enemies.Length;
        foreach (var enemy in enemies)
        {
            enemy.Health.AddListener(EnemyKilled);
        }

        if(worldGenerator != null)
        {
            var characterController = player.GetComponent<CharacterController>();
            characterController.enabled = false;
            player.transform.position = new Vector3(worldGenerator.PlayerSpawnPosition.x, player.transform.position.y, worldGenerator.PlayerSpawnPosition.z); // TODO: No sé si meter esto aquí o hacer un manager dedicado. -kike
            characterController.enabled = true;
            
        }
        BackgroundMusicManager.Instance.PlayBackgroundMusicWithFade(AudioNames.BackgroundMusic, 2.5f);
    }

    void EnemyKilled(float oldValue, float newValue)
    {
        if (newValue <= 0.0f && oldValue > 0.0f)
        {
            ++deaths;
            if (deaths >= numEnemies)
            {
                UIManager.Instance.GameOverUI.ShowGameOver(true);
            }
        }
    }

}
