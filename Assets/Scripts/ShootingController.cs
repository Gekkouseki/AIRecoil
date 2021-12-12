using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController : MonoBehaviour
{
    [SerializeField]
    private AiInformation AI;

    [Header("Common Information")]
    [SerializeField]
    private Vector2[] recoilPatterns;
    private int rpIndex = 0;
    [SerializeField]
    private float recoilPow = 1.0f;
    [SerializeField]
    private float shotCoolTime = 0.5f; // 次の弾が出るまでの待ち時間
    private float countShotTime = 0.0f;
    [SerializeField]
    private float recoilResetTime = 1.0f; // リコイルパターンリセットまでの時間
    private float countRecoilTime = 0.0f;
    [SerializeField]
    private float shootingRange = 50.0f; // 射程距離
    [SerializeField]
    private CameraController CC;
    [SerializeField]
    private Transform shotTrans;
    [SerializeField]
    private LayerMask shotHitMask;

    [SerializeField]
    private GameObject targetHitEffect;
    [SerializeField]
    private GameObject otherHitEffect;

    [Header("AI Information")]
    [SerializeField]
    private LayerMask aiShotMask;
    [SerializeField]
    private bool debugView;

    private void Update()
    {
        if(countRecoilTime > 0.0f)
        {
            countRecoilTime -= Time.deltaTime;
            if (countRecoilTime <= 0.0f)
                rpIndex = 0;
        }

        if (countShotTime > 0.0f)
        {
            countShotTime -= Time.deltaTime;
            return;
        }

        if (AI.AIMode)
            AIUpdate();
        else
            NormalUpdate();
    }

    private void NormalUpdate()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Shooting();
        }
    }

    private void AIUpdate()
    {
        var ray = new Ray(shotTrans.position, shotTrans.forward);
        if (Physics.Raycast(ray,out RaycastHit hit, shootingRange, aiShotMask))
        {
            Shooting();
        }
        if (debugView)
            Debug.DrawRay(ray.origin, ray.direction * shootingRange, Color.red, 1.0f);
    }

    private void Shooting()
    {
        var ray = new Ray(shotTrans.position, shotTrans.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, shootingRange, shotHitMask))
        {
            if (hit.collider.CompareTag("Target"))
                Instantiate(targetHitEffect, hit.point, Quaternion.identity);
            else
                Instantiate(otherHitEffect, hit.point, Quaternion.identity);
        }

        var angle = recoilPatterns[rpIndex] * recoilPow;
        rpIndex++;
        if (rpIndex >= recoilPatterns.Length) rpIndex = 0;
        CC.ShootingMove(angle.x,angle.y);

        countRecoilTime = recoilResetTime;
        countShotTime = shotCoolTime;
    }
}
