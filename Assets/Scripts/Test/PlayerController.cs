/// <remarks>
/// 
/// PlayerController is used for controlling major player actions such as input.
/// It is also responsible for correlations between separate parts of mechanics
/// NeverFeltAlive
/// 
/// </remarks>


using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

using Custom.Mechanics;
using Platformer.Visuals;

namespace Platformer.Controls
{
    public class PlayerController : MonoBehaviour
    /* DEBUG statements for this document 
     * 
     * Debug.Log("PlayerController --> Start: ");
     * Debug.Log("<size=13><i><b> PlayerController --> </b></i><color=yellow> FixedUpdate: </color></size>");
     * Debug.Log("<size=13><i><b> PlayerController --> </b></i><color=red> Update: </color></size>");
     * Debug.Log("<size=13><i><b> PlayerController --> </b></i><color=blue> Corutine: </color></size>");
     * Debug.Log("<size=13><i><b> PlayerController --> </b></i><color=green> Function: </color></size>");
     * 
     */
    /* TODO
     * 
     */
    {
        public PlayerControls playerControls;
        private CharacterMovement movement;
        private VectorMeleeAttack attack;
        private PowerMeleeAttack powerAttack;
        private TriggerableTrack track;
        private DashAndReturn dash;
        private Roll roll;
        private PlayerVisualsController visuals;

        public Vector3 Position { get { return transform.position; } }
        public static PlayerController Instance { get; private set; }



        private void Awake()
        {
            Instance = this;

            dash = GetComponent<DashAndReturn>();
            movement = GetComponent<CharacterMovement>();
            roll = GetComponent<Roll>();
            track = GetComponent<TriggerableTrack>();
            attack = GetComponent<VectorMeleeAttack>();
            powerAttack = GetComponent<PowerMeleeAttack>();
            visuals = GetComponent<PlayerVisualsController>();
        }

        private void Start()
        {
            playerControls = new PlayerControls();
            playerControls.MainControls.Enable();
        }

        private void Update()
        {
            movement.Direction = playerControls.MainControls.Walk.ReadValue<Vector2>();
            attack.Direction = movement.Direction;
        }

        public void Roll(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                roll.TriggerRoll();
                visuals.PlayRollAnimmation();
            }
        }

        public void Dash(InputAction.CallbackContext context)
        {
            if (dash == null)
                return;

            if (context.interaction is HoldInteraction)
            {
                if (context.performed)
                    dash.TriggerDash(movement.Direction, true);
            }
            else
            {
                dash.TriggerDash(movement.Direction, false);
            }

            visuals.PlayDashAnimation();
        }

        public void Track(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                track.TriggerSaving();
            }

            if (context.canceled)
            {
                track.TriggerTrack();
                visuals.PlayTrackAnimation(track.castingTime);
            }
        }

        public void Return(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;

            if (dash == null)
                return;

            dash.HandleReturn();
        }

        public void Attack(InputAction.CallbackContext context)
        {
            if (context.interaction is HoldInteraction)
            {
                if (context.performed)
                    powerAttack.TriggerAttack();
            }
            else
            {
                attack.TriggerAttack();
            }

            visuals.PlayAttackAnimation();
        }
    }
}
