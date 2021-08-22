/// <remarks>
/// 
/// Mover is used for implementing basic movement mechanics 
/// Other scripts inherit from it to create more complex movement system
/// but major logic is here.
/// NeverFeltAlive
/// 
/// </remarks>


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Custom.Utils;

namespace Platformer.Mechanics.General
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Mover : MonoBehaviour
    /* DEBUG statements for this document 
     * 
     * Debug.Log("Mover --> Start: ");
     * Debug.Log("<size=13><i><b> Mover --> </b></i><color=yellow> FixedUpdate: </color></size>");
     * Debug.Log("<size=13><i><b> Mover --> </b></i><color=red> Update: </color></size>");
     * Debug.Log("<size=13><i><b> Mover --> </b></i><color=blue> Corutine: </color></size>");
     * Debug.Log("<size=13><i><b> Mover --> </b></i><color=green> Function: </color></size>");
     * 
     */
    /* TODO
     * 
     */
    {
        #region Serialized Fields
        [SerializeField] protected Rigidbody2D body;
        [Space]
        [Header("Main Movement Stats")]
        [SerializeField] [Range(0.1f, 5f)] protected float maxMovementSpeed = 1f;
        [SerializeField] [Range(0f, 1f)] protected float minMovementSpeed = 1f;
        [Space]
        #endregion

        protected bool isStopped = false;

        #region Public Fields
        [HideInInspector] public float currentSpeed = 1f;
        [HideInInspector] public float movementSlow = 1f;
        /// <remarks>
        /// Implement movement slow as a coroutine or separate functions
        /// </remarks>

        public Vector2 Direction { get; protected set; }
        #endregion



        protected virtual void Start()
        {
            if (!body)
                body = gameObject.GetComponent<Rigidbody2D>();
        }

        #region Protected Functions
        protected virtual void Move()
        {
            return;
        }
        protected virtual void Move(Vector2 direction)
        {
            // Check if movement is allowed
            if (isStopped)
            {
                body.velocity = Vector2.zero;
                return;
            }

            body.velocity = direction.normalized * currentSpeed;
            /// <remarks>
            /// Velosity is chosen because it is much more convinient when movement modifiers (such as dash) are present
            /// </remarks>
        }
        #endregion



        #region Coroutines
        IEnumerator Wait(float time)
        {
            isStopped = true;
            yield return new WaitForSeconds(time);
            isStopped = false;
        }
        #endregion

        /// <remarks>
        /// To create triggerable movement for PC and not triggerable for joystick with the same code
        /// we can use triggerMovement variable and magnitude 
        /// (amnimator transitions' conditions will also be set for triggering with bool depending on triggerMovement variable).
        /// </remarks>
    }
}
