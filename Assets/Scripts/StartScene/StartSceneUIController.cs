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
    public Text musicControlText;

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

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
