/// <remarks>
/// 
/// PlayerDashContoller is used for controlling dash with user input
/// NeverFeltAlive
/// 
/// </remarks>


using System;
using UnityEngine;
using UnityEngine.InputSystem;

using Custom.Mechanics;

namespace Custom.Controlls
{
    [RequireComponent(typeof(Dash))]
    public class PlayerDashContoller : PlayerMechanicsController<DashEventArgs, Dash>
    /* DEBUG statements for this document 
     * 
     * Debug.Log("PlayerDashContoller --> Start: ");
     * Debug.Log("<size=13><i><b> PlayerDashContoller --> </b></i><color=yellow> FixedUpdate: </color></size>");
     * Debug.Log("<size=13><i><b> PlayerDashContoller --> </b></i><color=red> Update: </color></size>");
     * Debug.Log("<size=13><i><b> PlayerDashContoller --> </b></i><color=blue> Corutine: </color></size>");
     * Debug.Log("<size=13><i><b> PlayerDashContoller --> </b></i><color=green> Function: </color></size>");
     * 
     */
    /* TODO
     * 
     */
    {
        protected Vector3 direction;
        protected PlayerControls playerControls;



        protected override void Awake()
        {
            base.Awake();
            playerControls = new PlayerControls();
            playerControls.MainControls.Enable();
        }

        protected virtual void Update() =>
            direction = playerControls.MainControls.Walk.ReadValue<Vector2>();

        public override void Trigger(InputAction.CallbackContext context)
        {
            if (context.canceled)
                mechanics.Trigger(direction);
        }
    }
}
