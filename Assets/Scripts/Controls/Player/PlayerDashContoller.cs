using UnityEngine;
using UnityEngine.InputSystem;

using Custom.Mechanics;

namespace Custom.Controlls
{
    /// <summary>
    /// 
    /// PlayerDashContoller is used for controlling dash with user input
    /// :NeverFeltAlive
    /// 
    /// </summary>
    [RequireComponent(typeof(Dash))]
    public class PlayerDashContoller : MonoBehaviour
    {
        protected Dash dash;



        private void Awake() =>
            dash = GetComponent<Dash>();
        public virtual void TriggerDash(InputAction.CallbackContext context)
        {
            Vector3 direction = PlayerController.Instance.playerControls.MainControls.Walk.ReadValue<Vector2>();
            if (context.canceled)
                dash.PerformDash(direction);
        }
    }
}
