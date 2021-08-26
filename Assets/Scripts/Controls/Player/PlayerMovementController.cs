using UnityEngine;

using Custom.Mechanics;

namespace Custom.Controlls
{
    /// <summary>
    /// 
    /// PlayerMovementController is used for controlling character movement using player input
    /// :NeverFeltAlive
    /// 
    /// </summary>
    [RequireComponent(typeof(IMovement))]
    public class PlayerMovementController : MonoBehaviour
    {
        private IMovement movement;



        private void Awake() =>
            movement = GetComponent<IMovement>();

        private void Update() =>
            movement.Direction = PlayerController.Instance.playerControls.MainControls.Walk.ReadValue<Vector2>();
    }
}
