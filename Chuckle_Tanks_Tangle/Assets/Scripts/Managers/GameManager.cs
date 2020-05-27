using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Complete
{
    public class GameManager : MonoBehaviour
    {
        public int m_NumRoundsToWin = 5;            // The number of rounds a single player has to win to win the game.
        public float m_StartDelay = 3f;             // The delay between the start of RoundStarting and RoundPlaying phases.
        public float m_EndDelay = 3f;               // The delay between the end of RoundPlaying and RoundEnding phases.
        public CameraControl m_CameraControl;       // Reference to the CameraControl script for control during different phases.
        public Text m_MessageText;                  // Reference to the overlay Text to display winning text, etc.
        public GameObject m_TankPrefab;             // Reference to the prefab the players will control.
        public GameObject m_ChickenPrefab;

        public TankManager[] m_Tanks;               // A collection of managers for enabling and disabling different aspects of the tanks.
        public ChickenManager[] m_Chickens;

        
        private int m_RoundNumber;                  // Which round the game is currently on.
        private WaitForSeconds m_StartWait;         // Used to have a delay whilst the round starts.
        private WaitForSeconds m_EndWait;           // Used to have a delay whilst the round or game ends.
        private TankManager m_RoundWinner;          // Reference to the winner of the current round.  Used to make an announcement of who won.
        private TankManager m_GameWinner;           // Reference to the winner of the game.  Used to make an announcement of who won.


        private void Start()
        {
            // Create the delays so they only have to be made once.
            m_StartWait = new WaitForSeconds (m_StartDelay);
            m_EndWait = new WaitForSeconds (m_EndDelay);

            //SceneManager.LoadScene("Menu");

            DrawChickenSpawnPoints();
            //DrawChickenAgentPriority();
            SpawnAllTanks();
            SetCameraTargets();
            //SpawnChicken();
            //SpawnAllChickens();
            // Start the game.
            StartCoroutine(GameLoop ());
        }


        private void SpawnAllTanks()
        {
            // For all the tanks...
            for (int i = 0; i < m_Tanks.Length; i++)
            {
                // ... create them, set their player number and references needed for control.
                m_Tanks[i].m_Instance =
                    Instantiate(m_TankPrefab, m_Tanks[i].m_SpawnPoint.position, m_Tanks[i].m_SpawnPoint.rotation) as GameObject;
                m_Tanks[i].m_PlayerNumber = i + 1;
                m_Tanks[i].Setup();
            }
        }

        private void SpawnChicken()
        {            
                // ... create them, set their player number and references needed for control.
                m_Chickens[0].m_Instance =
                    Instantiate(m_ChickenPrefab, m_Chickens[0].m_SpawnPoint.position, m_Chickens[0].m_SpawnPoint.rotation) as GameObject;
                m_Chickens[0].Setup();
        }

        private void SpawnAllChickens()
        {
            // ... create them, set their player number and references needed for control.
            for (int i = 0; i < m_Chickens.Length; i++)
            {
                // ... create them, set their player number and references needed for control.
                m_Chickens[i].m_Instance =
                    Instantiate(m_ChickenPrefab, m_Chickens[i].m_SpawnPoint.position, m_Chickens[i].m_SpawnPoint.rotation) as GameObject;
                m_Chickens[i].Setup();
            }
        }

        private void DrawChickenSpawnPoints()
        {
            for (int i = 0; i < m_Chickens.Length; i++)
            {
                //set custom range for random position
                float MinX = -20f;
                float MaxX = 20f;
                float MinY = 0f;
                float MaxY = 0f;
                float MinZ = -20f;
                float MaxZ = 20f;

                float x = Random.Range(MinX, MaxX);
                float y = Random.Range(MinY, MaxY);
                float z = Random.Range(MinZ, MaxZ);

                m_Chickens[i].m_SpawnPoint.position = new Vector3(x, y, z);                
            }

            //Instantiate(spawnObj, new Vector3(x, y, z), Quaternion.identity) as GameObject;
        }
        // nie dziala
        /*
        private void DrawChickenAgentPriority()
        {
            for (int i = 0; i < m_Chickens.Length; i++)
            {
                int priority = 50;
                priority = (int)Random.Range(0, 99);

                m_Chickens[i].m_Movement.agent.avoidancePriority = priority;
            }  
        }
        */
        private void SetCameraTargets()
        {
            // Create a collection of transforms the same size as the number of tanks.
            Transform[] targets = new Transform[m_Tanks.Length];

            // For each of these transforms...
            for (int i = 0; i < targets.Length; i++)
            {
                // ... set it to the appropriate tank transform.
                targets[i] = m_Tanks[i].m_Instance.transform;
            }

            m_CameraControl.m_Targets = targets;
        }


        private IEnumerator GameLoop ()
        {
            
            // Start off by running the 'RoundStarting' coroutine but don't return until it's finished.
            yield return StartCoroutine (RoundStarting ());

            // Once the 'RoundStarting' coroutine is finished, run the 'RoundPlaying' coroutine but don't return until it's finished.
            yield return StartCoroutine (RoundPlaying());

            // Once execution has returned here, run the 'RoundEnding' coroutine, again don't return until it's finished.
            yield return StartCoroutine (RoundEnding());

            //SceneManager.LoadScene("Level2");

            // This code is not run until 'RoundEnding' has finished.  At which point, check if a game winner has been found.
            if (m_GameWinner != null)
            {
                // If there is a game winner, restart the level.
                SceneManager.LoadScene ("GameOver");
            }
            else
            {
                // If there isn't a winner yet, restart this coroutine so the loop continues.
                // Note that this coroutine doesn't yield.  This means that the current version of the GameLoop will end.
                //if (sceneNumber % 3 == 1)
                //{
                //    SceneManager.LoadScene("Level1");
                //    sceneNumber++;
                //}
                    

                //else if (sceneNumber % 3 == 2)
                //{
                //    SceneManager.LoadScene("Level2");
                //    sceneNumber++;
                //}
                //else
                //{
                //    SceneManager.LoadScene("Level3");
                //    sceneNumber++;
                //}

                StartCoroutine (GameLoop ());
            }
        }


        private IEnumerator RoundStarting ()
        {
            DrawChickenSpawnPoints();
            SpawnAllChickens();
            ResetAllTanks();
            //ResetAllChickens();
            //DestroyAllChickens();
            DisableTankControl ();
            DisableChickenControl();

            // Snap the camera's zoom and position to something appropriate for the reset tanks.
            m_CameraControl.SetStartPositionAndSize ();

            m_RoundNumber++;
            m_MessageText.text = "ROUND " + m_RoundNumber;

            // Wait for the specified length of time until yielding control back to the game loop.
            yield return m_StartWait;
        }


        private IEnumerator RoundPlaying ()
        {
            EnableTankControl ();
            EnableChickenControl();

            m_MessageText.text = string.Empty;

            // While there is not one tank left...
            while (!OneTankLeft())
            {
                // ... return on the next frame.
                yield return null;
            }
        }


        private IEnumerator RoundEnding ()
        {
            DisableTankControl ();
            //DisableChickenControl();
            DestroyAllChickens();
            DestroyEggs();
            DestroyFood();
            // Clear the winner from the previous round.
            m_RoundWinner = null;

            // See if there is a winner now the round is over.
            m_RoundWinner = GetRoundWinner ();

            // If there is a winner, increment their score.
            if (m_RoundWinner != null)
                m_RoundWinner.m_Wins++;

            m_GameWinner = GetGameWinner ();

            string message = EndMessage ();
            m_MessageText.text = message;

            // Wait for the specified length of time until yielding control back to the game loop.
            yield return m_EndWait;
        }


        // This is used to check if there is one or fewer tanks remaining and thus the round should end.
        private bool OneTankLeft()
        {
            // Start the count of tanks left at zero.
            int numTanksLeft = 0;

            // Go through all the tanks...
            for (int i = 0; i < m_Tanks.Length; i++)
            {
                // ... and if they are active, increment the counter.
                if (m_Tanks[i].m_Instance.activeSelf)
                    numTanksLeft++;
            }

            // If there are one or fewer tanks remaining return true, otherwise return false.
            return numTanksLeft <= 1;
        }
        
        
        // This function is called with the assumption that 1 or fewer tanks are currently active.
        private TankManager GetRoundWinner()
        {
            // Go through all the tanks...
            for (int i = 0; i < m_Tanks.Length; i++)
            {
                // ... and if one of them is active, it is the winner so return it.
                if (m_Tanks[i].m_Instance.activeSelf)
                    return m_Tanks[i];
            }

            // If none of the tanks are active it is a draw so return null.
            return null;
        }


        private TankManager GetGameWinner()
        {
            // Go through all the tanks...
            for (int i = 0; i < m_Tanks.Length; i++)
            {
                // ... and if one of them has enough rounds to win the game, return it.
                if (m_Tanks[i].m_Wins == m_NumRoundsToWin)
                    return m_Tanks[i];
            }

            // If no tanks have enough rounds to win, return null.
            return null;
        }


        // Returns a string message to display at the end of each round.
        private string EndMessage()
        {
            string message = "DRAW!";

            // If there is a winner then change the message to reflect that.
            if (m_RoundWinner != null)
                message = m_RoundWinner.m_ColoredPlayerText + " WINS THE ROUND!";

            // Add some line breaks after the initial message.
            message += "\n\n\n\n";

            // Go through all the tanks and add each of their scores to the message.
            for (int i = 0; i < m_Tanks.Length; i++)
            {
                message += m_Tanks[i].m_ColoredPlayerText + ": " + m_Tanks[i].m_Wins + " WINS\n";
            }

            // If there is a game winner, change the entire message to reflect that.
            if (m_GameWinner != null)
                message = m_GameWinner.m_ColoredPlayerText + " WINS THE GAME!";

            return message;
        }


        private void ResetAllTanks()
        {
            for (int i = 0; i < m_Tanks.Length; i++)
            {
                m_Tanks[i].Reset();
            }

            //m_Chickens[0].Reset();
        }

        private void ResetAllChickens()
        {
            for (int i = 0; i < m_Chickens.Length; i++)
            {
                m_Chickens[i].Reset();
            }

            //m_Chickens[0].Reset();
        }



        private void EnableTankControl()
        {
            for (int i = 0; i < m_Tanks.Length; i++)
            {
                m_Tanks[i].EnableControl();
            }

           //m_Chickens[0].EnableControl();
        }

        private void EnableChickenControl()
        {
            for (int i = 0; i < m_Chickens.Length; i++)
            {
                m_Chickens[i].EnableControl();
            }

            //m_Chickens[0].EnableControl();
        }


        private void DisableTankControl()
        {
            for (int i = 0; i < m_Tanks.Length; i++)
            {
                m_Tanks[i].DisableControl();
            }

            
            //m_Chickens[0].DisableControl();
            
        }

        private void DisableChickenControl()
        {
            for (int i = 0; i < m_Chickens.Length; i++)
            {
                m_Chickens[i].DisableControl();
            }


            //m_Chickens[0].DisableControl();

        }

        private void DestroyAllChickens()
        {
            
            for (int i = 0; i < m_Chickens.Length; i++)
            {
                if (m_Chickens[i] != null)
                {
                    Destroy(m_Chickens[i].m_Instance);
                }
            }
        }

        private void DestroyEggs()
        {
            GameObject[] eggs;
            eggs = GameObject.FindGameObjectsWithTag("Egg");

            for (int i = 0; i < eggs.Length; i++)
            {
                if (eggs[i] != null)
                {
                    Destroy(eggs[i]);
                }
            }
        }

        private void DestroyFood()
        {
            GameObject[] food;
            food = GameObject.FindGameObjectsWithTag("Food");

            for (int i = 0; i < food.Length; i++)
            {
                if (food[i] != null)
                {
                    Destroy(food[i]);
                }
            }
        }
    }
}