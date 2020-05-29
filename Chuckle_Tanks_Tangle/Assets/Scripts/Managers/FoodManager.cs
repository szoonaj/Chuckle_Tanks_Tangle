using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Complete
{
    public class FoodManager : MonoBehaviour
    {

        public float m_timeInterval = 0.5f;
        public GameObject m_foodPrefab;

        void Start()
        {
            InvokeRepeating("CreateFood", 1f, m_timeInterval);
        }

        private Vector3 DrawFoodSpawnPoints()
        {
            //set custom range for random position
            float MinX = -50f;
            float MaxX = 50f;
            float MinY = 20f;
            float MaxY = 60f;
            float MinZ = -50f;
            float MaxZ = 50f;

            float x = Random.Range(MinX, MaxX);
            float y = Random.Range(MinY, MaxY);
            float z = Random.Range(MinZ, MaxZ);

            return new Vector3(x, y, z);
        }

        private void CreateFood()
        {
            GameObject food;
            food = Instantiate(m_foodPrefab, DrawFoodSpawnPoints(), Quaternion.AngleAxis(90f, DrawFoodSpawnPoints())) as GameObject;
            food.SetActive(true);
        }
    }

}

