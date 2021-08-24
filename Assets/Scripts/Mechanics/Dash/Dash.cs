/// <remarks>
/// 
/// Dash is used for dashing character
/// NeverFeltAlive
/// 
/// </remarks>


using System;
using UnityEngine;


namespace Custom.Mechanics
{
    public class DashEventArgs : EventArgs
    {
        public float force;
        public Vector3 direction;
    }

    [RequireComponent(typeof(Rigidbody2D))]
    public class Dash : Mechanics<DashEventArgs>
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

        protected Vector3 direction;
        private Rigidbody2D body;



        protected virtual void Awake() =>
            body = GetComponent<Rigidbody2D>();

        #region Mechanics Overrides
        // Overloads default trigger to add custom parameters
        public virtual void Trigger(Vector3 direction)
        {
            this.direction = direction;
            base.Trigger();
        }

        protected override void Handle()
        {
            if (direction == Vector3.zero)
                return;

            direction = ModifyDirection(direction);
            body.MovePosition(transform.position + direction * force);
            base.Handle();
        }
        #endregion

        #region Methods
        private Vector3 ModifyDirection(Vector3 direction)
        {
            if (direction.magnitude > minMagnitude)
                return direction;
            else 
                return direction * minMagnitude / direction.magnitude;
        }

        protected override DashEventArgs GenerateEventArgs()
        {
            return new DashEventArgs { 
                direction = this.direction, 
                force = this.force 
            };
        }
        #endregion
    }
}
