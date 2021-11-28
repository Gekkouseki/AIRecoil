using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTest : MonoBehaviour
{
    [SerializeField]
    private GameObject A;
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private float maxDist;

    void Start()
    {
        var tPos = new Vector2(Screen.width, Screen.height);
        var fPos = new Vector2(Screen.width / 2, Screen.height / 2);
        maxDist = Vector2.Distance(tPos, fPos);
        Debug.Log($"Max {maxDist}");
    }

    private void Update()
    {
        var tPos = _camera.WorldToScreenPoint(A.transform.position);
        var fPos = new Vector2(Screen.width / 2, Screen.height / 2);
        var dist = Vector2.Distance(tPos, fPos);
        Debug.Log($"dist/max = {dist / maxDist}");
    }
}
