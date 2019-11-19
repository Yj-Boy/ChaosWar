﻿/*
 *  Boss控制脚本 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public Animator bossAnimator;           //Boss状态机
    public float mainSkillTime;             //Boss大招释放到结束总时间
    private float tmpSkillTime;             //Boss大招释放开始累计时间

    // Start is called before the first frame update
    void Start()
    {
        //参数检查
        if(bossAnimator==null)
        {
            Debug.Log(GetType() + "/Start/Boss状态机BossAnimator没有指定！");
        }
        if(mainSkillTime == 0f)
        {
            Debug.Log(GetType() + "/Start/Boss大招时间MainSkillTime没有设定或者为0！");
        }

        tmpSkillTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("_ECSscript").GetComponent<CreateBossMainSkill>().GetLaunch())
        {
            tmpSkillTime += Time.deltaTime;
            if(tmpSkillTime>mainSkillTime)
            {
                //CreateBossMainSkill.Stop();
                GameObject.Find("_ECSscript").GetComponent<CreateBossMainSkill>().Stop();
                bossAnimator.SetBool("BackBossAttack", true);
                bossAnimator.SetBool("BossAttack", false);
                tmpSkillTime = 0f;
                Debug.Log("BossAttackFalse");
            }
        }
    }

    void LaunchBossMainSkill()
    {
        int rangeNum = Random.Range(0, 100);
        if(rangeNum>0&&rangeNum<=30)
        {
            bossAnimator.SetBool("BossAttack", true);
            //CreateBossMainSkill.Launch();
            GameObject.Find("_ECSscript").GetComponent<CreateBossMainSkill>().Launch();
            Debug.Log("MainSkillLaunch!");
        }
    }

    void AnimatorToIdle()
    {
        bossAnimator.SetBool("BackBossAttack", false);
    }
}
