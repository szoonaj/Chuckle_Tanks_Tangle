using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Complete
{
    public class FoodManager : MonoBehaviour
    {

        public GameObject m_foodPrefab;
        // Start is called before the first frame update
        void Start()
        {
            InvokeRepeating("CreateFood", 1f, 0.5f);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private Vector3 DrawFoodSpawnPoints()
        {
            //set custom range for random position
            float MinX = -60f;
            float MaxX = 60f;
            float MinY = 20f;
            float MaxY = 60f;
            float MinZ = -60f;
            float MaxZ = 60f;

            float x = Random.Range(MinX, MaxX);
            float y = Random.Range(MinY, MaxY);
            float z = Random.Range(MinZ, MaxZ);

            return new Vector3(x, y, z);
        }

        private void CreateFood()
        {
            GameObject food;
            food = Instantiate(m_foodPrefab, DrawFoodSpawnPoints(), Quaternion.identity) as GameObject;
            food.SetActive(true);
        }
    }

}

