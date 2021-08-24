/// <remarks>
/// 
/// Roll is used for controlling roll mechanics.
/// It is based on changing movement speed over time
/// NeverFeltAlive
/// 
/// </remarks>


using System;
using UnityEngine;

namespace Custom.Mechanics
{
    [RequireComponent(typeof(IDisablableMovement<Vector3, float>))]
    public class Roll : Mechanics<EventArgs>
    /* DEBUG statements for this document 
     * 
     * Debug.Log("Roll --> Start: ");
     * Debug.Log("<size=13><i><b> Roll --> </b></i><color=yellow> FixedUpdate: </color></size>");
     * Debug.Log("<size=13><i><b> Roll --> </b></i><color=red> Update: </color></size>");
     * Debug.Log("<size=13><i><b> Roll --> </b></i><color=blue> Corutine: </color></size>");
     * Debug.Log("<size=13><i><b> Roll --> </b></i><color=green> Function: </color></size>");
     * 
     */
    {
        #region Fields
        [SerializeField] private float rollSpeed = 5f;
        [SerializeField] private float rollDrop = 3f;

        private bool isTriggered = false;

        private float currentSpeed;

        private Vector3 _direction;

        private IDisablableMovement<Vector3, float> movement;

        public Vector3 Direction { 
            set {
                if (value.magnitude > 0)
                    _direction = value;
            }
        }
        #endregion



        #region MonoBehaviour Callbacks
        protected virtual void Awake() =>
            movement = GetComponent<IDisablableMovement<Vector3, float>>();

        protected virtual void Update()
        {
            if (!isTriggered)
                Direction = movement.Direction;
        }

        protected virtual void FixedUpdate()
        {
            if (!isTriggered)
                return;

            movement.Move(_direction.normalized, currentSpeed);
            currentSpeed -= rollSpeed * rollDrop * Time.fixedDeltaTime;
            if (currentSpeed <= .5f)
            {
                movement.EnableMovement();
                isTriggered = false;
                base.Handle();
            }
        }
        #endregion 

        public override void Trigger()
        {
            if (isTriggered)
                return;

            base.Trigger();
        }

        protected override void Handle()
        {
            currentSpeed = rollSpeed;
            movement.DisableMovement();
            movement.Move(_direction, rollSpeed);
            isTriggered = true;
        }

        public bool IsRolling()
        {
            if (isTriggered)
                return true;
            else
                return false;
        }

        protected override EventArgs GenerateEventArgs()
        {
            return EventArgs.Empty;
        }
    }
}
