/// <remarks>
/// 
/// PlayerMovementController is used for controlling target's core movement
/// NeverFeltAlive
/// 
/// </remarks>


using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

using Platformer.Mechanics.General;

namespace Platformer.Mechanics.Player
{
    public class PlayerMovementController : Mover
    /* DEBUG statements for this document 
     * 
     * Debug.Log("PlayerMovementController --> Start: ");
     * Debug.Log("<size=13><i><b> PlayerMovementController --> </b></i><color=yellow> FixedUpdate: </color></size>");
     * Debug.Log("<size=13><i><b> PlayerMovementController --> </b></i><color=red> Update: </color></size>");
     * Debug.Log("<size=13><i><b> PlayerMovementController --> </b></i><color=blue> Corutine: </color></size>");
     * Debug.Log("<size=13><i><b> PlayerMovementController --> </b></i><color=green> Function: </color></size>");
     * 
     */
    {
        public enum OverLoadState
        {
            Active, 
            Charging,
            InActive,
            OnCooldown
        }

        public enum RollState
        {
            Active,
            Ready
        }



        public static event EventHandler OnRoll;
        public static event EventHandler<OnOverLoadStateChangeEventArgs> OnOverLoadStateChange;
        public class OnOverLoadStateChangeEventArgs : EventArgs
        {
            public OverLoadState state;
        }



        #region Serialized Fields
        [Header("OverLoad Stats")]
        [SerializeField] [Range(1f, 10f)] [Tooltip("Time overload remains active after casting")] private float overLoadTime = 5f;
        [SerializeField] [Range(10f, 100f)] private float overLoadCooldownTime = 30f;
        [Space]
        [Header("Roll Stats")]
        [SerializeField] private float rollSpeed;
        [SerializeField] private float rollSpeedDrop;
        #endregion

        #region Private Fields
        private OverLoadState overLoadState;
        private RollState rollState;

        private Vector2 direction;
        #endregion

        public static PlayerControls playerControls;



        #region Context Menu
        [ContextMenu("Default values")]
        private void DefaultValues()
        {
            maxMovementSpeed = 1f;
            minMovementSpeed = 0f;

            overLoadTime = 5f;
            overLoadCooldownTime = 30f;
        }
        #endregion

        #region MonoBehaviour Callbacks
        protected sealed override void Start()
        {
            base.Start();

            playerControls = new PlayerControls();
            playerControls.MainControls.Enable();

            DashController.OnDashStateChanged += DashHandler;
            TrackController.OnTrack += TrackHandler;

            overLoadState = OverLoadState.InActive;
            rollState = RollState.Ready;
        }

        private void Update()
        {
            switch (rollState)
            {
                case RollState.Ready:
                    direction = playerControls.MainControls.Walk.ReadValue<Vector2>();
                    currentSpeed = Mathf.Clamp(direction.magnitude, minMovementSpeed, maxMovementSpeed);
                    break;

                case RollState.Active:
                    currentSpeed -= rollSpeed * rollSpeedDrop * Time.deltaTime;
                    if (currentSpeed <= 1f)
                        rollState = RollState.Ready;
                    break;
            }
        }

        private void FixedUpdate() =>
            Move(direction);
        #endregion

        #region Functions
        #region Event Handlers
        private void DashHandler(object sender, DashController.OnDashStateChangedEventArgs args)
        {
            if (args.state == DashController.DashState.Active)
                StartCoroutine(DisableMovement(.3f));
        }

        private void TrackHandler(object sender, TrackController.OnTrackEventArgs args)
        {
            StartCoroutine(DisableMovement(args.castingTime));
        }

        private void ShowState(object sender, OnOverLoadStateChangeEventArgs args) =>
            Debug.Log("<size=13><i><b> PlayerMovementController --> </b></i><color=green> ShowState: </color></size>" + args.state);

        private void PowerAttackHandler(object sender, Fighter.OnAttackEventArgs args)
        {
            if (args.isPower)
                isStopped = true;
            else
                isStopped = false;
        }
        #endregion

        #region Input Actions Handlers
        public void StopMovementForInteraction(InputAction.CallbackContext context)
        {
            if (context.started)
                isStopped = true;
            if (context.canceled)
                isStopped = false;
        }

        public void OverLoad(InputAction.CallbackContext context)
        {
            if (context.interaction is HoldInteraction)
            {
                if (context.started)
                {
                    isStopped = true;
                    overLoadState = OverLoadState.Charging;
                    OnOverLoadStateChange?.Invoke(this, new OnOverLoadStateChangeEventArgs { state = overLoadState });
                }
                if (context.performed)
                {
                    isStopped = false;
                    StartCoroutine(OverLoad());
                }
            }
        }

        public void Roll(InputAction.CallbackContext context)
        {
            if (rollState == RollState.Ready)
            {
                if (context.canceled)
                {
                    currentSpeed = rollSpeed;

                    OnRoll?.Invoke(this, EventArgs.Empty);
                    rollState = RollState.Active;
                }
            }
        }
        #endregion
        #endregion

        public bool IsMoving()
        {
            if (body.velocity == Vector2.zero)
                return false;
            else
                return true;
        }



        #region Coroutines
        IEnumerator DelayMovementEnable(float time = 0)
        {
            yield return new WaitForSeconds(time);
            isStopped = false;
        }

        IEnumerator DisableMovement(float time = 0)
        {
            Debug.Log("<size=13><i><b> PlayerMovementController --> </b></i><color=blue> DisableMovement: </color></size>");
            isStopped = true;
            if (time == 0)
                yield return new WaitForFixedUpdate();
            else
                yield return new WaitForSeconds(time);
            isStopped = false;
        }

        IEnumerator OverLoad()
        {
            overLoadState = OverLoadState.Active;
            OnOverLoadStateChange?.Invoke(this, new OnOverLoadStateChangeEventArgs { state = overLoadState });
            yield return new WaitForSeconds(overLoadTime);
            overLoadState = OverLoadState.OnCooldown;
            OnOverLoadStateChange?.Invoke(this, new OnOverLoadStateChangeEventArgs { state = overLoadState });
            yield return new WaitForSeconds(overLoadCooldownTime);
            overLoadState = OverLoadState.InActive;
            OnOverLoadStateChange?.Invoke(this, new OnOverLoadStateChangeEventArgs { state = overLoadState });
        }
        #endregion
    }
}
