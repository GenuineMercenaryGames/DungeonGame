using UnityEngine;

namespace Assets.Scripts.Controllers.Player
{
    public class PlayerAnimationEventController : MonoBehaviour
    {
        public void PlayStepSound()
        {
            SoundManager.Instance.PlaySound("player_step");
        }
    }
}
