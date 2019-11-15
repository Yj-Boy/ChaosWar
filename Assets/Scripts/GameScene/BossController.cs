using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum SkillState
{
    normal,
    launching
};
public class BossController : MonoBehaviour
{
    public Animator bossAnimator;
    public float mainSkillTime;
    public float continueSkillTime;
    private float tmpSkillTime;
    private SkillState skillState;

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
        //mainSkillTime += Time.deltaTime;
        //到达释放大招的时间和条件

        //释放大招动画
        //大招动画完触发大招

        if (CreateBossMainSkill.GetLaunch())
        {
            tmpSkillTime += Time.deltaTime;
            if(tmpSkillTime>continueSkillTime)
            {
                CreateBossMainSkill.Stop();
                bossAnimator.SetBool("BackBossAttack", true);
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
            CreateBossMainSkill.Launch();
            Debug.Log("MainSkillLaunch!");
        }
    }

    void AnimatorToIdle()
    {
        bossAnimator.SetBool("BackBossAttack", false);
    }
}
