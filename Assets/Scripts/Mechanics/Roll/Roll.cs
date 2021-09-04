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
    [RequireComponent(typeof(Rigidbody2D))]
    public class Roll : MonoBehaviour
    {
        public event EventHandler OnRoll;



        #region Fields
        [SerializeField] private RollStats rollStats;

        private bool isRolling = false;

        private const float ROLL_DROP = 3f;
        private float currentSpeed;

        private Vector3 direction;

        private Rigidbody2D body;
        private IDisablableMovement movement;
        #endregion



        #region MonoBehaviour Callbacks
        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            movement = GetComponent<IDisablableMovement>();
        }

        private void FixedUpdate()
        {
            if (isRolling)
            {
                body.velocity = direction * currentSpeed;
                currentSpeed -= rollStats.rollSpeed * ROLL_DROP * Time.fixedDeltaTime;

                // Stop rolling 
                float stopSpeed = .05f;
                if (currentSpeed <= stopSpeed)
                {
                    movement.EnableMovement();
                    isRolling = false;
                }
            }
        }
        #endregion

        public void TriggerRoll(Vector3 direction)
        {
            if (!isRolling)
            {
                if (direction != Vector3.zero)
                {
                    OnRoll?.Invoke(this, EventArgs.Empty);

                    // Trigger rolling
                    currentSpeed = rollStats.rollSpeed;
                    this.direction = direction.normalized;
                    movement.DisableMovement();
                    isRolling = true;
                }
            }
        }
    }
}
