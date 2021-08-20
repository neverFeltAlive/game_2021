/// <remarks>
/// 
/// DashController is used for controlling target's dashing mechanics
/// NeverFeltAlive
/// 
/// </remarks>


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

using Platformer.Utils;

namespace Platformer.Mechanics.Character
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CharacterMovementController))]
    public class DashController : MonoBehaviour
    /* DEBUG statements for this document 
     * 
     * Debug.Log("DashController --> Start: ")
     * Debug.Log("<size=13><i><b> DashController --> </b></i><color=yellow> FixedUpdate: </color></size>");
     * Debug.Log("<size=13><i><b> DashController --> </b></i><color=red> Update: </color></size>");
     * Debug.Log("<size=13><i><b> DashController --> </b></i><color=blue> Corutine: </color></size>");
     * Debug.Log("<size=13><i><b> DashController --> </b></i><color=green> Function: </color></size>");
     * 
     */
    /* TODO: 
     *
     * Queuing dashes
     *
     */
    {
        public enum DashState
        {
            Ready,
            Active,
            OnCooldown
        }
        public enum ReturnState
        {
            Active,
            InActive
        }



        public static event EventHandler<OnDashStateChangedEventArgs> OnDashStateChanged;
        public class OnDashStateChangedEventArgs : EventArgs
        {
            public DashState state;
            public bool isPower;
        }
        public static event EventHandler<OnReturnStateChangedEventArgs> OnReturnStateChanged;
        public class OnReturnStateChangedEventArgs : EventArgs
        {
            public ReturnState state;
        }



        #region Serialized Fields
        [SerializeField] private Rigidbody2D characterBody;
        [Space]
        [SerializeField] [Range(0f, 100f)] [Tooltip("Dashing force")] private float multiplier = .45f;
        [SerializeField] [Range(0f, 100f)] [Tooltip("Dashing force for overloaded dash")] private float overLoadMultiplier = 1f;
        [SerializeField] [Range(0f, 100f)] [Tooltip("Minimum value for the input to be changed")] private float minMagnitude = .6f;
        [SerializeField] [Range(1, 5)] [Tooltip("Max number of dashed available at once")] private int maxNumberOfDashes = 5;
        [Space]
        [Header("Cooldown settings")]
        [SerializeField] [Range(1f, 10f)] [Tooltip("Dash cooldown time in seconds")] private float cooldownTime = 3f;
        [SerializeField] [Range(1f, 10f)] [Tooltip("Dash cooldown time after every return in seconds")] private float returnCooldownTime = 2f;
        [SerializeField] [Range(0f, 10f)] [Tooltip("Dash cooldown time after last return in seconds")] private float lastReturnCooldownTime = .5f;
        [SerializeField] [Range(.1f, 5f)] [Tooltip("Time  after one dash untill coldown is triggered")] private float cooldownTriggeringTime = .8f;
        #endregion

        #region Private Fields
        private bool overLoad;

        private float currentCooldownTriggeringTime;

        private DashState dashState;
        private ReturnState returnState;

        private Vector3 direction = new Vector3(1, 0);

        private List<Vector2> savedCoordinates;
        #endregion

        private bool showDebug = true;
        /// <remarks>
        /// Set to true to show debug vectors
        /// </remarks>



        #region Context Menu
        [ContextMenu("Default Values")]
        private void DefaultValues()
        {
            characterBody = gameObject.GetComponent<Rigidbody2D>();
            characterBody.drag = 5f;

            multiplier = .45f;
            overLoadMultiplier = 1f;
            minMagnitude = .6f;
            maxNumberOfDashes = 5;

            cooldownTime = 3f;
            returnCooldownTime = 2f;
            lastReturnCooldownTime = .5f;
            cooldownTriggeringTime = .8f;
        }   
        #endregion

        #region MonoBehaviour Callbacks
        private void Start()
        {
            if (!characterBody)
                characterBody = gameObject.GetComponent<Rigidbody2D>();

            dashState = DashState.Ready;
            returnState = ReturnState.InActive;
            OnDashStateChanged?.Invoke(this, new OnDashStateChangedEventArgs { state = dashState });
            OnReturnStateChanged?.Invoke(this, new OnReturnStateChangedEventArgs { state = returnState });

            OnDashStateChanged += ShowState;
            OnReturnStateChanged += ShowState;
            CharacterMovementController.OnOverLoadStateChange += OverLoadHandler;

            savedCoordinates = new List<Vector2>();
            currentCooldownTriggeringTime = cooldownTriggeringTime;
            overLoad = false;
        }

        private void Update()
        {
            CheckCooldownTriggeringTime();

            Vector2 input = CharacterMovementController.playerControls.MainControls.Walk.ReadValue<Vector2>();
            if (input.magnitude > minMagnitude)
                direction = input;
            else if (input.magnitude > .01f)
                direction = input * minMagnitude / input.magnitude;
        }
        #endregion

        #region Functions
        // Check if too much time passed since last dash
        private void CheckCooldownTriggeringTime()
        {
            if (dashState == DashState.Ready && savedCoordinates.Count != 0)
            {
                if (currentCooldownTriggeringTime - Time.deltaTime < 0)
                {
                    currentCooldownTriggeringTime = cooldownTriggeringTime;
                    StartCoroutine(Cooldown(cooldownTime));
                }
                else
                    currentCooldownTriggeringTime -= Time.deltaTime;
            }
            else
                currentCooldownTriggeringTime = cooldownTriggeringTime;
        }

        public void Dash(InputAction.CallbackContext context)
        {
            if (dashState != DashState.Ready)
                return;

            if (context.interaction is HoldInteraction)
            {
                if (context.performed)
                    StartCoroutine(DashCoroutine(true));
            }
            else
            {
                if (overLoad)
                    StartCoroutine(DashCoroutine(true));
                else
                    StartCoroutine(DashCoroutine());
            }
        }

        public void Return(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;

            if (returnState == ReturnState.Active)
            {
                gameObject.transform.position = savedCoordinates[savedCoordinates.Count - 1];    
                savedCoordinates.RemoveAt(savedCoordinates.Count - 1);

                float cooldownTime;
                if (savedCoordinates.Count == 0)
                {
                    returnState = ReturnState.InActive;
                    OnReturnStateChanged?.Invoke(this, new OnReturnStateChangedEventArgs { state = returnState });
                    cooldownTime = lastReturnCooldownTime;
                }
                else
                    cooldownTime = returnCooldownTime;

                StopAllCoroutines();
                StartCoroutine(Cooldown(cooldownTime));
                /// <remarks>
                /// We need to stop all coroutines to make last return completely restart cooldown with new timer
                /// </remarks>
            }
        }

        private void ShowState(object sender, OnDashStateChangedEventArgs args) =>
            Debug.Log("<size=13><i><b> DashController --> </b></i><color=green> Dash State: </color></size>" + args.state);
        private void ShowState(object sender, OnReturnStateChangedEventArgs args) =>
            Debug.Log("<size=13><i><b> DashController --> </b></i><color=green> Return State: </color></size>" + args.state);

        private void OverLoadHandler(object sender, CharacterMovementController.OnOverLoadStateChangeEventArgs args)
        {
            if (args.state == CharacterMovementController.OverLoadState.Active)
                overLoad = true;
            else
                overLoad = false;
        }
        #endregion



        #region Coroutines
        IEnumerator Cooldown(float time)
        {
            dashState = DashState.OnCooldown;
            OnDashStateChanged?.Invoke(this, new OnDashStateChangedEventArgs { state = dashState });

            yield return new WaitForSeconds(time); // wait for required amount of seconds

            savedCoordinates = new List<Vector2>();
            returnState = ReturnState.InActive;
            dashState = DashState.Ready;
            OnDashStateChanged?.Invoke(this, new OnDashStateChangedEventArgs { state = dashState });
            OnReturnStateChanged?.Invoke(this, new OnReturnStateChangedEventArgs { state = returnState });
        }

        IEnumerator DashCoroutine(bool isPowerDash = false)
        {
            dashState = DashState.Active;
            OnDashStateChanged?.Invoke(this, new OnDashStateChangedEventArgs { state = dashState , isPower = isPowerDash});

            Vector2 target;
            if (isPowerDash)
                target = transform.position + direction * overLoadMultiplier;
            else
                target = transform.position + direction * multiplier;

            characterBody.MovePosition(target);
            
            // DEBUG
            if (showDebug)
                Debug.DrawLine(transform.position, target, Color.yellow, .125f);

            yield return new WaitForFixedUpdate();

            savedCoordinates.Add(transform.position);
            returnState = ReturnState.Active;
            OnReturnStateChanged?.Invoke(this, new OnReturnStateChangedEventArgs { state = returnState });

            // DEBUG
            if (showDebug)
                UtilsClass.DrawCross(transform.position, Color.yellow, cooldownTime);

            yield return new WaitForSeconds(.3f);
            /// <remarks>
            /// Shoud be set to animation time
            /// </remarks>
            
            if (isPowerDash && !overLoad)
                StartCoroutine(Cooldown(cooldownTime));
            else
            {
                if (savedCoordinates.Count == maxNumberOfDashes)
                    StartCoroutine(Cooldown(cooldownTime));
                else
                {
                    dashState = DashState.Ready;
                    OnDashStateChanged?.Invoke(this, new OnDashStateChangedEventArgs { state = dashState });
                }
            }
        }
        /// <remarks>
        /// Dash is implemented as a coroutine because of the need to save coordinates
        /// We cannot save target location before moving to that location as we need to check if there are any colliders on the way
        /// </remarks>
        #endregion
    }
}

