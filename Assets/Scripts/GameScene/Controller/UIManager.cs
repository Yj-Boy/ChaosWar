using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //单例模式
    private static UIManager instance = null;
    private static readonly object threadSafeLock = new object();

    public static UIManager Instance
    {
        get
        {
            if(instance==null)
            {
                lock (threadSafeLock)
                {
                    if (instance == null)
                    {
                        instance = new UIManager();
                    }             
                }
            }
            return instance;
        }
    }

    //参数
    public Text tipText;

    //方法
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        if(tipText==null)
        {
            Debug.LogWarning(GetType() + "UIManager/Start()/tipText未设置!");
        }

        tipText.gameObject.SetActive(false);
    }

    public void ShowTipText(string tip)
    {
        tipText.text = tip;
        tipText.gameObject.SetActive(true);
        Invoke("HideTipText", 1f);
    }

    public void HideTipText()
    {
        tipText.gameObject.SetActive(false);
    }
}
