using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private AiInformation AI;

    [Header("Normal Information")]
    [SerializeField]
    private float m_rotateSpeed = 1.0f;

    [Header("AI Information")]
    [SerializeField]
    private Transform targetTrans;
    public Transform TargetTrans { get => targetTrans; set => targetTrans = value; }
    private Vector3 nTargetPos;
    private Vector3 toTargetPos;
    private Vector3 fromTargePos;

    [SerializeField]
    private float m_TargetSetDelayTime = 0.01f;
    private float targetSetDelayTime = 0.0f;

    [SerializeField]
    private float m_AiRotateSpeed = 1.0f;
    [SerializeField]
    private FRange m_AiAddSpeed;
    private float aiAddSpeed = 0.0f;
    private Tween addSpeedTween;

    [SerializeField]
    private AnimationCurve brakingErrorCurve;
    [SerializeField]
    private float brakingErrorMultiplier;
    [SerializeField, ReadOnly]
    private float brakingError;

    [Header("Common Information")]
    [SerializeField]
    private Transform transHead;
    [SerializeField]
    private Transform transY;
    [SerializeField]
    private Transform transX;
    [SerializeField]
    private FRange fRangeX;

    [SerializeField]
    private Camera _camera;
    private Vector2 cameraCenter;
    private float maxDist;

    #region Common Method
    private void Start()
    {
        if (!AI.AIMode)
            Cursor.lockState = CursorLockMode.Locked;

        cameraCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        maxDist = Vector2.Distance(cameraCenter, Vector2.zero);
    }

    private void Update()
    {
        if (AI.AIMode)
            AIUpdate();
        else
            NormalUpdate();
    }
    private void OnValidate()
    {
        fRangeX.OnValidate();
    }

    public void ShootingMove(float angleX, float angleY)
    {
        transY.Rotate(Vector3.up, angleX * m_rotateSpeed * Time.deltaTime);

        var eAngle = transX.localEulerAngles;
        eAngle.x -= angleY * m_rotateSpeed * Time.deltaTime;
        if (eAngle.x > fRangeX.MaxValue && eAngle.x < 180)
            eAngle.x = fRangeX.MaxValue;
        if (eAngle.x < fRangeX.MinValue && eAngle.x > 180)
            eAngle.x = fRangeX.MinValue;
        transX.localEulerAngles = eAngle;
    }

    #endregion

    #region NormalMode Method
    private void NormalUpdate()
    {
        var mX = Input.GetAxis("Mouse X"); // ??????
        transY.Rotate(Vector3.up, mX * m_rotateSpeed * Time.deltaTime);

        var mY = Input.GetAxis("Mouse Y"); // ?c????
        var eAngle = transX.localEulerAngles;
        eAngle.x -= mY * m_rotateSpeed * Time.deltaTime;
        if (eAngle.x > fRangeX.MaxValue && eAngle.x < 180)
            eAngle.x = fRangeX.MaxValue;
        if (eAngle.x < fRangeX.MinValue && eAngle.x > 180)
            eAngle.x = fRangeX.MinValue;
        transX.localEulerAngles = eAngle;
    }

    #endregion

    #region AIMode Method
    private void AIUpdate()
    {
        SetTargetPos();
        SetRotateX();
        SetRotateY();
    }

    private void SetTargetPos()
    {
        targetSetDelayTime -= Time.deltaTime;
        nTargetPos = Vector3.Lerp(fromTargePos, toTargetPos, 1 - (targetSetDelayTime / m_TargetSetDelayTime));
        if(targetSetDelayTime <= 0.0f)
        {
            SetBrakingError();
            targetSetDelayTime = m_TargetSetDelayTime;
            var addPos = RandomVec3().normalized * brakingError;
            fromTargePos = nTargetPos;
            toTargetPos = targetTrans.position + addPos;
            addSpeedTween = DOTween.To(x => aiAddSpeed = x, m_AiAddSpeed.Value, m_AiAddSpeed.RandomValue(), m_TargetSetDelayTime);
        }
    }

    private void SetRotateY()
    {
        var toVec = Vector3.ProjectOnPlane(nTargetPos - transHead.position, Vector3.up);
        var fromVec = Vector3.ProjectOnPlane(transHead.forward, Vector3.up);
        var angle = Vector3.SignedAngle(fromVec, toVec, Vector3.up);
        var r = angle * (m_AiRotateSpeed + aiAddSpeed) * Time.deltaTime;
        transY.Rotate(Vector3.up, r);
    }

    private void SetRotateX()
    {
        var toVec = Vector3.ProjectOnPlane(nTargetPos - transHead.position, Vector3.right);
        var fromVec = Vector3.ProjectOnPlane(transHead.forward, Vector3.right);
        var addY = Vector3.SignedAngle(fromVec, toVec, Vector3.right);
        var eAngle = transX.localEulerAngles;
        var r = addY * (m_AiRotateSpeed + aiAddSpeed) * Time.deltaTime;
        eAngle.x += r;
        if (eAngle.x > fRangeX.MaxValue && eAngle.x < 180)
            eAngle.x = fRangeX.MaxValue;
        if (eAngle.x < fRangeX.MinValue && eAngle.x > 180)
            eAngle.x = fRangeX.MinValue;
        //eAngle.y = 0;
        //eAngle.z = 0;
        transX.localEulerAngles = eAngle;
    }

    private void SetBrakingError()
    {
        var tPos = _camera.WorldToScreenPoint(targetTrans.position);
        var dist = Vector2.Distance(tPos, cameraCenter);

        brakingError = brakingErrorCurve.Evaluate(dist / maxDist) // ?J?[?u???????????l.???S??????????????????
            * brakingErrorMultiplier; // ?J?[?u?????l?????p?{??
    }

    private Vector3 RandomVec3()
    {
        return new Vector3(RFloat(), RFloat(), RFloat());
    }

    private float RFloat()
    {
        return Random.Range(-1.0f, 1.0f);
    }

    #endregion
}
