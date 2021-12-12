using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayMove : MonoBehaviour
{
    [SerializeField]
    private Transform targetTrans;
    private Vector3 offset;
    [SerializeField]
    private float delayTime = 1.0f;

    private void Start()
    {
        offset = transform.position - targetTrans.position;
        transform.parent = null;
    }

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, targetTrans.position + offset, delayTime * Time.deltaTime);
    }
}
