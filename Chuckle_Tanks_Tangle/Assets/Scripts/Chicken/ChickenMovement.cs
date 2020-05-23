using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Complete
{
    public class ChickenMovement : MonoBehaviour
    {
        public Transform target;
        private TankMovement tankMovement;
        private Animator animator;
        private NavMeshAgent agent;
        private Vector3 destination;

        //private GameObject altTarget;

        private float rangeAttack = 25f;

        //private enum State
        //{
        //    Idle,
        //    Move,
        //}

        //private State state = State.Idle;
        //private bool attackRunning = false;


        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();

            tankMovement = GetComponent<TankMovement>();

           // altTarget = GameObject.FindWithTag("AltTarget");
            //if (target == null)
            //{
            //    target = FindObjectOfType<TankMovement>().transform;
            //}

        }

        private void Update()
        {
            // animator.SetFloat("Speed", agent.velocity.magnitude);
            //if (target == null)
            //{
            //    target = FindObjectOfType<TankMovement>().transform;
            //    //return;
            //}
            //agent.SetDestination(destination);

            //agent.destination = target.position;
            //if (target == null)
            //{
            //    target = FindObjectOfType<TankMovement>().transform;
            //}

            //target = FindObjectOfType<TankMovement>().transform;
            target = FindClosestEnemy().transform;
            UpdateState();

        }

        private bool IsTargetVisible()
        {
            Vector3 toTarget = target.position - transform.position;
            bool result = Physics.Raycast(transform.position, toTarget, out RaycastHit hitInfo, rangeAttack);
            return result && hitInfo.transform == target;
        }

        public void Move(Vector3 target)
        {
            //TODO
            agent.isStopped = false;
            agent.SetDestination(target);
            //NavMesh.SamplePosition
        }

        // dziala tak samo, ale te Idle, Move to jest wgl niepotrzebne
        /*
        private void UpdateState()
        {
            if (state == State.Idle)
            {
                //TODO
                if (IsTargetVisible())
                {
                    // to idz za czolgiem
                    agent.isStopped = false;
                    Move(target.position);
                    state = State.Move;
                }
                else
                {
                    agent.isStopped = true;
                    state = State.Idle;
                }
            }
            else if (state == State.Move)
            {
                //TODO
                if (IsTargetVisible())
                {
                    // to idz za czolgiem
                    agent.isStopped = false;
                    state = State.Move;
                    animator.SetBool("Move", true);
                    Move(target.position);
                }

                else
                {
                    agent.isStopped = true;
                    state = State.Idle;
                }
            }
        }
        */

        private void UpdateState()
        {
            animator.SetFloat("Speed", agent.velocity.magnitude);

            if (target != null && IsTargetVisible())
            {
                agent.isStopped = false;
                //animator.SetBool("IsVisible", true);
                //animator.SetFloat("Speed", 1f);
                //animator.SetFloat("Speed", agent.velocity.magnitude);
                //agent.speed = 3.5f;

                Move(target.position);                

            }
            else if (target != null && !IsTargetVisible())
            {
                //agent.isStopped = true;
                //animator.SetBool("IsVisible", false);
                //animator.SetFloat("Speed", 0f);
                //agent.velocity = Vector3.zero;

                agent.velocity = Vector3.zero;
                agent.isStopped = true;
                animator.SetFloat("Speed", 0f);

                //Move(altTarget.transform.position);
                //if (!agent.pathPending && agent.remainingDistance < 0.5f)
                //{
                //    agent.velocity = Vector3.zero;
                //    agent.isStopped = true;
                //    animator.SetFloat("Speed", 0f);
                //}
                //animator.SetFloat("Speed", agent.velocity.magnitude);
            }
            
        }

        private GameObject FindClosestEnemy()
        {
            GameObject[] gos;
            gos = GameObject.FindGameObjectsWithTag("TankTarget");
            GameObject closest = null;
            float distance = Mathf.Infinity;
            Vector3 position = transform.position;
            foreach (GameObject go in gos)
            {
                Vector3 diff = go.transform.position - position;
                float curDistance = diff.sqrMagnitude;
                if (curDistance < distance)
                {
                    closest = go;
                    distance = curDistance;
                }
            }
            return closest;
        }

        private void OnDrawGizmos()
        {

            // Draw a yellow sphere at the transform's position
            //Gizmos.color = Color.yellow;
            //Gizmos.DrawRay(transform.position, config.rangeAttack);

            Gizmos.color = Color.red;
            float theta = 0;
            float x = rangeAttack * Mathf.Cos(theta);
            float y = rangeAttack * Mathf.Sin(theta);
            Vector3 pos = transform.position + new Vector3(x, 0, y);
            Vector3 newPos = pos;
            Vector3 lastPos = pos;
            for (theta = 0.1f; theta < Mathf.PI * 2; theta += 0.1f)
            {
                x = rangeAttack * Mathf.Cos(theta);
                y = rangeAttack * Mathf.Sin(theta);
                newPos = transform.position + new Vector3(x, 0, y);
                Gizmos.DrawLine(pos, newPos);
                pos = newPos;
            }
            Gizmos.DrawLine(pos, lastPos);

        }
    }
}
