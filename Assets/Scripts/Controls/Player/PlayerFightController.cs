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



        #region Fields
        [SerializeField] private int maxAmmo = 5;
        [SerializeField] private GameObject crosshair;

        private int currentAmmo;
        private float ammoRestoreTimer = 3f;

        private Vector3 movementDirection;
        private Vector3 attackDirection;
        private Vector3 shootDirection;

        private IPowerMeeleAttack attack;
        private IRangeAttack shoot;

        private AttackState _state;
        #endregion

        public AttackState State { get { return _state; } }



        private void Awake()
        {
            _state = AttackState.Meele;
            attack = GetComponent<IPowerMeeleAttack>();
            shoot = GetComponent<IRangeAttack>();
            currentAmmo = maxAmmo;
            crosshair.SetActive(false);
        }

        private void Update()
        {
            if (currentAmmo != maxAmmo)
            {
                if (ammoRestoreTimer - Time.deltaTime > 0)
                    ammoRestoreTimer -= Time.deltaTime;
                else
                {
                    currentAmmo++;
                    ammoRestoreTimer = 3f;
                }
                
            }

            movementDirection = PlayerController.Instance.playerControls.MainControls.Walk.ReadValue<Vector2>();

            if (_state == AttackState.Shooting)
            {
                shootDirection = PlayerController.Instance.playerControls.MainControls.Aim.ReadValue<Vector2>();

                if (movementDirection.magnitude == 0)
                {
                    crosshair.SetActive(true);
                    float crosshairDistance = .2f;
                    crosshair.transform.position = transform.position + shootDirection.normalized * crosshairDistance;
                }
                else
                    crosshair.SetActive(false);
            }
            else
            {
                if (movementDirection != Vector3.zero)
                    attackDirection = movementDirection;
            }
        }

        public void Shoot(InputAction.CallbackContext context)
        {
            if (_state == AttackState.Shooting)
            {
                if (context.canceled)
                {
                    if (movementDirection == Vector3.zero)
                    {
                        if (currentAmmo > 0)
                        {
                            Vector3 aimDirection = PlayerController.Instance.playerControls.MainControls.Aim.ReadValue<Vector2>();
                            shoot.Shoot(aimDirection);
                            currentAmmo--;
                        }
                    }
                }
            }
        }

        public void Attack(InputAction.CallbackContext context)
        {
            if (_state != AttackState.Meele)
                return;

            if (context.interaction is HoldInteraction)
            {
                if (context.performed)
                    attack.TriggerAttack(attackDirection, true);
            }
            else
            {
                if (context.canceled)
                    attack.TriggerAttack(attackDirection, false);
            }
        }

        public void ToggleShooting(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;

            crosshair.SetActive(false);

            if (_state == AttackState.Meele)
                _state = AttackState.Shooting;
            else if (_state == AttackState.Shooting)
                _state = AttackState.Meele;

            OnStateChanged?.Invoke(this, new OnAttackStateChangedEventArgs { state = _state });
        }
    }
}
