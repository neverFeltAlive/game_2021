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
        [SerializeField] private float rollSpeed = 5f;

        private bool isRolling = false;
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
            float rollDrop = 3f;

            if (isRolling)
            {
                body.velocity = direction * currentSpeed;
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

        public void TriggerRoll(Vector3 direction)
        {
            if (!isRolling)
            {
                if (direction != Vector3.zero)
                {
                    OnRoll?.Invoke(this, EventArgs.Empty);

                    // Trigger rolling
                    currentSpeed = rollSpeed;
                    this.direction = direction;
                    movement.DisableMovement();
                    isRolling = true;
                }
            }
        }
    }
}
