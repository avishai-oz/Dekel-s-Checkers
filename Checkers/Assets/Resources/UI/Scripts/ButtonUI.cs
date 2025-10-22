using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonUI : MonoBehaviour
{
    [SerializeField] private string gameSceneName =  "MainGame";
    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
