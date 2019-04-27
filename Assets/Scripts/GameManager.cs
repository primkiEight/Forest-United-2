using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    [Header("Canvases")]
    public GameObject GameCanvases;
    public Canvas PauseCanvas;
    public Canvas OptionsCanvas;
    public Canvas WinCanvas;
    public Canvas LoseCanvas;

    [Header("Timers")]
    public float WaitToShowScreen = 1.0f;

    private bool _gamePused = false;

    [Header("Audio Manager")]
    public AudioSource AudioMusicSource;
    public AudioSource AudioSFXSource;

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
    }
    
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
        Time.timeScale = 0.0f;
        GameCanvases.gameObject.SetActive(false);
        WinCanvas.gameObject.SetActive(true);
    }

    public void GameWon()
    {
        StartCoroutine(GameWonCo());
    }

    public void GameRastart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void GameQuit()
    {
        SceneManager.LoadScene(0);
    }


    public void PlaySoundOneShot(AudioClip clipToPlay)
    {
        //_myAudioSource.pitch = Random.Range(0.8f, 1.2f);
        //_myAudioSource.PlayOneShot(clipToPlay);
        //_myAudioSource.pitch = 1.0f;
    }

    public void PlaySoundDelayed(AudioClip clipToPlay, float delay)
    {
        //_myAudioSource.clip = clipToPlay;
        //_myAudioSource.pitch = Random.Range(0.8f, 1.2f);
        //_myAudioSource.PlayDelayed(delay);
        //_myAudioSource.pitch = 1.0f;
    }
}
