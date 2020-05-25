using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Complete
{
    public class SceneSwitcher : MonoBehaviour
    {
        public void LoadMenu()
        {
            SceneManager.LoadScene("Menu");
        }

        public void EndGame()
        {
            SceneManager.LoadScene("GameOver");
        }

        public void Quit()
        {
            Application.Quit();
        }

        public void Level1()
        {
            SceneManager.LoadScene("Level1");
        }

        public void Level2()
        {
            SceneManager.LoadScene("Level2");
        }

        public void Level3()
        {
            SceneManager.LoadScene("Level3");
        }


    }
}


