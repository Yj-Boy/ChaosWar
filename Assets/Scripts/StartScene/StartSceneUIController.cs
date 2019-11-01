/*
 *  开始场景的UI相关操作 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneUIController : MonoBehaviour
{
    public Text musicControlText;       //音乐开关文字提示

    //开始游戏
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    //结束游戏
    public void ExitGame()
    {
        Application.Quit();
    }

    //背景音乐控制开关
    public void AudioControl(AudioSource audioSource)
    {
        if(audioSource.isPlaying)
        {
            audioSource.Stop();
            musicControlText.text = "音乐：关";
        }
        else
        {
            audioSource.Play();
            musicControlText.text = "音乐：开";
        }
    }
}
