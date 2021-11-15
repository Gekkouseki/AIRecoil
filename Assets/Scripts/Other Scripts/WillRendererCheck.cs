using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WillRendererCheck : MonoBehaviour
{
    [SerializeField]
    private string m_CameraName = "Game Camera";
    [SerializeField]
    private int m_MaxCameraNum = 2;
    [SerializeField]
    private bool[] m_CameraViewArray;
    [SerializeField]
    private int m_CameraNum = 0;

    private void Start()
    {
        m_CameraViewArray = new bool[m_MaxCameraNum];
    }

    private void Update()
    {
        m_CameraNum = 0;
        m_CameraViewArray = m_CameraViewArray.Select(b => false).ToArray();
    }

    private void OnWillRenderObject()
    {
        if (Camera.current.name == "SceneCamera")
            return;
        var name = Camera.current.name;
        var index = int.Parse(name.Substring(name.Length - 1));
        m_CameraViewArray[index - 1] = true;
        m_CameraNum++;
    }

    public bool CheckCameraView()
    {
        for (int i = 0; i < m_CameraViewArray.Length; i++)
        {
            if (m_CameraViewArray[i])
                return true;
        }
        return false;
    }

    public bool CheckCameraView(int index)
    {
        if (index < 0 || index >= m_CameraViewArray.Length)
        {
            Debug.LogError($"CheckCameraView : Index Out of Range");
            return false;
        }
        return m_CameraViewArray[index];
    }
}
