using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    //改单例
    private static ShakeCamera instance=null;
    private static readonly object threadSafeLock = new object();

    public static ShakeCamera Instance
    {
        get
        {
            if(instance==null)
            {
                lock(threadSafeLock)
                {
                    if(instance==null)
                    {
                        instance = new ShakeCamera();
                    }
                }
            }
            return instance;
        }
    }


    // 抖动目标的transform(若未添加引用，怎默认为当前物体的transform)
    public Transform camTransform;

    //持续抖动的时长
    private static float shake=0f;

    // 抖动幅度（振幅）
    //振幅越大抖动越厉害
    private static float shakeAmount =0.2f;
    private static float decreaseFactor =0.5f;

    Vector3 originalPos;

    void Awake()
    {
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }

        instance = this;
    }

    private void OnEnable()
    {
        originalPos = camTransform.localPosition;
    }

    private void Update()
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

    public static void SetCameraShake(float time,float amount,float decrease)
    {
        shake = time;
        shakeAmount = amount;
        decreaseFactor = decrease;
    }

    public void StartShakeCamera()
    {
        if(shake==0)
        {
            shake = 0.01f;
            shakeAmount = 0.05f;
            decreaseFactor = 0.05f;
        }
    }
}
