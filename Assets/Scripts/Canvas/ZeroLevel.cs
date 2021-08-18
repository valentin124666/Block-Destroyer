using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ZeroLevel : MonoBehaviour
{
    private void Start()
    {
        if (PlayerPrefs.GetInt("Scenes")<=0|| PlayerPrefs.GetInt("Scenes") >= SceneManager.sceneCountInBuildSettings)
        {
            PlayerPrefs.SetInt("Scenes", 1);
        }
        if (PlayerPrefs.GetInt("Level") <=0)
        {
            PlayerPrefs.SetInt("Level", 1);
        }

        SceneManager.LoadScene(PlayerPrefs.GetInt("Scenes"));

    }

}
