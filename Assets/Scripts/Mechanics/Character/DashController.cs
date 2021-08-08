/// <remarks>
/// 
/// DashController is used for controlling target's dashing mechanics
/// NeverFeltAlive
/// 
/// </remarks>


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        // Fields

        #region Serialized Fields
        [Space]
        [Space]
        [Header("DashController Script")]
        [Space]
        [Header("Components")]
        [SerializeField] private Rigidbody2D characterBody;
        [SerializeField] private CharacterMovementController movement;
        [Space]

        [Header("Variables")]
        [SerializeField] [Range(1, 5)] [Tooltip("Max number of dashed available at once")] private int maxNumberOfDashes = 3;
        [SerializeField] [Range(1, 10)] [Tooltip("Number of fractions in one dash")] private int numberOfFractions = 1;
        [Space]
        //[SerializeField] [Range(0f, 2f)] [Tooltip("Dashing speed")] private float speed = 1f;
        //[SerializeField] [Range(0.1f, 1f)] [Tooltip("Dashing minimal distance")] private float minRange = 5f;
        //[SerializeField] [Range(0.2f, 2f)] [Tooltip("Dashing maximum distance")] private float maxRange = 10f;
        [SerializeField] [Range(1f, 100f)] [Tooltip("Dashing force")] private float multiplier = 10f;
        [SerializeField] [Range(0f, 2f)] [Tooltip("Delay between fractions in seconds")] private float fractionDelay = 0.1f;
        [SerializeField] [Range(0f, 2f)] [Tooltip("Delay after last fraction in seconds")] private float lastFractionDelay = 0.1f;
        [Space]
        [SerializeField] [Range(0f, 5f)] [Tooltip("Movement speed slow down value")] private float slow = 3f;
        [SerializeField] [Range(0f, 5f)] [Tooltip("Movement speed slow down time")] private float slowTime = 1f;
        [SerializeField] [Range(0f, 5f)] [Tooltip("Movement stop time after return")] private float returnStopTime = 1f;
        [Space]
        [SerializeField] [Range(1, 10)] [Tooltip("Dash cooldown time in seconds")] private float defaultCooldownTime = 5f;
        [SerializeField] [Range(1, 10)] [Tooltip("Dash cooldown time after last return in seconds")] private float returnCooldownTime = 3f;
        [SerializeField] [Range(1, 10)] [Tooltip("Dash cooldown time after every return in seconds")] private float returnCooldownRefreshTime = 2f;
        [SerializeField] [Range(0f, 1f)] [Tooltip("Distance from dash point required to trigger coldown")] private float cooldownDistance = 0.2f;
        [SerializeField] [Range(1, 5)] [Tooltip("Time  after one dash untill coldown is triggered")] private float cooldownTriggeringTime = 2f;
        #endregion

        #region Private Fields
        private Vector2 direction = new Vector2(1, 0);
        private Vector2[] savedCoordinates;

        private float currentSlowTime = 0f;                         // check for slowing time
        private float range;                         // check for slowing time

        private int numberOfDashes;                                 // stores current number of dashes available
        private int lastDashIndex = 0;                              // saves the lindex of the last dash

        private bool onCooldown = false;                            // flag for dash cooldownTime
        private bool isDashing = false;                             // trigger dash
        private bool triggerReturn = false;                         // trigger return
        private bool triggerSlow = false;                           // trigger movement slowdown 
        private bool isSlowing = false;                             // flag for movement slowdown 
        #endregion

        // Functions 

        #region Context Menu
        [ContextMenu("Default Values")]
        private void DefaultValues()
        {
            characterBody = gameObject.GetComponent<Rigidbody2D>();
            movement = gameObject.GetComponent<CharacterMovementController>();

            maxNumberOfDashes = 3;
            numberOfFractions = 5;

            multiplier = 30f;

            slow = 2f;
            slowTime = 3f;
            returnStopTime = 1f;

            fractionDelay = 0.1f;
            lastFractionDelay = 0.1f;

            defaultCooldownTime = 5f;
            returnCooldownTime = 2f;
            returnCooldownRefreshTime = 2f;
            cooldownDistance = 0.3f;
            cooldownTriggeringTime = 3f;
        }        
        
        [ContextMenu("Boy Default")]
        private void BoyDefault()
        {
            characterBody = gameObject.GetComponent<Rigidbody2D>();
            movement = gameObject.GetComponent<CharacterMovementController>();

            maxNumberOfDashes = 3;
            numberOfFractions = 1;

            multiplier = 10f;

            slow = 3f;
            slowTime = 1f;
            returnStopTime = 0.25f;

            fractionDelay = 0.1f;
            lastFractionDelay = 0.1f;

            defaultCooldownTime = 5f;
            returnCooldownTime = 3f;
            returnCooldownRefreshTime = 2f;
            cooldownDistance = 0.2f;
            cooldownTriggeringTime = 2f;
        }
        #endregion

        #region MonoBehaviour Callbacks
        private void Start()
        {
            // Assign default values
            savedCoordinates = new Vector2[maxNumberOfDashes];
            numberOfDashes = maxNumberOfDashes;
            currentSlowTime = slowTime;

            // Manage components
            if (!characterBody)
                characterBody = gameObject.GetComponent<Rigidbody2D>();
            if (!movement)
                movement = gameObject.GetComponent<CharacterMovementController>();
        }

        private void Update()
        {
            #region Slowing Down
            // Check if slow is triggered
            if (triggerSlow)
            {
                // Manage flags
                triggerSlow = false;
                isSlowing = true;

                // movementSlow down
                movement.movementSlow /= slow;

                // DEBUG
                Debug.Log("<size=13><i><b> DashController --> </b></i><color=red> Update: </color></size>Slowed down to: "
                    + movement.currentSpeed + " from: " + movement.currentSpeed / slow);
            }

            // Check if character is slowing
            if (isSlowing)
            {
                if (currentSlowTime > 0)
                    currentSlowTime -= Time.deltaTime;      // count time

                // Check timer and if character dashed again
                if (currentSlowTime <= 0 || isDashing)
                {
                    movement.movementSlow *= slow;                 // speed up
                    isSlowing = false;                      // stop slowing
                    currentSlowTime = slowTime;             // reset timer

                    // DEBUG
                    Debug.Log("<size=13><i><b> DashController --> </b></i><color=red> Update: </color></size>Speeded up back to: "
                        + movement.currentSpeed);
                }

                /// <remarks>
                /// These if conditions should be separate for more percise time counting
                /// </remarks>
            }
            /// <summary>
            /// Can't make this work in coroutine because it is not possible to both stop slowing when character dashes and percisely count time if slowTime is float
            /// </summary>
            #endregion

            // Check if dash is unavailable but no cooldownTime started
            if (numberOfDashes == 0 && !onCooldown && !isDashing)
                StartCoroutine(Cooldown(defaultCooldownTime));

            // Check if any dashes were complete
            if (lastDashIndex != 0)
            {
                // Check if the target has moves since last dash
                if (Vector3.Distance(gameObject.transform.position, savedCoordinates[lastDashIndex - 1]) >= cooldownDistance && !isDashing && !onCooldown && numberOfDashes != maxNumberOfDashes)
                    StartCoroutine(Cooldown(defaultCooldownTime, "player moved"));

                //  Trigger teleportation to the last saved coordinates
                if (Input.GetButtonDown(Constants.B_BUTTON))
                    triggerReturn = true;
            }

            // Define dashing _direction
            if (!isDashing)
            {
                float horizontalDirection = Input.GetAxis(Constants.MOVEMENT_JOYSTICK_X);
                float vertivalDirection = Input.GetAxis(Constants.MOVEMENT_JOYSTICK_Y);
                direction = new Vector2(horizontalDirection, vertivalDirection);
                //if (horizontalDirection != 0f || vertivalDirection != 0f)                             // check if _direction was changed
            }

            // Trigger dash 
            if (Input.GetButtonDown(Constants.Y_BUTTON) && numberOfDashes != 0 && !isDashing && !onCooldown)
                StartCoroutine(DashCoroutine());
        }

        private void FixedUpdate()
        {
            // Teleport target to saved coordinates
            if (triggerReturn)
            {
                gameObject.transform.position = savedCoordinates[lastDashIndex - 1];    // teleport target to saved coordinates
                savedCoordinates[lastDashIndex - 1] = Vector2.zero;                     // discard coordinates

                // Disable movement
                movement.StopMoving(returnStopTime);
                /// <remarks>
                /// We need to stop movement after returning for better controls.
                /// Can implement return animation with this logic also
                /// </remarks>

                // DEBUG   
                Debug.Log("<size=13><i><b> DashController --> </b></i><color=yellow> FixedUpdate: </color></size> Player returned to: " + savedCoordinates[lastDashIndex - 1] + " Current position " + gameObject.transform.position);

                lastDashIndex--;                                                        // change last index

                // Renew cooldownTime
                if (lastDashIndex == 0)
                {
                    // Reset all bool variables to defaults
                    ResetStatus();

                    StopAllCoroutines();
                    StartCoroutine(Cooldown(returnCooldownTime, "player returned for the last time"));
                }
                else
                {
                    // Reset all bool variables to defaults
                    ResetStatus();

                    StopAllCoroutines();
                    StartCoroutine(Cooldown(returnCooldownRefreshTime, "player returned"));
                }

                triggerReturn = false;
            }
        }
        #endregion

        #region Private Functions
        // Dash the target (one fraction)
        private void Dash() =>
            characterBody.MovePosition(characterBody.position + direction * Time.fixedDeltaTime * multiplier / numberOfFractions);      
        /// <remarks>
        /// Need to come up with max and min range system 
        /// </remarks>

        // Reset all flags and stats
        private void ResetStatus()
        {
            // Reset stats
            if (isSlowing)
                movement.currentSpeed *= slow;

            // Reset flags
            onCooldown = false;                            // flag for dash cooldownTime
            isDashing = false;                             // trigger dash
            triggerReturn = false;                         // trigger return
            triggerSlow = false;                           // trigger movement slowdown 
            isSlowing = false;                             // flag for movement slowdown 
        }
        /// <summary> 
        /// This metthod prevents mistakes if corutines are stoped before they finish 
        /// </summary> 
        #endregion

        // Coroutines

        #region IEnumerators
        // Dash the target
        IEnumerator DashCoroutine()
        {
            isDashing = true;

            // Turn off movement
            movement.enabled = false;

            for (int i = 1; i <= numberOfFractions; i++)
            {
                Dash();

                // If not last itteration
                if (i != numberOfFractions)
                    yield return new WaitForSeconds(fractionDelay);
                else
                    yield return new WaitForSeconds(lastFractionDelay);
            }


            numberOfDashes--;                                                           // change number of dashes

            // Need to wait to save new coordinates
            yield return new WaitForFixedUpdate();

            lastDashIndex++;                                                            // change the index
            savedCoordinates[lastDashIndex - 1] = gameObject.transform.position;

            // DEBUG
            Debug.Log("<size=13><i><b> DashController --> </b></i><color=blue> Corutine: </color></size>Index: " +
                lastDashIndex + " Coordinates " + savedCoordinates[lastDashIndex - 1] + " Current possisiton: " + gameObject.transform.position);

            // Turn movement back on
            movement.enabled = true;

            // Manage flags
            isDashing = false;
            triggerSlow = true;

            // Trigger dash cooldownTime based on time
            if (isDashing || onCooldown) // check if cooldownTime already started
                yield break;
            else
            {
                // Wait and start cooldownTime
                yield return new WaitForSeconds(cooldownTriggeringTime);
                numberOfDashes = 0;                                                         // trigger cooldownTime
            }
        }

        // Dash cooldownTime
        IEnumerator Cooldown(float time, string reason = "no dash charges or time left")
        {
            onCooldown = true;                              // change flag

            // DEBUG
            Debug.Log("<size=13><i><b> DashController --> </b></i><color=blue> Corutine: </color></size>Dash on cooldown => " + reason);

            yield return new WaitForSeconds(time); // wait for required amount of seconds

            // DEBUG
            Debug.Log("<size=13><i><b> DashController --> </b></i><color=blue> Corutine: </color></size>Dash ready");

            // Restore defaults
            numberOfDashes = maxNumberOfDashes;
            onCooldown = false;
            savedCoordinates = new Vector2[maxNumberOfDashes];                     // discard coordinates
            lastDashIndex = 0;
        }
        #endregion
    }
}

