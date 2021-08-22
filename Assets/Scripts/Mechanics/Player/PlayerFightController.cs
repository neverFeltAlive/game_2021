/// <remarks>
/// 
/// PlayerFightController is used for [short description]
/// NeverFeltAlive
/// 
/// </remarks>


using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

using Platformer.Mechanics.General;
using Platformer.Mechanics.Objects;
using Custom.Utils;

namespace Platformer.Mechanics.Player
{
    public class PlayerFightController : Fighter
    /* DEBUG statements for this document 
     * 
     * Debug.Log("PlayerFightController --> Start: ");
     * Debug.Log("<size=13><i><b> PlayerFightController --> </b></i><color=yellow> FixedUpdate: </color></size>");
     * Debug.Log("<size=13><i><b> PlayerFightController --> </b></i><color=red> Update: </color></size>");
     * Debug.Log("<size=13><i><b> PlayerFightController --> </b></i><color=blue> Corutine: </color></size>");
     * Debug.Log("<size=13><i><b> PlayerFightController --> </b></i><color=green> Function: </color></size>");
     * 
     */
    /* TODO
     * 
     */
    {
        public enum FightState
        {
            Shooting,
            Normal
        }



        public static event EventHandler OnShoot;
        public static event EventHandler<OnFightStateChangedEventArgs> OnFightStateChanged;
        public class OnFightStateChangedEventArgs : EventArgs
        {
            public FightState state;
        }



        #region Serialized Fields
        [Space]
        [Header("Shooting Stats")]
        [SerializeField] private GameObject crosshair;
        [SerializeField] private float shootingRange = 1f;
        #endregion

        #region Priavte Fields
        private bool overLoad = true;

        private float lastStopTime;

        private FightState state;

        private Vector3 direction;
        #endregion



        #region Contex Menu
        [ContextMenu("Default Values")]
        private void DefaultValues()
        {
            maxHitPoints = 5;
            aimPunchResistance = 1.5f;

            attackRange = .2f;
            attackDashRange = .2f;

            damage = new Damage(amount: 1,  punch: 2f, instant: true);
            powerAttackDamage = new Damage(amount: 2,  punch: 5f, instant: true);

            shootingRange = 1f;
        }
        #endregion

        #region MonoBehaviour Callbacks
        protected sealed override void Start()
        {
            base.Start();

            PlayerMovementController.OnOverLoadStateChange += OverLoadHandler;

            crosshair.SetActive(false);
            targetTag = Constants.ENEMY_TAG;
            overLoad = false;
            state = FightState.Normal;
        }

        protected sealed override void Update()
        {
            base.Update();

            switch (state)
            {
                case FightState.Normal:
                    Vector2 input = PlayerMovementController.playerControls.MainControls.Walk.ReadValue<Vector2>();
                    if (input != Vector2.zero)
                        direction = input;
                    break;

                case FightState.Shooting:
                    if (GetComponent<PlayerMovementController>().IsMoving())
                        crosshair.SetActive(false);
                    else
                    {
                        input = PlayerMovementController.playerControls.MainControls.Aim.ReadValue<Vector2>();
                        if (input.magnitude > .5f)
                        {
                            input.Normalize();
                            crosshair.SetActive(true);
                            crosshair.transform.localPosition = input * .3f;
                            direction = input;
                        }
                    }
                    break;
            }
        }
        #endregion

        #region Functions
        private void OverLoadHandler(object sender, PlayerMovementController.OnOverLoadStateChangeEventArgs args)
        {
            if (args.state == PlayerMovementController.OverLoadState.Active)
                overLoad = true;
            else
                overLoad = false;
        }

        #region Input Actions Handlers
        public void TriggerAttack(InputAction.CallbackContext context)
        {
            if (state == FightState.Shooting)
                return;

            if (context.interaction is HoldInteraction)
            {
                if (context.performed)
                    StartCoroutine(PowerAttack(direction));
            }
            else
            {
                // Check interaction to destinquish power and simple attack
                if (overLoad)
                    StartCoroutine(PowerAttack(direction));
                else
                    HandleAttack(direction);
            }
        }

        public void Shoot(InputAction.CallbackContext context)
        {
            if (state == FightState.Normal)
                return;

            if (GetComponent<PlayerMovementController>().IsMoving())
                return;

            if (context.canceled)
            {
                Debug.Log("<size=13><i><b> PlayerFightController --> </b></i><color=green> Shoot: </color></size>");
                OnShoot?.Invoke(this, EventArgs.Empty);

                if (showDebug) Debug.DrawLine(crosshair.transform.position, crosshair.transform.position + direction.normalized * shootingRange, Color.red, .5f);

                RaycastHit2D hit = Physics2D.Raycast(crosshair.transform.position, direction.normalized, shootingRange);
                if (hit)
                {
                    if (hit.transform.tag == targetTag)
                        hit.transform.SendMessage("TakeDamage", damage);
                }
            }
        }

        public void ToggleShooting(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                switch (state)
                {
                    case FightState.Shooting:
                        state = FightState.Normal;
                        crosshair.SetActive(false);
                        break;
                    case FightState.Normal:
                        state = FightState.Shooting;
                        break;
                }

                OnFightStateChanged?.Invoke(this, new OnFightStateChangedEventArgs { state = state });
            }
        }
        #endregion
        #endregion
    }
}
