using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    private Text modeText;

    private void Start()
    {
        ChangeText();
    }

    public void ChangeMoveMode()
    {
        switch (TargetMove.Mode)
        {
            case TargetMove.MoveMode.None:
                TargetMove.Mode = TargetMove.MoveMode.Circle;
                break;
            case TargetMove.MoveMode.Box:
                TargetMove.Mode = TargetMove.MoveMode.Circle;
                break;
            case TargetMove.MoveMode.Circle:
                TargetMove.Mode = TargetMove.MoveMode.RandomRange;
                break;
            case TargetMove.MoveMode.RandomRange:
                TargetMove.Mode = TargetMove.MoveMode.Warp;
                break;
            case TargetMove.MoveMode.Warp:
                TargetMove.Mode = TargetMove.MoveMode.Box;
                break;
        }
        ChangeText();
    }

    private void ChangeText()
    {
        switch (TargetMove.Mode)
        {
            case TargetMove.MoveMode.Box:
                modeText.text = "Now : Box Mode";
                break;
            case TargetMove.MoveMode.Circle:
                modeText.text = "Now : Circle Mode";
                break;
            case TargetMove.MoveMode.RandomRange:
                modeText.text = "Now : Random Mode";
                break;
            case TargetMove.MoveMode.Warp:
                modeText.text = "Now : Warp Mode";
                break;
        }
    }
}
