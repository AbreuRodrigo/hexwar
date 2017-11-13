using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public Text loadingText;

    private bool loadScene = false;

    private static int sceneToLoad = 1;

    private int currentScene;

    AsyncOperation async = null;

    private void Awake()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(LoadNewScene());
    }

    static public void LoadScene(int levelToLoad)
    {
        sceneToLoad = levelToLoad;
        SceneManager.LoadScene(0);
    }


    void WaitForLoader()
    {
        StartCoroutine(LoadNewScene());
    }
    
    void Update()
    {
        if (async != null)
        {
            loadingText.text = "Loading " + Mathf.RoundToInt(async.progress * 100) + "%";
        }
    }

    // TODO: Is not loading back to the menu from the main scene
    // The coroutine runs on its own at the same time as Update() and takes an integer indicating which scene to load.
    IEnumerator LoadNewScene()
    {
        // This line waits for 3 seconds before executing the next line in the coroutine.
        // This line is only necessary for this demo. The scenes are so simple that they load too fast to read the "Loading..." text.
        yield return new WaitForSeconds(0.15f);

        // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
        async = SceneManager.LoadSceneAsync(sceneToLoad);
        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone)
        {
            yield return null;
        }

    }

}
