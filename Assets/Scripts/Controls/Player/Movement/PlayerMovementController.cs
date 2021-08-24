/// <remarks>
/// 
/// PlayerMovementController is used for controlling character movement using player input
/// NeverFeltAlive
/// 
/// </remarks>


using UnityEngine;

using Custom.Mechanics;

namespace Custom.Controlls
{
    [RequireComponent(typeof(CharacterMovement))]
    public class PlayerMovementController : MonoBehaviour
    /* DEBUG statements for this document 
     * 
     * Debug.Log("PlayerMovementController --> Start: ");
     * Debug.Log("<size=13><i><b> PlayerMovementController --> </b></i><color=yellow> FixedUpdate: </color></size>");
     * Debug.Log("<size=13><i><b> PlayerMovementController --> </b></i><color=red> Update: </color></size>");
     * Debug.Log("<size=13><i><b> PlayerMovementController --> </b></i><color=blue> Corutine: </color></size>");
     * Debug.Log("<size=13><i><b> PlayerMovementController --> </b></i><color=green> Function: </color></size>");
     * 
     */
    /* TODO
     * 
     */
    {
        public PlayerControls playerControls;
        private CharacterMovement movement;



        private void Awake() =>
            movement = GetComponent<CharacterMovement>();

        private void Start()
        {
            playerControls = new PlayerControls();
            playerControls.MainControls.Enable();
        }

        private void Update() => 
            movement.Direction = playerControls.MainControls.Walk.ReadValue<Vector2>();
    }
}
