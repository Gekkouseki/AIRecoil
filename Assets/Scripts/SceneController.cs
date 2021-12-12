using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void TitleSceneMove()
    {
        SceneManager.LoadScene(SceneName.Title);
    }

    public void GameSceneMove()
    {
        SceneManager.LoadScene(SceneName.Game);
    }
}

public static class SceneName
{
    public const string Title = "TitleScene";
    public const string Game = "GameScene";
}