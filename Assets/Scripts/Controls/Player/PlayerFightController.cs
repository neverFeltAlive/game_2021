using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

using Custom.Mechanics;

namespace Custom.Controlls
{
    /// <summary>
    /// 
    /// PlayerFightController is used for switching between meele and shooting mode using player input
    /// :NeverFeltAlive
    /// 
    /// </summary>
    public class PlayerFightController : MonoBehaviour
    {
        public enum AttackState
        {
            Meele,
            Shooting
        }



        public event EventHandler<OnAttackStateChangedEventArgs> OnStateChanged;
        public class OnAttackStateChangedEventArgs : EventArgs
        {
            public AttackState state;
        }



        [SerializeField] private GameObject crosshair;

        private PowerMeleeAttack attack;
        private ShootProjectiles shoot;

        private AttackState _state;

        public AttackState State { get { return _state; } }



        private void Awake()
        {
            _state = AttackState.Meele;
            attack = GetComponent<PowerMeleeAttack>();
            shoot = GetComponent<ShootProjectiles>();
            crosshair.SetActive(false);
        }

        private void Update()
        {
            if (_state == AttackState.Shooting)
            {
                Vector3 movementDirection = PlayerController.Instance.playerControls.MainControls.Walk.ReadValue<Vector2>();
                Vector3 aimDirection = PlayerController.Instance.playerControls.MainControls.Aim.ReadValue<Vector2>();

                if (movementDirection.magnitude == 0)
                {
                    crosshair.SetActive(true);
                    float crosshairDistance = .2f;
                    crosshair.transform.position = transform.position + aimDirection.normalized * crosshairDistance;
                }
                else
                    crosshair.SetActive(false);
            }
        }

        public void Shoot(InputAction.CallbackContext context)
        {
            if (_state == AttackState.Shooting)
            {
                if (context.canceled)
                {
                    Vector3 walkDirection = PlayerController.Instance.playerControls.MainControls.Walk.ReadValue<Vector2>();
                    if (walkDirection == Vector3.zero)
                    {
                        Vector3 aimDirection = PlayerController.Instance.playerControls.MainControls.Aim.ReadValue<Vector2>();
                        shoot.Shoot(aimDirection);
                    }
                }
            }
        }

        public void Attack(InputAction.CallbackContext context)
        {
            if (_state != AttackState.Meele)
                return;

            Vector3 direction = PlayerController.Instance.playerControls.MainControls.Walk.ReadValue<Vector2>();
            if (context.interaction is HoldInteraction)
            {
                if (context.performed)
                    attack.TriggerAttack(direction, true);
            }
            else
            {
                if (context.canceled)
                    attack.TriggerAttack(direction, false);
            }
        }

        public void ToggleShooting(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;

            if (_state == AttackState.Meele)
                _state = AttackState.Shooting;
            else if (_state == AttackState.Shooting)
                _state = AttackState.Meele;

            OnStateChanged?.Invoke(this, new OnAttackStateChangedEventArgs { state = _state });
        }
    }
}
