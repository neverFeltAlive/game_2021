using UnityEngine;
using UnityEngine.InputSystem;

using Custom.Mechanics;

namespace Custom.Controlls
{
    /// <summary>
    /// 
    /// PlayerOverloadController is used for controlling overload mechanics with user input
    /// :NeverFeltAlive
    /// 
    /// </summary>
    [RequireComponent(typeof(OverLoad))]
    public class PlayerOverloadController : MonoBehaviour
    {
        private OverLoad overload;



        private void Awake() =>
            overload = GetComponent<OverLoad>();

        public void TriggerOverload(InputAction.CallbackContext context)
        {
            if (context.performed)
                overload.TriggerOverLoad();
        }
    }
}
