using System;
using UnityEngine;

namespace Complete
{
    [Serializable]
    public class ChickenManager
    {
        public Transform m_SpawnPoint;                          // The position and direction the chicken will have when it spawns.
        [HideInInspector] public GameObject m_Instance;         // A reference to the instance of the chicken when it is created.

        private ChickenMovement m_Movement;                        // Reference to chicken's movement script, used to disable and enable control.
        private GameObject m_CanvasGameObject;                  // Used to disable the world space UI during the Starting and Ending phases of each round.


        public void Setup()
        {
            // Get references to the components.
            m_Movement = m_Instance.GetComponent<ChickenMovement>();
            m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas>().gameObject;
        }


        public void DisableControl()
        {
            m_Movement.enabled = false;

            m_CanvasGameObject.SetActive(false);
        }


        public void EnableControl()
        {
            m_Movement.enabled = true;

            m_CanvasGameObject.SetActive(true);
        }


        public void Reset()
        {
            m_Instance.transform.position = m_SpawnPoint.position;
            m_Instance.transform.rotation = m_SpawnPoint.rotation;

            m_Instance.SetActive(false);
            m_Instance.SetActive(true);
        }
    }
}