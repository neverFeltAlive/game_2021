/// <remarks>
/// 
/// CharacterMovementController is used for controlling target's core movement
/// NeverFeltAlive
/// 
/// </remarks>


using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

using Platformer.Mechanics.General;

namespace Platformer.Mechanics.Character
{
    public class CharacterMovementController : Mover
    /* DEBUG statements for this document 
     * 
     * Debug.Log("CharacterMovementController --> Start: ");
     * Debug.Log("<size=13><i><b> CharacterMovementController --> </b></i><color=yellow> FixedUpdate: </color></size>");
     * Debug.Log("<size=13><i><b> CharacterMovementController --> </b></i><color=red> Update: </color></size>");
     * Debug.Log("<size=13><i><b> CharacterMovementController --> </b></i><color=blue> Corutine: </color></size>");
     * Debug.Log("<size=13><i><b> CharacterMovementController --> </b></i><color=green> Function: </color></size>");
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



        public static event EventHandler<OnOverLoadStateChangeEventArgs> OnOverLoadStateChange;
        public class OnOverLoadStateChangeEventArgs : EventArgs
        {
            public OverLoadState state;
        }



        #region Serialized Fields
        [SerializeField] [Range(0f, 5f)] private float dashMovementSlow;
        [SerializeField] [Range(0f, 5f)] private float dashMovementSlowTime;
        [Space]
        [Header("OverLoad Stats")]
        [SerializeField] [Range(1f, 10f)] [Tooltip("Time overload remains active after casting")] private float overLoadTime = 5f;
        [SerializeField] [Range(10f, 100f)] private float overLoadCooldownTime = 30f;
        #endregion

        #region Private Fields
        private OverLoadState overLoadState;

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
            CharacterFightController.OnAttack += PowerAttackHandler;

            overLoadState = OverLoadState.InActive;
        }

        private void Update() =>
            direction = playerControls.MainControls.Walk.ReadValue<Vector2>();

        protected void FixedUpdate() =>
            Move(direction);
        #endregion

        #region Functions
        #region Event Handlers
        private void DashHandler(object sender, DashController.OnDashStateChangedEventArgs args)
        {
            if (args.state == DashController.DashState.Active)
                isStopped = true;
            if (args.state == DashController.DashState.Ready || args.state == DashController.DashState.OnCooldown)
                isStopped = false;
        }

        private void TrackHandler(object sender, TrackController.OnTrackEventArgs args)
        {
            StartCoroutine(DisableMovement(args.castingTime));
        }

        private void ShowState(object sender, OnOverLoadStateChangeEventArgs args) =>
            Debug.Log("<size=13><i><b> CharacterMovementController --> </b></i><color=green> ShowState: </color></size>" + args.state);

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
        #endregion
        #endregion



        #region Coroutines
        IEnumerator DisableMovement(float time = 0)
        {
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
