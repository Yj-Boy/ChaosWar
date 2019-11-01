﻿/*
 *  Loading场景的Loading效果
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Progressbar : MonoBehaviour
{
    static string nextLevel;            //下一关的场景名
    AsyncOperation asyn;
    public Slider slider;               //进度条
    public Text text;                   //进度

    private float tempProgress;

    private void Start()
    {
        tempProgress = 0;
        if (SceneManager.GetActiveScene().name == "LoadingScene")//如果当前活跃场景是Loading，异步加载下一个场景
        {
            asyn = SceneManager.LoadSceneAsync(nextLevel);
            //把allowSceneActivation设置为false后，
            //Unity就只会加载场景到90 %，剩下的10%要等到allowSceneActivation设置为true后才加载
            asyn.allowSceneActivation = false;
        }
    }

    public void LoadLoadingLevel(string nextLevelName)//加载Loading界面，并传入下一个场景名
    {
        nextLevel = nextLevelName;
        SceneManager.LoadScene("LoadingScene");
    }

    private void Update()
    {
        if (text && slider)
        {
            //更新Loading进度条和加载数字
            tempProgress = Mathf.Lerp(tempProgress, asyn.progress, Time.deltaTime);
            text.text = ((int)(tempProgress / 9 * 10 * 100)).ToString() + "%";
            slider.value = tempProgress / 9 * 10;

            if (slider.value >= 0.99)
            {
                tempProgress = 1;
                text.text = 100 + "%";
                slider.value = 100 / 9 * 10;
                asyn.allowSceneActivation = true;
            }

        }
    }
}