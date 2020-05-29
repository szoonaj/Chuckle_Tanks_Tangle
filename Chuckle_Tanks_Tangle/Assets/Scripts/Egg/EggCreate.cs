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

        private void Start()
        {
            chickenMovement = GetComponent<ChickenMovement>();
            InvokeRepeating("CreateEgg", 1f, 5f);
        }

        private void CreateEgg()
        {
            GameObject egg;
            egg = Instantiate(m_eggPrefab, new Vector3(chickenMovement.transform.position.x - chickenMovement.transform.forward.x * 3.5f, 2f, chickenMovement.transform.position.z - chickenMovement.transform.forward.z * 3.5f), chickenMovement.transform.rotation) as GameObject;
            egg.SetActive(true);
        }
    }
}

