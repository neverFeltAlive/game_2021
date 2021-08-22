/// <remarks>
/// 
/// Dash is used for dashing character
/// NeverFeltAlive
/// 
/// </remarks>


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [add custom usings if needed]

namespace Custom.Mechanics
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Dash : MonoBehaviour
    /* DEBUG statements for this document 
     * 
     * Debug.Log("Dash --> Start: ");
     * Debug.Log("<size=13><i><b> Dash --> </b></i><color=yellow> FixedUpdate: </color></size>");
     * Debug.Log("<size=13><i><b> Dash --> </b></i><color=red> Update: </color></size>");
     * Debug.Log("<size=13><i><b> Dash --> </b></i><color=blue> Corutine: </color></size>");
     * Debug.Log("<size=13><i><b> Dash --> </b></i><color=green> Function: </color></size>");
     * 
     */
    /* TODO
     * 
     */
    {
        [SerializeField] protected float force = .45f;
        [SerializeField] protected float minMagnitude = .4f;

        private Rigidbody2D body;



        protected virtual void Awake() =>
            body = GetComponent<Rigidbody2D>();

        private Vector3 ModifyDirection(Vector3 direction)
        {
            if (direction.magnitude > minMagnitude)
                return direction;
            else 
                return direction * minMagnitude / direction.magnitude;
        }

        public virtual void HandleDash(Vector3 direction, float multiplier = 1f)
        {
            if (direction == Vector3.zero)
                return;

            direction = ModifyDirection(direction);
            body.MovePosition(transform.position + direction * multiplier * force);
        }
    }
}
