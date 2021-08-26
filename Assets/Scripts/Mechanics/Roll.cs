using System;
using UnityEngine;

namespace Custom.Mechanics
{
    /// <summary>
    /// 
    /// Roll is used for implementing roll mechanics.
    /// It is based on changing movement speed over time
    /// :NeverFeltAlive
    /// 
    /// </summary>
    [RequireComponent(typeof(IDisablableMovement))]
    public class Roll : MonoBehaviour
    {
        public event EventHandler OnRoll;



        #region Fields
        [SerializeField] private float rollSpeed = 5f;

        private bool isRolling = false;
        private float currentSpeed;

        private Vector3 direction;
        private IDisablableMovement movement;
        #endregion



        #region MonoBehaviour Callbacks
        private void Awake() =>
            movement = GetComponent<IDisablableMovement>();

        private void Update()
        {
            if (!isRolling)
                SetDirection(movement.Direction);
        }

        private void FixedUpdate()
        {
            float rollDrop = 3f;

            if (isRolling)
            {
                movement.Move(direction.normalized, currentSpeed);
                currentSpeed -= rollSpeed * rollDrop * Time.fixedDeltaTime;

                // Stop rolling 
                if (currentSpeed <= .5f)
                {
                    movement.EnableMovement();
                    isRolling = false;
                }
            }
        }
        #endregion 
        
        private void SetDirection(Vector3 direction)
        {
            if (direction.magnitude > 0)
                this.direction = direction;
        }

        public void PerformRoll()
        {
            if (!isRolling)
            {
                OnRoll?.Invoke(this, EventArgs.Empty);

                // Trigger rolling
                currentSpeed = rollSpeed;
                movement.DisableMovement();
                movement.Move(direction, rollSpeed);
                isRolling = true;
            }
        }
    }
}
