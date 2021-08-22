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
    [RequireComponent(typeof(IDisablableMovement))]
    public class Roll : MonoBehaviour
    /* DEBUG statements for this document 
     * 
     * Debug.Log("Roll --> Start: ");
     * Debug.Log("<size=13><i><b> Roll --> </b></i><color=yellow> FixedUpdate: </color></size>");
     * Debug.Log("<size=13><i><b> Roll --> </b></i><color=red> Update: </color></size>");
     * Debug.Log("<size=13><i><b> Roll --> </b></i><color=blue> Corutine: </color></size>");
     * Debug.Log("<size=13><i><b> Roll --> </b></i><color=green> Function: </color></size>");
     * 
     */
    /* TODO
     * 
     */
    {
        public static event EventHandler OnRoll;

        #region Fields
        [SerializeField] private float rollSpeed = 5f;
        [SerializeField] private float rollDrop = 3f;

        private bool isTriggered = false;

        private float currentSpeed;

        private Vector3 _direction;

        private IDisablableMovement movement;

        public Vector3 Direction { 
            set {
                if (value.magnitude > 0)
                    _direction = value;
            }
        }
        #endregion



        #region MonoBehaviour Callbacks
        private void Awake() =>
            movement = GetComponent<IDisablableMovement>();

        private void Update()
        {
            if (!isTriggered)
                Direction = movement.Direction;
        }

        private void FixedUpdate()
        {
            if (!isTriggered)
                return;

            movement.Move(_direction.normalized, currentSpeed);
            currentSpeed -= rollSpeed * rollDrop * Time.fixedDeltaTime;
            if (currentSpeed <= .5f)
            {
                movement.EnableMovement();
                isTriggered = false;
            }
        }
        #endregion 

        public void TriggerRoll()
        {
            if (isTriggered)
                return;

            OnRoll?.Invoke(this, EventArgs.Empty);

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
    }
}
