using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Custom.Mechanics;

namespace Custom.Controlls
{
    /// <summary>
    /// 
    /// EnemyMovementController is used for controlling movement with wimple Enemy AI
    /// :NeverFeltAlvie
    /// 
    /// </summary>
    [RequireComponent(typeof(IMovement))]
    public class EnemyMovementController : MonoBehaviour
    {
        public enum EnemyState
        {
            Roaming,
            Chase
        }



        #region Private Fields
        private int currentPathIndex;

        private float triggerRange = .5f;
        private float chaseRange = 1f;

        private EnemyState state;

        private Vector3 direction;
        private Vector3 startingPosition;
        private Vector3 roamingPosition;

        private Rigidbody2D body;
        private IMovement movement;

        private List<Vector3> pathVectorsList;
        #endregion

        #region DEBUG
        /// <remarks>
        /// Set to true to turn visual debug on
        /// </remarks>
        private bool showDebug = true;
        #endregion



        #region MonoBehaviour Callbacks
        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            movement = GetComponent<IMovement>();

            if (Pathfinding.Instance == null)
                new Pathfinding(40, 40, new Vector3(-2.5f, -2.5f, 0), .125f);
        }

        private void Start()
        {
            startingPosition = transform.position;
            roamingPosition = GetRoamingPosition();
            state = EnemyState.Roaming;

            SetPath(roamingPosition);
        }

        private void Update()
        {
            switch (state)
            {
                case EnemyState.Roaming:
                    if (pathVectorsList == null)
                        roamingPosition = GetRoamingPosition();

                    SetPath(roamingPosition);
                    LocateTarget();
                    break;

                case EnemyState.Chase:
                    if (Vector3.Distance(transform.position, PlayerController.Instance.Position) < chaseRange)
                        SetPath(PlayerController.Instance.Position);
                    else
                    {
                        state = EnemyState.Roaming;
                        pathVectorsList = null;
                    }
                    break;

            }
            SetDirection();
        }
        #endregion

        #region Functions
        private void SetDirection()
        {
            if (pathVectorsList == null)
                return;

            Vector3 targetPosition = pathVectorsList[currentPathIndex];
            if (Vector3.Distance(transform.position, targetPosition) < .01f)
            {
                currentPathIndex++;
                if (currentPathIndex >= pathVectorsList.Count)
                {
                    pathVectorsList = null;
                    direction = Vector3.zero;
                    return;
                }
            }

            direction = (pathVectorsList[currentPathIndex] - transform.position).normalized;
            movement.Direction = direction;
        }

        private void DebugPath()
        {
            if (pathVectorsList != null)
            {
                for (int i = 0; i < pathVectorsList.Count - 1; i++)
                {
                    if (i == 0)
                    {
                        Debug.DrawLine(transform.position, pathVectorsList[i + 1], Color.black);
                        continue;
                    }

                    Debug.DrawLine(pathVectorsList[i], pathVectorsList[i + 1], Color.black);
                }
            }
        }

        private void LocateTarget()
        {
            if (Vector3.Distance(transform.position, PlayerController.Instance.Position) < triggerRange)
                state = EnemyState.Chase;
        }

        public void SetPath(Vector3 targetPosition)
        {
            FindPath(targetPosition);

            if (pathVectorsList == null)
                FindPath(startingPosition);
        }

        public void FindPath(Vector3 targetPosition)
        {
            currentPathIndex = 0;
            pathVectorsList = Pathfinding.Instance.FindPath(transform.position, targetPosition);

            #region DEBUG
            if (showDebug) DebugPath();
            #endregion

            if (pathVectorsList != null && pathVectorsList.Count > 1)
                pathVectorsList.RemoveAt(0);
        }
        #endregion

        private Vector3 GetRoamingPosition()
        {
            return startingPosition + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * Random.Range(0f, 1f);
        }
    }
}
