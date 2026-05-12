using Assets.Scripts.Generation;
using UnityEngine;

// NOTE : The current implementation is a temporary workaround to get things going for the deadline.
// We will be improving upon this system later on.
public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject player;
    [SerializeField] private World worldGenerator;

    public bool GameHasEnded {  get; private set; }

    void Start()
    {
        GameHasEnded = false;
        if(worldGenerator != null)
        {
            var characterController = player.GetComponent<CharacterController>();
            characterController.enabled = false;
            player.transform.position = new Vector3(worldGenerator.PlayerSpawnPosition.x, player.transform.position.y, worldGenerator.PlayerSpawnPosition.z); // TODO: No sé si meter esto aquí o hacer un manager dedicado. -kike
            characterController.enabled = true;
            
        }
        BackgroundMusicManager.Instance?.PlayBackgroundMusicWithFade(AudioNames.BackgroundMusic, 2.5f);
    }

    public void StartDefeat()
    {
        if (GameHasEnded) return; // Disallow starting victory after death
        GameHasEnded = true;
        UIManager.Instance.DefeatUI.gameObject.SetActive(true); // TODO : This is a temporary hack, use the start animation stuff
        SoundManager.Instance.PlaySound("on_die_screen");
        PlayerManager.Instance.Player.inputEnabled = false;
    }

    public void StartVictory()
    {
        if (GameHasEnded) return; // Disallow starting defeat in the event that player dies after victory
        GameHasEnded = true;
        UIManager.Instance.VictoryUI.gameObject.SetActive(true);
        UIManager.Instance.VictoryUI.RunVictory();
        SoundManager.Instance.PlaySound("victory_sound");
        PlayerManager.Instance.Player.inputEnabled = false;
    }

    public void StartGameOver(bool victory)
    {
        if (victory)
            StartVictory();
        else
            StartDefeat();
    }
}
