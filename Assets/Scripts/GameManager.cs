using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    [Header("Canvases")]
    public GameObject GameCanvases;
    public Text TextBuldozerCount;
    public Canvas PauseCanvas;
    public Canvas OptionsCanvas;
    public Canvas WinCanvas;
    public Canvas LoseCanvas;

    private int _buldozerCountTotal;
    private int _forestCountTotal;

    [Header("Timers")]
    public float WaitToShowScreen = 4.0f;

    private bool _gamePused = false;

    [Header("Audio Manager")]
    public AudioSource AudioMusicSource;
    public AudioSource AudioSFXSource;

    [Header("Sound Effects")]
    public AudioClip Win;
    public AudioClip Lose;

    //private AudioSource _myAudioSource;
    //private float _maxVolume;
    //private float _currentVolume;

    //Dodati dio koji će pratiti dan i noć i prema tome mijenjati osvjetljenje :)
    //https://wiki.unity3d.com/index.php/DayNightController
    [Header("Include Day and Night")]
    public bool IncludeNightGameplay = true;

    //Singleton
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (GameManager)FindObjectOfType(typeof(GameManager));

                if (_instance == null)
                    Debug.Log("An instance of GameManager doesn't exist!");
                //Sad ćemo napisati poruku da ne postoji
                //i da ga netko treba postaviti na scenu.
                //To nije konačno rješenje; konačno rješenje bi bilo
                //napraviti GameObject, dodati mu ovu skriptu, postaviti vrijednosti varijabli i vratimo taj objekt.
            }
            return _instance;
        }
    }

    private void Awake()
    {
        //Singleton
        if (Instance != this)
            Destroy(gameObject);

        GameCanvases.gameObject.SetActive(false);
        PauseCanvas.gameObject.SetActive(false);
        OptionsCanvas.gameObject.SetActive(false);
        WinCanvas.gameObject.SetActive(false);
        LoseCanvas.gameObject.SetActive(false);

        _gamePused = false;

        //_myAudioSource = GetComponent<AudioSource>();
        //_maxVolume = _myAudioSource.volume;
        //_currentVolume = _maxVolume;

        Time.timeScale = 1.0f;
    }

    private void Start()
    {
        GameCanvases.SetActive(true);
    }
    /*
    private void Update()
    {
        //PAUSE
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_gamePused)
            {
                _gamePused = true;
                GamePause(true);
            }
            else
            {
                _gamePused = false;
                GameUnPause(false);
            }
        }
    }*/
    
    public void GamePause(bool isPaused)
    {
        _gamePused = isPaused;
        Time.timeScale = 0.0f;
        //_currentVolume = _maxVolume * 0.3f;
        //_myAudioSource.volume = _currentVolume;
        GameCanvases.gameObject.SetActive(false);
        PauseCanvas.gameObject.SetActive(true);
    }

    public void GameUnPause(bool isPaused)
    {
        _gamePused = isPaused;
        Time.timeScale = 1.0f;
        //_currentVolume = _maxVolume;
        //_myAudioSource.volume = _currentVolume;
        GameCanvases.gameObject.SetActive(true);
        PauseCanvas.gameObject.SetActive(false);
    }

    public IEnumerator GameLostCo()
    {
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(WaitToShowScreen);
        Time.timeScale = 0.0f;
        GameCanvases.gameObject.SetActive(false);
        LoseCanvas.gameObject.SetActive(true);
    }

    public void GameLost()
    {
        StartCoroutine(GameLostCo());
    }

    public IEnumerator GameWonCo()
    {
        yield return new WaitForSeconds(WaitToShowScreen);
        //Time.timeScale = 0.0f;
        GameCanvases.gameObject.SetActive(false);
        WinCanvas.gameObject.SetActive(true);
    }

    public void GameWon()
    {
        StartCoroutine(GameWonCo());
    }

    public void GameRastart()
    {
        Time.timeScale = 1.0f;
        int thisLevelIndex = SceneManager.GetActiveScene().buildIndex;
        PauseCanvas.gameObject.SetActive(true);

        //SceneManager.LoadScene(thisLevelIndex);
        LoadingSceneManager.LoadScene(thisLevelIndex);
    }

    public void GameContinueToNextLevel()
    {
        Time.timeScale = 1.0f;
        int nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextLevelIndex == SceneManager.sceneCountInBuildSettings)
        {
            GameToMainMenu(); 
        } else
        {
            //SceneManager.LoadScene(nextLevelIndex);
            LoadingSceneManager.LoadScene(nextLevelIndex);
        }
    }

    public void GameToMainMenu()
    {
        Time.timeScale = 1.0f;
        //SceneManager.LoadScene(0);
        PauseCanvas.gameObject.SetActive(true);
        LoadingSceneManager.LoadScene(0);
    }

    public void GameQuit()
    {
        Time.timeScale = 1.0f;
        //SceneManager.LoadScene(0);
        PauseCanvas.gameObject.SetActive(true);
        LoadingSceneManager.LoadScene(0);
    }

    public void PlayBackgroundMusic(AudioClip audioClip)
    {
        AudioMusicSource.clip = audioClip;
        AudioMusicSource.playOnAwake = true;
        AudioMusicSource.Play();
    }

    public void PlaySoundOneShot(AudioClip clipToPlay)
    {
        AudioSFXSource.pitch = Random.Range(0.8f, 1.2f);
        AudioSFXSource.PlayOneShot(clipToPlay);
        AudioSFXSource.pitch = 1.0f;
    }

    public void PlaySoundDelayed(AudioClip clipToPlay, float delay)
    {
        //_myAudioSource.clip = clipToPlay;
        //_myAudioSource.pitch = Random.Range(0.8f, 1.2f);
        //_myAudioSource.PlayDelayed(delay);
        //_myAudioSource.pitch = 1.0f;
    }

    public void BuldozerCountSet(int buldozerCountTotal)
    {
        _buldozerCountTotal = buldozerCountTotal;
        TextBuldozerCount.text = _buldozerCountTotal.ToString();
    }

    public void BuldozerCountReduce()
    {
        --_buldozerCountTotal;
        TextBuldozerCount.text = _buldozerCountTotal.ToString();
        if (_buldozerCountTotal <= 0)
            GameWon();
    }
}
