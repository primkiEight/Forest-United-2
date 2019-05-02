using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour {

    [Header("Loading Scene Visuals")]
    public Image SceneFadeOverlay;
    public Image ProgressLoadingImage;
    public Slider ProgressSlider;
    public Image LoadingSceneFadeOverlay;
    public List<Rigidbody2D> AppleRigidBodiesList = new List<Rigidbody2D> { };

    [Header("Loading Settings")]
    public float FadeDuration = 2.0f;

    //Statična public varijabla koju koristimo za pohranu indexa scene koju želimo loadati
    //(U statičnim metodama (LoadScene, dolje) ne možemo koristiti lokalne varijable)
    public static int SceneIndexToLoad;

    private void Awake()
    {
        SceneFadeOverlay.enabled = true;
        ProgressSlider.gameObject.SetActive(false);
        ProgressSlider.value = 0.0f;
        ProgressLoadingImage.gameObject.SetActive(false);
        LoadingSceneFadeOverlay.enabled = false;

        StartCoroutine(LoadSceneAsync(SceneIndexToLoad));
    }

    public static void LoadScene(int sceneIndex)
    {
        SceneIndexToLoad = sceneIndex;
        SceneManager.LoadScene("LoadingScene", LoadSceneMode.Additive);
    }

    //true - fadeIn
    //false - fadeOut
    private IEnumerator Fade (Image image, float fadeTime, bool fadeType)
    {
        float startAlpha = fadeType ? 0.0f : 1.0f;
        float endAlpha = fadeType ? 1.0f : 0.0f;

        Color imageColor = image.color;
        imageColor.a = startAlpha;

        float stopwatch = 0.0f;

        while (stopwatch <= fadeTime)
        {
            stopwatch += Time.deltaTime;
            imageColor.a = Mathf.Lerp(startAlpha, endAlpha, stopwatch / fadeTime);
            image.color = imageColor;

            //Čekanje 1 frame
            yield return null;
        }

        imageColor.a = endAlpha;
        image.color = imageColor;
    }

    private IEnumerator UnloadSceneAsync(Scene activeScene)
    {
        AsyncOperation unloadingAsyncOperationOldScene = SceneManager.UnloadSceneAsync(activeScene);
        yield return null;
    }

    private IEnumerator LoadSceneAsync(int sceneBuildIndex)
    {
        //FadeOut zaslona aktivne scene
        yield return StartCoroutine(Fade(SceneFadeOverlay, FadeDuration, true));

        //Unloading postojeće scene
        //Trebalo bi provjeriti je li gotovo, prije prikazivanja nove scene (vjerojatno mogo dobiti čekanjem?)
        yield return StartCoroutine(UnloadSceneAsync(SceneManager.GetActiveScene()));

        //Uključiti zaslon loading scene / progress bara, i progress bar
        LoadingSceneFadeOverlay.enabled = true;
        //ProgressSlider.gameObject.SetActive(true);
        ProgressLoadingImage.gameObject.SetActive(true);

        //FadeOut zaslona loading scene / progress bara
        yield return StartCoroutine(Fade(LoadingSceneFadeOverlay, FadeDuration, false));

        AsyncOperation loadingAsyncOperationNewScene = SceneManager.LoadSceneAsync(sceneBuildIndex, LoadSceneMode.Additive);

        //Ne želimo odmah uključiti učitanu scenu, nego tek kad ju mi odlučimo prikazati:
        loadingAsyncOperationNewScene.allowSceneActivation = false;

        float loadingProgress = 0.0f;
        float apples = (float)AppleRigidBodiesList.Count;
        float firstMargin = 1/apples;
        int i = 0;

        Debug.Log(apples);
        Debug.Log(firstMargin);

        while (loadingAsyncOperationNewScene.progress < 0.9f)
        {
            yield return null;
            //Ovaj nikad ne prelazi 0.9f, mogli bi prikazivati isto sa Lerpom između 0 i 0.9...
            loadingProgress = loadingAsyncOperationNewScene.progress;
            //ProgressSlider.value = loadingProgress;

            if(loadingProgress >= firstMargin)
            {
                AppleRigidBodiesList[i].WakeUp();
                firstMargin += 1/apples;
                i++;
                Debug.Log("jabuka");
                //yield return new WaitForSecondsRealtime(1.0f);
            }

        }
        //... pa nam ovo zapravo ne bi trebalo:
        //ProgressSlider.value = 1.0f;

        //yield return new WaitForSeconds(5.0f);

        //Scena je loadana

        //FadeIn zaslona loading scene / progress bara
        yield return StartCoroutine(Fade(LoadingSceneFadeOverlay, FadeDuration, true));

        //Ovime sada želimo uključiti novu učitanu scenu:
        loadingAsyncOperationNewScene.allowSceneActivation = true;

        //Isključiti zaslon loading scene / progress bara, i progress bar
        //ProgressSlider.gameObject.SetActive(false);
        ProgressLoadingImage.gameObject.SetActive(false);
        LoadingSceneFadeOverlay.enabled = false;
        
        //FadeIn zaslona aktivne scene
        yield return StartCoroutine(Fade(SceneFadeOverlay, FadeDuration, false));

        //Gasimo i ovu scenu
        //SceneManager.UnloadSceneAsync("LoadingScene");
        Scene loadingScene = SceneManager.GetSceneByName("LoadingScene");
        yield return StartCoroutine(UnloadSceneAsync(loadingScene));
    }
}
