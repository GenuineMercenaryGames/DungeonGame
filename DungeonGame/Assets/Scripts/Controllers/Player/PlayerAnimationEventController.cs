using UnityEngine;

namespace Assets.Scripts.Controllers.Player
{
    public class PlayerAnimationEventController : MonoBehaviour
    {
        public void PlayStepSound()
        {
            SfxManager.Instance.PlaySfx("player_step", 5.0f, false);
        }
    }
}
