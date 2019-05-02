using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerMainMenu : MonoBehaviour
{
    [Header("Canvases")]
    public Canvas MainCanvas;
    public Canvas OptionsCanvas;
    
    [Header("Audio Manager")]
    public AudioSource AudioMusicSource;
    
    private void Awake()
    {
        MainCanvas.gameObject.SetActive(true);
        OptionsCanvas.gameObject.SetActive(false);
        
    }
    
    public void GameContinueToNextLevel()
    {
        LoadingSceneManager.LoadScene(2);
    }

    public void GameQuit()
    {
        Application.Quit();
    }
}
