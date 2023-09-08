using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    private string sceneNameToBeLoaded;
    private float timeDelayLoad = 1.5f;

    public void LoadScene(string sceneName)
    {
        sceneNameToBeLoaded = sceneName;
        StartCoroutine(InitializeSceneLoading());
    }

    IEnumerator InitializeSceneLoading()
    {
       /* SceneManager.LoadScene(NameScene.Scene_Loading.ToString());

        yield return new WaitForSeconds(timeDelayLoad);
        SceneManager.LoadScene(sceneNameToBeLoaded);*/

        yield return SceneManager.LoadSceneAsync(sceneNameToBeLoaded);
        StartCoroutine(LoadActualyScene());
    }

    IEnumerator LoadActualyScene()
    {
        var asyncSceneLoading = SceneManager.LoadSceneAsync(sceneNameToBeLoaded);

        asyncSceneLoading.allowSceneActivation = false;

        while(!asyncSceneLoading.isDone)
        {
            Debug.Log("asyncSceneLoading.progress: " + asyncSceneLoading.progress);
            if(asyncSceneLoading.progress >= 0.9f)
            {
                asyncSceneLoading.allowSceneActivation = true;
            }

            yield return null;
        }
    }

}
