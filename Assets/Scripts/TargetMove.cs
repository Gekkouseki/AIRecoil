using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TargetMove : MonoBehaviour
{
    public enum MoveMode
    {
        Manual,
        Box,
        Circle,
        RandomRange,
    }

    [SerializeField]
    private MoveMode mode;

    [SerializeField]
    private float moveSpeed = 1.0f;
    private Vector3 startPos;

    [SerializeField]
    private float moveRange = 10.0f;

    private List<Vector3> boxPosArray = new List<Vector3>();
    private int boxPosIndex = 1;

    private void Start()
    {
        startPos = transform.position;
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
        }
    }

    private void ManualMove()
    {
        Vector3 moveVec;
        moveVec.x = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        moveVec.y = (Input.GetKey(KeyCode.Space) ? moveSpeed : Input.GetKey(KeyCode.LeftShift) ? -moveSpeed : 0) * Time.deltaTime;
        moveVec.z = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        transform.Translate(moveVec);
    }

    private void BoxMove()
    {
        transform.DOMove(boxPosArray[boxPosIndex], (2 * moveRange) / moveSpeed)
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
        transform.RotateAround(startPos, Vector3.forward, moveSpeed * Time.deltaTime);
    }

    private void RandomMove()
    {
        transform.DOMove(RandomPos() + startPos, moveSpeed * 2)
            .OnComplete(() =>
            {
                RandomMove();
            });
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
