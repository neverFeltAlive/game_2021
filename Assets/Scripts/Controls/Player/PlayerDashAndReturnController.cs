using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

using Custom.Mechanics;

namespace Custom.Controlls
{
    /// <summary>
    /// 
    /// PlayerDashAndReturnController is used for controlling dash with user input 
    /// :NeverFeltAlive
    /// 
    /// </summary>
    [RequireComponent(typeof(DashAndReturn))]
    public class PlayerDashAndReturnController : MonoBehaviour
    {
        private DashAndReturn dash;



        private void Awake()
        {
            dash = GetComponent<DashAndReturn>();
        }
        public void TriggerDash(InputAction.CallbackContext context)
        {
            Vector3 direction = PlayerController.Instance.playerControls.MainControls.Walk.ReadValue<Vector2>();
            if (context.interaction is HoldInteraction)
            {
                if (context.performed)
                    dash.TriggerDash(direction, true);
            }
            else
            {
                if (context.canceled)
                    dash.TriggerDash(direction);
            }
        }

        public void TriggerReturn(InputAction.CallbackContext context)
        {
            if (context.canceled)
                dash.Return();
        }
    }
}
