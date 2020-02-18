using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadSinglePlayerGame()
    {
        SceneManager.LoadScene(1); // single player game scene
    }

    public void LoadCoOpPlayerGame()
    {
        SceneManager.LoadScene(2); // co-op player game scene

    }
}
