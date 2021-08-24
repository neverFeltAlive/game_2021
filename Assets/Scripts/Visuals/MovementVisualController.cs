/// <remarks>
/// 
/// MovementVisualController is used for controlling movement animation
/// NeverFeltAlive
/// 
/// </remarks>


using UnityEngine;

using Custom.Mechanics;

namespace Custom.Visuals
{
    [RequireComponent(typeof(IMovement<Vector3, float>))]
    public class MovementVisualController : MonoBehaviour
    /* DEBUG statements for this document 
     * 
     * Debug.Log("MovementVisualController --> Start: ");
     * Debug.Log("<size=13><i><b> MovementVisualController --> </b></i><color=yellow> FixedUpdate: </color></size>");
     * Debug.Log("<size=13><i><b> MovementVisualController --> </b></i><color=red> Update: </color></size>");
     * Debug.Log("<size=13><i><b> MovementVisualController --> </b></i><color=blue> Corutine: </color></size>");
     * Debug.Log("<size=13><i><b> MovementVisualController --> </b></i><color=green> Function: </color></size>");
     * 
     */
    /* TODO
     * 
     */
    {
        [SerializeField] private Animator animator;

        private IMovement<Vector3, float> movement;



        private void Awake() =>
            movement = GetComponent<IMovement<Vector3, float>>();

        private void Update()
        {
            animator.SetFloat("Magnitude", movement.Direction.magnitude);
            animator.SetFloat("Horizontal", movement.Direction.x);
            animator.SetFloat("Vertical", movement.Direction.y);
        }
    }
}
