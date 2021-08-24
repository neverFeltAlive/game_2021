/// <remarks>
/// 
/// DashVisualsController is used for controlling dash animation
/// NeverFeltAlive
/// 
/// </remarks>


using System.Collections;
using UnityEngine;

using Custom.Mechanics;

namespace Custom.Visuals
{
    [RequireComponent(typeof(Dash))]
    public class DashVisualsController : MonoBehaviour
    /* DEBUG statements for this document 
     * 
     * Debug.Log("DashVisualsController --> Start: ");
     * Debug.Log("<size=13><i><b> DashVisualsController --> </b></i><color=yellow> FixedUpdate: </color></size>");
     * Debug.Log("<size=13><i><b> DashVisualsController --> </b></i><color=red> Update: </color></size>");
     * Debug.Log("<size=13><i><b> DashVisualsController --> </b></i><color=blue> Corutine: </color></size>");
     * Debug.Log("<size=13><i><b> DashVisualsController --> </b></i><color=green> Function: </color></size>");
     * 
     */
    /* TODO
     * 
     */
    {
        [SerializeField] private Material dashMaterial;
        [SerializeField] private Material defaultMaterial;
        [SerializeField] private SpriteRenderer spriteRenderer;

        private Dash dash;

        private IEnumerator dashRoutine;



        protected virtual void Awake() =>
            dash = GetComponent<Dash>();

        protected virtual void OnEnable() =>
            dash.OnTriggered += Handler;

        protected virtual void OnDisable() =>
            dash.OnTriggered -= Handler;

        private void Handler(object sender, DashEventArgs args)
        {
            if (dashRoutine == null)
            {
                dashRoutine = AnimateDashMaterial(args.direction);
                StartCoroutine(dashRoutine);
            }
        }

        IEnumerator AnimateDashMaterial(Vector3 direction)
        {
            int numberOfFrames = 6;
            float frameDuration = .05f;
            float blurAmount = 30f;
            float blurAmountDrop = 5f;

            spriteRenderer.material = dashMaterial;
            dashMaterial.SetVector("_Direction", direction.normalized);

            for (int i = 0; i < numberOfFrames; i++)
            {
                yield return new WaitForSeconds(frameDuration);
                dashMaterial.SetFloat("_BlurAmount", blurAmount - blurAmountDrop * i);
            }

            spriteRenderer.material = defaultMaterial;
            dashRoutine = null;
        }

    }
}
