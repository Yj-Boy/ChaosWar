/*
 *  enemy射击脚本 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyShoot : MonoBehaviour
{
    public float timeBetweenAttack;                 //射击的时间间隔
    public LineRenderer lineRender;                 //射线组件
    public Transform lineTrans;                     //射线位置
    public ParticleSystem shootParticle;            //射击特效

    private Transform troopShooter;                 //射击目标的父对象
    private Transform targetTroopShooter;           //射击目标
    private int troopShooterNum;                    //射击目标数量
    private int index;                              //射击目标索引
    private float timer;                            //距离射击间隔的时间
    private float shootTimer;
    private Vector3 tmpShootTargetV;
    private float lineTimer;

    private bool isShooting;

    // Start is called before the first frame update
    void Start()
    {
        troopShooter = GameObject.Find("TroopShooter").transform;
        shootParticle.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        troopShooterNum = troopShooter.childCount;
    }

    //获得射击目标数量公有接口
    public int GetTroopShooterNum()
    {
        return troopShooterNum;
    }

    //射击接口
    public void Shoot()
    {
        lineRender.enabled = false;
        //如果射击目标数量大于0
        //如果当前射击目标为空，随机从射击目标对象中赋予一个射击目标
        if (troopShooterNum > 0 && targetTroopShooter == null)
        {
            GetComponent<NavMeshAgent>().enabled = false;

            index = Random.Range(0, troopShooterNum);
            targetTroopShooter = troopShooter.GetChild(index);
            tmpShootTargetV = lineTrans.position;

        }
        else
        {
            if (targetTroopShooter != null)
            {
                //若当前射击目标不为空，将enemy旋转至朝向射击目标
                Vector3 targetVec = targetTroopShooter.position - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(
                    targetVec
                    );
                //Debug.Log("targetRotation:" + targetRotation);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    2.5f * Time.deltaTime);
                //Debug.Log("transform.rotation:" + transform.rotation);

                //Debug.Log("angle:" + Vector3.Angle(transform.forward, targetVec));
                if (Vector3.Angle(transform.forward, targetVec) <= 12)
                {

                    //累计timer，若大于射击间隔，进行一次射击并造成伤害
                    timer += Time.deltaTime;
                    if (timer >= timeBetweenAttack)
                    {
                        lineRender.SetPosition(0, lineTrans.position);
                        shootParticle.Play();
                        GetComponent<Animator>().SetTrigger("DevilHeadShoot");
                        
                        //如果射击目标的血量小于0，则将当前射击目标设置为空
                        if (targetTroopShooter.GetComponent<TroopsHealth>().currentHealth <= 0)
                        {
                            lineRender.enabled = false;
                            targetTroopShooter = null;
                        }

                        timer = 0f;
                    }
                }
            }
        }



        if (isShooting)
        {
            //Debug.Log("shootParticle.main.duration:"+ shootParticle.main.duration);
            //if (shootTimer >= shootParticle.main.duration)
            //{
            //Debug.Log("shoot");
            lineRender.enabled = true;
            shootParticle.Stop();

            lineTimer += Time.deltaTime;
            if (lineTimer <= 0.5f)
            {
                //tmpShootTargetV = Vector3.Lerp(lineTrans.position, targetTroopShooter.position, 3f * Time.deltaTime);
                tmpShootTargetV += (targetTroopShooter.position - lineTrans.position).normalized * Time.deltaTime * 90f;

                lineRender.SetPosition(1, tmpShootTargetV);
                //Debug.Log("lineShoot");
            }
            else
            {
                lineRender.enabled = false;
                targetTroopShooter.GetComponent<TroopsHealth>().TakeDamage(20);
                targetTroopShooter.GetComponent<TroopShooterController>().SetStateToGetHit();
                lineTimer = 0f;
                tmpShootTargetV = lineTrans.position;
                isShooting = false;      
                GetComponent<NavMeshAgent>().enabled = true;
            }

            //}
        }
        else
        {
            lineRender.enabled = false;
        }
    }

    public void AnimToShoot()
    {
        isShooting = true;
    }
}
