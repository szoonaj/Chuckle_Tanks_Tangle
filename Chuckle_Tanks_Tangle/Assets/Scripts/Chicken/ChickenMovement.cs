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

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();

            tankMovement = GetComponent<TankMovement>();

            if (target == null)
            {
                target = FindObjectOfType<TankMovement>().transform;
            }

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
            agent.destination = target.position;
        }

        
    }
}
