using UnityEngine;

namespace Custom.Controlls
{
    /// <summary>
    /// 
    /// PlayerController is used for controlling major player actions such as input.
    /// It is also responsible for correlations between separate parts of mechanics
    /// :NeverFeltAlive
    /// 
    /// </summary>
    public class PlayerController : MonoBehaviour
    /* DEBUG statements for this document 
     * 
     * Debug.Log("PlayerController --> Start: ");
     * Debug.Log("<size=13><i><b> PlayerController --> </b></i><color=yellow> FixedUpdate: </color></size>");
     * Debug.Log("<size=13><i><b> PlayerController --> </b></i><color=red> Update: </color></size>");
     * Debug.Log("<size=13><i><b> PlayerController --> </b></i><color=blue> Corutine: </color></size>");
     * Debug.Log("<size=13><i><b> PlayerController --> </b></i><color=green> Function: </color></size>");
     * 
     */
    {
        [HideInInspector] public PlayerControls playerControls;

        public Vector3 Position { get { return transform.position; } }
        public static PlayerController Instance { get; private set; }



        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            playerControls = new PlayerControls();
            playerControls.MainControls.Enable();
        }
    }
}
