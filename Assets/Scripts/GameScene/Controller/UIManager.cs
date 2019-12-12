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
    public Text tipText;                    //提示文字
    public Slider castleHpSlider;           //血条
    public Slider goldSlider;               //金币条
    public Slider bossHpSlider;             //Boss血条
    public float sliderSpeed;               //slider变化速度

    private float tmpHpSliderValue;         //血条中间转换值
    private float tmpGoldSliderValue;       //金币条中间转换值
    private float tmpBossSliderValue;       //Boss血条中间转换值

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
        if (castleHpSlider == null)
        {
            Debug.LogWarning(GetType() + "UIManager/Start()/castleHpSlider未设置!");
        }

        tipText.gameObject.SetActive(false);        
    }

    public void Update()
    {
        //更新Slider
        UpdateCastleHpSlider(tmpHpSliderValue);
        UpdateGoldSlider(tmpGoldSliderValue);
        UpdateBossSlider(tmpBossSliderValue);
    }

    //初始化Boss血条的值
    public void InitBossSlider(int hp)
    {
        bossHpSlider.maxValue = hp;
        bossHpSlider.value = hp;
        UpdateBossSliderValue(hp);
    }

    //更新Boss血条的值
    public void UpdateBossSliderValue(int hp)
    {
        tmpBossSliderValue = hp;
    }

    //更行Boss血条UI
    public void UpdateBossSlider(float hp)
    {
        bossHpSlider.value = Mathf.Lerp(
            bossHpSlider.value,
            hp,
            sliderSpeed * Time.deltaTime
            );
    }

    //初始化金币条的值
    public void InitGoldSlider(int gold)
    {
        goldSlider.maxValue = gold;
        goldSlider.value = 0;
        UpdateGoldSliderValue(gold);
    }

    //更新金币条的值
    public void UpdateGoldSliderValue(int gold)
    {
        tmpGoldSliderValue = gold;
    }

    //更新金币条UI
    public void UpdateGoldSlider(float gold)
    {
        goldSlider.value = Mathf.Lerp(
            goldSlider.value,
            gold,
            sliderSpeed * Time.deltaTime
            );
    }

    //初始化血条的值
    public void InitCastleHpSlider(int hp)
    {
        castleHpSlider.maxValue = hp;
        castleHpSlider.value = hp;
        UpdateHpSliderValue(hp);
    }
    
    //更新血条的值
    public void UpdateHpSliderValue(int hp)
    {
        tmpHpSliderValue = hp;
    }

    //更新血条的UI
    private void UpdateCastleHpSlider(float hp)
    {
        castleHpSlider.value = Mathf.Lerp(
            castleHpSlider.value,
            hp,
            sliderSpeed * Time.deltaTime
            );
    }

    //展示提示文字
    public void ShowTipText(string tip)
    {
        tipText.text = tip;
        tipText.gameObject.SetActive(true);
        Invoke("HideTipText", 1f);
    }

    //影藏提示文字
    public void HideTipText()
    {
        tipText.gameObject.SetActive(false);
    }
}
