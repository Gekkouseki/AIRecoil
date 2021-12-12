using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TargetMove : MonoBehaviour
{
    public enum MoveMode
    {
        None,
        Manual,
        Box,
        Circle,
        RandomRange,
        Warp,
    }

    public static MoveMode Mode;

    [SerializeField]
    private MoveMode mode;

    [SerializeField]
    private float manualMoveSpeed = 1.0f;
    [SerializeField]
    private float boxMoveSpeed = 1.0f;
    [SerializeField]
    private float circleMoveSpeed = 1.0f;
    [SerializeField]
    private float randomMoveSpeed = 1.0f;
    [SerializeField]
    private float warpMoveSpeed = 1.0f;
    private float countTime = 0.0f;
    private Vector3 startPos;

    [SerializeField]
    private float moveRange = 10.0f;

    private List<Vector3> boxPosArray = new List<Vector3>();
    private int boxPosIndex = 1;

    private void Start()
    {
        startPos = transform.position;
        if (Mode != MoveMode.None)
            mode = Mode;
        switch (mode)
        {
            case MoveMode.Box:
                boxPosArray.Add(startPos + new Vector3(1, 1) * moveRange);
                boxPosArray.Add(startPos + new Vector3(-1, 1) * moveRange);
                boxPosArray.Add(startPos + new Vector3(-1, -1) * moveRange);
                boxPosArray.Add(startPos + new Vector3(1, -1) * moveRange);
                transform.position = boxPosArray[0];
                BoxMove();
                break;
            case MoveMode.Circle:
                transform.position += Vector3.right * moveRange;
                break;
            case MoveMode.RandomRange:
                RandomMove();
                break;
            case MoveMode.Warp:
                WarpMove();
                break;
        }
    }

    private void Update()
    {
        switch (mode)
        {
            case MoveMode.Manual:
                ManualMove();
                break;
            case MoveMode.Circle:
                CircleMove();
                break;
            case MoveMode.Warp:
                countTime -= Time.deltaTime;
                if (countTime <= 0.0f)
                    WarpMove();
                break;
        }
    }

    private void ManualMove()
    {
        Vector3 moveVec;
        moveVec.x = Input.GetAxis("Horizontal") * manualMoveSpeed * Time.deltaTime;
        moveVec.y = (Input.GetKey(KeyCode.Space) ? manualMoveSpeed : Input.GetKey(KeyCode.LeftShift) ? -manualMoveSpeed : 0) * Time.deltaTime;
        moveVec.z = Input.GetAxis("Vertical") * manualMoveSpeed * Time.deltaTime;

        transform.Translate(moveVec);
    }

    private void BoxMove()
    {
        transform.DOMove(boxPosArray[boxPosIndex], (2 * moveRange) / boxMoveSpeed)
            .OnComplete(() =>
            {
                boxPosIndex++;
                if (boxPosIndex >= boxPosArray.Count)
                    boxPosIndex = 0;
                BoxMove();
            });
    }

    private void CircleMove()
    {
        transform.RotateAround(startPos, Vector3.forward, circleMoveSpeed * Time.deltaTime);
    }

    private void RandomMove()
    {
        transform.DOMove(RandomPos() + startPos, randomMoveSpeed)
            .OnComplete(() =>
            {
                RandomMove();
            });
    }

    private void WarpMove()
    {
        transform.position = (RandomPos() / 2) + startPos;
        countTime = warpMoveSpeed;
    }

    private Vector3 RandomPos()
    {
        return new Vector3(RandomRange(), RandomRange(), RandomRange());
    }

    private float RandomRange()
    {
        return Random.Range(-moveRange, moveRange);
    }
}
