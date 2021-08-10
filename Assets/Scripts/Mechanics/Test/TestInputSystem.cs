using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace Platformer
{
    public class TestInputSystem : MonoBehaviour
    /* DEBUG statements for this document 
     * 
     * Debug.Log("TestInputSystem --> Start: ");
     * Debug.Log("<size=13><i><b> TestInputSystem --> </b></i><color=yellow> FixedUpdate: </color></size>");
     * Debug.Log("<size=13><i><b> TestInputSystem --> </b></i><color=red> Update: </color></size>");
     * Debug.Log("<size=13><i><b> TestInputSystem --> </b></i><color=blue> Corutine: </color></size>");
     * Debug.Log("<size=13><i><b> TestInputSystem --> </b></i><color=green> Function: </color></size>");
     * 
     */
    /* TODO
     * 
     */
    {
        public void TestButtonPressed(InputAction.CallbackContext context)
        {
            Debug.Log("<size=13><i><b> TestInputSystem --> </b></i><color=green> Function: </color></size>" + context.phase);
        }
    }
}
