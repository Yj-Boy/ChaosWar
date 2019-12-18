using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    //单例模式
    private static UIManager instance = null;
    private static readonly object threadSafeLock = new object();

    public static UIManager Instance
    {
        get
        {
            if (instance == null)
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
    public Slider[] skillCountDownSlider;   //技能倒计时图片
    public Button[] skillButton;            //技能按钮
    public int[] skillTime;
    public Image damageImage;               //受伤Image
    public Text winOrLoseText;              //胜负文字提示
    public GameObject gameOver;             //游戏结束界面
    public Image twinkleImage;              //濒临死亡闪烁画面
    public GameObject pausePanel;           //暂停面板

    private float tmpHpSliderValue;         //血条中间转换值
    private float tmpGoldSliderValue;       //金币条中间转换值
    private float tmpBossSliderValue;       //Boss血条中间转换值
    private float twinkleValue;             //闪烁画面的数值
    private Color color;                    //闪烁颜色变化
    //方法
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (tipText == null)
        {
            Debug.LogWarning(GetType() + "UIManager/Start()/tipText未设置!");
        }
        if (castleHpSlider == null)
        {
            Debug.LogWarning(GetType() + "UIManager/Start()/castleHpSlider未设置!");
        }

        tipText.gameObject.SetActive(false);
        InitSkillCountDownTime();

        color = twinkleImage.color;
    }

    public void Update()
    {
        //更新Slider
        UpdateCastleHpSlider(tmpHpSliderValue);
        UpdateGoldSlider(tmpGoldSliderValue);
        UpdateBossSlider(tmpBossSliderValue);

        //更新技能
        UpdateSkillButton();
        UpdateSkillCountDownSlider();

        //濒临死亡画面闪烁
        ShowTwinkleImage();
    }

    //暂停游戏接口
    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    //继续游戏接口
    public void ContinueGame()
    {
        Time.timeScale = 1;
    }

    //返回主界面
    public void BackStartScene()
    {
        ContinueGame();
        SceneManager.LoadScene("StartScene");
    }

    //显示暂停面板
    public void ShowPausePanel()
    {
        pausePanel.SetActive(true);
        gameOver.GetComponent<AudioSource>().Stop();
        pausePanel.GetComponent<AudioSource>().Play();      
    }

    //隐藏暂停面板
    public void HidePausePanel()
    {
        pausePanel.SetActive(false);
    }

    //重新开始接口
    public void ReStart()
    {
        ContinueGame();
        SceneManager.LoadScene("LoadingScene");
    }

    //退出游戏接口
    public void ExitGame()
    {
        Application.Quit();
    }

    //濒临死亡画面闪烁
    private void ShowTwinkleImage()
    {
        if (castleHpSlider.value <= 20)
        {
            if (color.a >= 0.045f)
            {
                twinkleValue = 0;
            }
            if (color.a <= 0.01f)
            {
                twinkleValue = 0.05f;
            }
            color.a = Mathf.Lerp(
                color.a,
                twinkleValue,
                2.5f * Time.deltaTime
                );
            twinkleImage.color = color;
        }      
        else
        {
            if(color.a!=0)
            {
                color.a = 0;
                twinkleImage.color = color;
            }         
        }
    }

    //显示游戏结束画面
    public void ShowGameOver(bool resule)
    {
        gameOver.SetActive(true);
        gameOver.GetComponent<AudioSource>().time = 0.2f;
        gameOver.GetComponent<AudioSource>().Stop();
        gameOver.GetComponent<AudioSource>().Play();
        
        ShowWinOrLoseText(resule);
       // PauseGame();
    }

    //显示胜败Text
    public void ShowWinOrLoseText(bool isWin)
    {
        winOrLoseText.enabled = true;
        if (isWin)
        {
            winOrLoseText.text = "You Win!";
        }
        else
        {
            winOrLoseText.text = "You Lose!";
        }    
    }

    //隐藏胜败Text
    public void HideWinOrLoseText()
    {
        winOrLoseText.enabled = false;
    }

    //显示受伤红色画面效果
    public void ShowDamageImage()
    {
        damageImage.enabled = true;
        Invoke("HideDamageImage", 0.1f);
    }

    //隐藏受伤红色画面效果
    private void HideDamageImage()
    {
        damageImage.enabled = false;
        CancelInvoke();
    }

    //获得技能enable
    public bool GetSkillButtonEnable(int index)
    {
        return skillButton[index].enabled;
    }

    //更新技能
    public void UpdateSkillButton()
    {
        for (int i = 0; i < skillButton.Length; i++)
        {
            if (skillCountDownSlider[i].value > 0)
            {
                skillButton[i].enabled = false;
            }
            else
            {
                skillButton[i].enabled = true;
            }
        }
    }

    //初始化节能冷却时间
    private void InitSkillCountDownTime()
    {
        for (int i = 0; i < skillCountDownSlider.Length; i++)
        {
            skillCountDownSlider[i].maxValue = 0;
            skillCountDownSlider[i].value = 0;
        }       
    }

    //重置技能冷却时间
    public void ResetSkillCountDownTime(int index)
    {
        skillCountDownSlider[index].maxValue = skillTime[index];
        skillCountDownSlider[index].value = skillTime[index];
    }

    //更新冷却时间
    public void UpdateSkillCountDownSlider()
    {
        for(int i=0;i<skillButton.Length;i++)
        {
            if(skillCountDownSlider[i].value>0)
            {
                skillCountDownSlider[i].value -= Time.deltaTime;
            }
        }
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
