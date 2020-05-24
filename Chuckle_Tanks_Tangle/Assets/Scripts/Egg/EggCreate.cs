using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Complete
{
    public class EggCreate : MonoBehaviour
    {

        public ChickenMovement chickenMovement;
        public GameObject m_eggPrefab;

        //Start is called before the first frame update
        private void Start()
        {
            chickenMovement = GetComponent<ChickenMovement>();
            //StartCoroutine(CreateEgg());
            InvokeRepeating("CreateEgg", 1f, 5f);
            //CreateEgg();
        }

        // Update is called once per frame
        private void Update()
        {
            //StartCoroutine(CreateEgg());
            //chickenMovement = GetComponent<ChickenMovement>();
            //InvokeRepeating("CreateEgg", 1f, 5f);

        }

        private void CreateEgg()
        {
            //Rigidbody eggInstance;
            //eggInstance = Instantiate(m_eggRigidbody, chickenMovement.transform.position, chickenMovement.transform.rotation) as Rigidbody;
            //Rigidbody rb = egg.GetComponent<Rigidbody>();
            //yield return new WaitForSeconds(5f);
            //Rigidbody clone;
            //chickenMovement = GetComponent<ChickenMovement>();
            GameObject egg;
            egg = Instantiate(m_eggPrefab, new Vector3(chickenMovement.transform.position.x - chickenMovement.transform.forward.x * 3.5f, 2f, chickenMovement.transform.position.z - chickenMovement.transform.forward.z * 3.5f), chickenMovement.transform.rotation) as GameObject;
            egg.SetActive(true);
            //clone = Instantiate(m_eggPrefab, chickenMovement.transform.position, chickenMovement.transform.rotation);
            Debug.Log("egg created");
            //yield return null;
        }
    }
}

