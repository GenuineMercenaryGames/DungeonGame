using UnityEngine;

// NOTE : For now, this is just a dummy manager whose only purpose is to handle player reference delivery to other managers and controllers in the code.
// When the dungeon generation algorithm is finished, to integrate this into a proper level, the system will need to be modified so as to allow player input manager
// spawning logic to be used, which will take quite a few lines of code.
// That code will go here. So, this is just a warning to other readers, to not delete this or think it is useless. Right now, it lacks code, because it doesn't need
// it yet, but it will be required once the project advances to the next stage.
public class PlayerManager : Singleton<PlayerManager>
{
    public PlayerController Player { get; private set; }

    public void SetPlayer(PlayerController player)
    {
        Player = player;
        UIManager.Instance.PlayerUI.Init();
    }
}
