/// <remarks>
/// 
/// CharacterVisualsController is used for shooting
/// NeverFeltAlive
/// 
/// </remarks>


using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

using Platformer.Utils;

namespace Platformer.Mechanics.Character
{
    public class CharacterShootingController : MonoBehaviour
    /* DEBUG statements for this document 
     * 
     * Debug.Log("CharacterShootingController --> Start: ");
     * Debug.Log("<size=13><i><b> CharacterShootingController --> </b></i><color=yellow> FixedUpdate: </color></size>");
     * Debug.Log("<size=13><i><b> CharacterShootingController --> </b></i><color=red> Update: </color></size>");
     * Debug.Log("<size=13><i><b> CharacterShootingController --> </b></i><color=blue> Corutine: </color></size>");
     * Debug.Log("<size=13><i><b> CharacterShootingController --> </b></i><color=green> Function: </color></size>");
     * 
     */
    {
        public static event EventHandler<OnToggleShootingEventArgs> OnToggleShooting;
        public class OnToggleShootingEventArgs : EventArgs
        {
            public bool isShootingOn;
        }



        [SerializeField] private GameObject crosshair;

        private PlayerInput playerInput;



        public void Start()
        {
            playerInput = GetComponent<PlayerInput>();

            crosshair.SetActive(false);
        }

        private void Update()
        {
            Aim();
        }

        #region Functions
        private void ChangeActionMap()
        {
            // DEBUG
            string currentMapName = playerInput.currentActionMap.name;

            if (playerInput.currentActionMap.name == Constants.DEFAULT_MAP)
            {
                OnToggleShooting?.Invoke(this, new OnToggleShootingEventArgs { isShootingOn = true });

                playerInput.SwitchCurrentActionMap(Constants.SHOOTING_MAP);
                CharacterMovementController.playerControls.MainControls.Disable();
                CharacterMovementController.playerControls.ShootingControls.Enable();

                crosshair.SetActive(true);
            }
            else if (playerInput.currentActionMap.name == Constants.SHOOTING_MAP)
            {
                OnToggleShooting?.Invoke(this, new OnToggleShootingEventArgs { isShootingOn = false });

                playerInput.SwitchCurrentActionMap(Constants.DEFAULT_MAP);
                CharacterMovementController.playerControls.MainControls.Enable();
                CharacterMovementController.playerControls.ShootingControls.Disable();

                crosshair.SetActive(false);
            }

            // DEBUG
            Debug.Log("<size=13><i><b> CharacterShootingController --> </b></i><color=green> ChangeActionMap: </color></size> Action map changed to " + 
                playerInput.currentActionMap.name + " from " + currentMapName);
        }

        private void Aim()
        {
            Vector2 input = CharacterMovementController.playerControls.ShootingControls.Aim.ReadValue<Vector2>();
            if (input.magnitude > 0f)
            {
                input.Normalize();
                crosshair.transform.localPosition = input * .3f;
            }
        }

        public void ToggleShooting(InputAction.CallbackContext context)
        {
            if (context.performed)
                ChangeActionMap();
        }

        public void Shoot(InputAction.CallbackContext context)
        {
            if (context.canceled)
                // DEBUG
                Debug.Log("<size=13><i><b> CharacterShootingController --> </b></i><color=green> Shoot: </color></size>" + context.phase);
        }
        #endregion
    }
}
