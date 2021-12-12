using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static bool infoView = true;

    [SerializeField]
    private GameObject hideInfo;
    [SerializeField]
    private GameObject viewInfo;

    private void Start()
    {
        ChangeInfoView();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneController.Instance.TitleSceneMove();
            Cursor.lockState = CursorLockMode.None;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            infoView = !infoView;
            ChangeInfoView();
        }
    }

    private void ChangeInfoView()
    {
        hideInfo.SetActive(!infoView);
        viewInfo.SetActive(infoView);
    }
}
