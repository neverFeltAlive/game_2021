using System;
using System.Collections;
using UnityEngine;
using Cinemachine;

using Custom.Mechanics;
using Custom.Controlls;
using Custom.Utils;

namespace Custom.Visuals
{
    /// <summary>
    /// 
    /// PlayerVisualController is used for controlling movement animation
    /// :NeverFeltAlive
    /// 
    /// </summary>
    [RequireComponent(typeof(IMovement))]
    public class PlayerVisualController : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Animator animator;
        [Space]
        [SerializeField] private CinemachineVirtualCamera virtualCam;
        [Space]
        [SerializeField] private Material trackMaterial;
        [SerializeField] private Material dashMaterial;
        [SerializeField] private Material defaultMaterial;
        [SerializeField] private Material glowMaterial;
        [Space]
        [SerializeField] private RuntimeAnimatorController defaultAnimationController;
        [SerializeField] private RuntimeAnimatorController shootingAnimatorController;
        #endregion

        #region Private Fields
        private Roll roll;
        private Track track;
        private Dash dash;
        private ShootProjectiles shoot;
        private VectorMeleeAttack attack;
        private PlayerFightController fightController;

        private IMovement movement;
        private IEnumerator trackRoutine;
        private IEnumerator dashRoutine;
        #endregion



        #region MonoBehaviour Callbacks
        private void Awake()
        {
            movement = GetComponent<IMovement>();
            roll = GetComponent<Roll>();
            track = GetComponent<Track>();
            dash = GetComponent<Dash>();
            shoot = GetComponent<ShootProjectiles>();
            attack = GetComponent<VectorMeleeAttack>();
            fightController = GetComponent<PlayerFightController>();
        }

        private void OnEnable()
        {
            roll.OnRoll += RollHandler;
            track.OnTrack += TrackHandler;
            dash.OnDash += DashHandler;
            shoot.OnShoot += AttackHandler;
            attack.OnAttack += AttackHandler;
            fightController.OnStateChanged += AttackStateChangeHandler;
        }

        private void OnDisable()
        {
            roll.OnRoll -= RollHandler;
            track.OnTrack -= TrackHandler;
            dash.OnDash -= DashHandler;
            shoot.OnShoot -= AttackHandler;
            attack.OnAttack -= AttackHandler;
            fightController.OnStateChanged -= AttackStateChangeHandler;
        }

        private void Update()
        {
            Vector3 direction = PlayerController.Instance.playerControls.MainControls.Walk.ReadValue<Vector2>(); ;
            if (direction.magnitude == 0 && fightController.State == PlayerFightController.AttackState.Shooting)
                direction = PlayerController.Instance.playerControls.MainControls.Aim.ReadValue<Vector2>();

            animator.SetFloat("Magnitude", direction.magnitude);
            animator.SetFloat("Horizontal", direction.x);
            animator.SetFloat("Vertical", direction.y);
        }
        #endregion

        #region Event Handlers
        private void RollHandler(object sender, EventArgs args) =>
            animator.SetTrigger("Roll");

        private void TrackHandler(object sender, EventArgs args)
        {
            if (trackRoutine == null)
            {
                trackRoutine = AnimateTrackMaterial();
                StartCoroutine(trackRoutine);
            }
        }

        private void DashHandler(object sender, Dash.OnDashEventArgs args)
        {
            if (dashRoutine == null)
            {
                dashRoutine = AnimateDashMaterial(args.direction);
                StartCoroutine(dashRoutine);
            }
        }

        private void AttackHandler(object sender, EventArgs args) =>
            animator.SetTrigger("Attack");

        private void AttackStateChangeHandler(object sender, PlayerFightController.OnAttackStateChangedEventArgs args)
        {
            if (args.state == PlayerFightController.AttackState.Meele)
                animator.runtimeAnimatorController = defaultAnimationController;
            else 
                animator.runtimeAnimatorController = shootingAnimatorController;

            StartCoroutine(AnimateGlowMaterial(1));
        }
        #endregion



        #region Coroutines
        IEnumerator AnimateGlowMaterial(float time = 2f)
        {
            spriteRenderer.material = glowMaterial;
            yield return new WaitForSeconds(time);
            spriteRenderer.material = defaultMaterial;
        }

        IEnumerator AnimateDashMaterial(Vector3 direction)
        {
            float camShakeAmplitude = 1f;
            float camShakeFrequency = 2f;
            int numberOfFrames = 6;
            float frameDuration = .05f;
            float blurAmount = 30f;
            float blurAmountDrop = 5f;

            if (virtualCam != null)
                UtilsClass.StartCamShake(virtualCam, camShakeAmplitude, camShakeFrequency);

            spriteRenderer.material = dashMaterial;
            dashMaterial.SetVector("_Direction", direction.normalized);

            for (int i = 0; i < numberOfFrames; i++)
            {
                yield return new WaitForSeconds(frameDuration);
                dashMaterial.SetFloat("_BlurAmount", blurAmount - blurAmountDrop * i);
            }

            if (virtualCam != null)
                UtilsClass.StopCamShake(virtualCam);

            spriteRenderer.material = defaultMaterial;
            dashRoutine = null;
        }

        IEnumerator AnimateTrackMaterial()
        {
            int numberOfFrames = 3;
            float castingTime = .3f;
            float maxBlurAmount = 9f;
            float minBlurAmount = .5f;

            spriteRenderer.material = trackMaterial;

            for (int i = 0; i < numberOfFrames; i++)
            {
                yield return new WaitForSeconds(castingTime / numberOfFrames);
                trackMaterial.SetFloat("_BlurAmount", UnityEngine.Random.Range(minBlurAmount, maxBlurAmount));
            }

            spriteRenderer.material = defaultMaterial;
            trackMaterial = null;
        }
        #endregion
    }
}
