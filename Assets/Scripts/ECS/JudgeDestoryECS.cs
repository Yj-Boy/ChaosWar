using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgeDestoryECS : MonoBehaviour
{
    private static JudgeDestoryECS instance=null;
    private static readonly object threadSafeLock = new object();

    public static JudgeDestoryECS Instance
    {
        get
        {
            if(instance==null)
            {
                lock(threadSafeLock)
                {
                    if(instance==null)
                    {
                        instance = new JudgeDestoryECS();
                    }
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    public bool isDestroyECS;

    public void DestroyECS()
    {
        isDestroyECS = true;
    }
}
