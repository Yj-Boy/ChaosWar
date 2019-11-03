using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{

    // 抖动目标的transform(若未添加引用，怎默认为当前物体的transform)
    public Transform camTransform;

    //持续抖动的时长
    private float shake=10f;

    // 抖动幅度（振幅）
    //振幅越大抖动越厉害
    private float shakeAmount=0.2f;
    private float decreaseFactor=0.5f;

    Vector3 originalPos;

    void Awake()
    {
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void OnEnable()
    {
        originalPos = camTransform.localPosition;
    }

    void Update()
    {
        if (shake > 0)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

            shake -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shake = 0f;
            camTransform.localPosition = originalPos;
        }
    }

    public void SetCameraShake(float time, float amout, float decrease)
    {
        shake = time;
        shakeAmount = amout;
        decreaseFactor= decrease;
        Debug.Log("shaketime:" + this.shake);
    }
}
