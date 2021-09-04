using UnityEngine;
using UnityEngine.InputSystem;

namespace Custom.Controlls
{
    /// <summary>
    /// 
    /// SlowDownTime is used for controlling time slowing with user input
    /// :NeverFeltAlive
    /// 
    /// </summary>
    public class SlowDownTime : MonoBehaviour
    {
        public void SlowTime(InputAction.CallbackContext context)
        {
            TimeController.Instance.ChangeTimeScale(.1f);
        }
    }
}
