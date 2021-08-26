using UnityEngine;
using UnityEngine.InputSystem;

using Custom.Mechanics;

namespace Custom.Controlls
{
    /// <summary>
    /// 
    /// PlayerRollController is used for triggering roll mechinics using player input
    /// :NeverFeltAlive
    /// 
    /// </summary>
    [RequireComponent(typeof(Roll))]
    public class PlayerRollController : MonoBehaviour
    {
        private Roll roll;



        private void Awake() =>
            roll = GetComponent<Roll>();

        public void TriggerRoll(InputAction.CallbackContext context)
        {
            if (context.canceled)
                roll.PerformRoll();
        }
    }
}
