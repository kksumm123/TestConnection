using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum LoadSceneType
{
    None, MoveToStage, LoadNewWorld,
}

public static class SceneLoadSystem
{
    static AsyncOperation process;
    static bool isAbleToLoadScene = true;
    public static string currentScene;

    static void ReleaseDontDestroyOnLoad(LoadSceneType loadSceneType)
    {
        switch (loadSceneType)
        {
            case LoadSceneType.None:
                break;
            case LoadSceneType.MoveToStage:
                break;
            case LoadSceneType.LoadNewWorld:
                break;
        }
    }

    static void OnUnloadScene()
    {
    }

    static void OnLoadedSceneEvent(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnLoadedSceneEvent;
        SceneManager.SetActiveScene(scene);
        isAbleToLoadScene = true;
    }

    /// <summary>
    /// Awake에서 LoadScene 시작시 OnLoadedScene이벤트에는
    /// 새로 로드된 씬이 아닌, 현재 시작한 씬의 정보가 담김.
    /// 2022.07.13 Woony
    /// </summary>
    /// <returns></returns>
    static IEnumerator AsyncLoadScene(string sceneName, LoadSceneType loadSceneType)
    {
        if (isAbleToLoadScene == false) yield break;

        ReleaseDontDestroyOnLoad(loadSceneType);

        isAbleToLoadScene = false;

        if (IsNeedToUnloadScene())
        {
            OnUnloadScene();
            process = SceneManager.UnloadSceneAsync(currentScene);
            yield return new WaitUntil(() => process.isDone);
        }
        SceneManager.sceneLoaded += OnLoadedSceneEvent;
        process = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        yield return new WaitUntil(() => process.isDone);
        currentScene = sceneName;
        process.allowSceneActivation = true;

        bool IsNeedToUnloadScene() =>
            currentScene != null
            && (currentScene != string.Empty
            || currentScene != "");
    }

    public static void LoadScene(string sceneName, LoadSceneType loadSceneType)
    {
        // GameManager.Instance.StartCoroutine(AsyncLoadScene(sceneName, loadSceneType));
    }
}
