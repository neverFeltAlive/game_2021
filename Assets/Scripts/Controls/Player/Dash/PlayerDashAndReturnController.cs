/// <remarks>
/// 
/// PlayerDashAndReturnController is used for controlling dash with user input 
/// NeverFeltAlive
/// 
/// </remarks>


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using Custom.Mechanics;

namespace Custom.Controlls
{
    [RequireComponent(typeof(DashAndReturn))]
    public class PlayerDashAndReturnController : PlayerDashContoller
    /* DEBUG statements for this document 
     * 
     * Debug.Log("PlayerDashAndReturnController --> Start: ");
     * Debug.Log("<size=13><i><b> PlayerDashAndReturnController --> </b></i><color=yellow> FixedUpdate: </color></size>");
     * Debug.Log("<size=13><i><b> PlayerDashAndReturnController --> </b></i><color=red> Update: </color></size>");
     * Debug.Log("<size=13><i><b> PlayerDashAndReturnController --> </b></i><color=blue> Corutine: </color></size>");
     * Debug.Log("<size=13><i><b> PlayerDashAndReturnController --> </b></i><color=green> Function: </color></size>");
     * 
     */
    /* TODO
     * 
     */
    {
        private DashAndReturn dashAndReturn;



        protected override void Awake()
        {
            base.Awake();
            dashAndReturn = GetComponent<DashAndReturn>();
        }

        public void TriggerReturn(InputAction.CallbackContext context)
        {
            if (context.canceled)
                dashAndReturn.TriggerReturn();
        }
    }
}
