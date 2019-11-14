using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public Animator bossAnimator;
    public float mainSkillTime;
    public float continueSkillTime;
    private float tmpSkillTime;

    // Start is called before the first frame update
    void Start()
    {
        if(bossAnimator==null)
        {
            Debug.Log(GetType() + "/Start/Boss状态机BossAnimator没有指定！");
        }
        if(mainSkillTime==0f)
        {
            Debug.Log(GetType() + "/Start/Boss大招时间mainSkillTime没有设定或者为0！");
        }

        tmpSkillTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LaunchBossMainSkill()
    {
        if(mainSkillTime>=3f)
        {
            bossAnimator.SetBool("Atack", true);
            GameObject.Find("_ECSScript").GetComponent<CreateBossMainSkill>().Launch();
        }
    }
}
