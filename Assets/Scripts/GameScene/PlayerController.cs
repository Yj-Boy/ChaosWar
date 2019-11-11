using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public Animator animator;

    Vector3 pos;
    Vector3 rotate;
    float h;
    private void Start()
    {
        pos = transform.position;
        rotate=transform.rotation.eulerAngles;
        Debug.Log(rotate);
    }
    private void Update()
    {
        MoveHero();
    }
    private void MoveHero()
    {
        //if (Input.GetKey(KeyCode.A))
        //{
        //    pos.x -= moveSpeed * Time.deltaTime;
        //    transform.position = pos;
        //}
        //else if (Input.GetKey(KeyCode.D))
        //{
        //    pos.x += moveSpeed * Time.deltaTime;
        //    transform.position = pos;
        //}

        h= Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        if(h<0)
        {
            pos.x += h;
            rotate.z = Mathf.LerpAngle(0, -90, 2);
            animator.SetBool("Walk", true);
        }
        else if(h>0)
        {
            pos.x += h;
            rotate.z = Mathf.LerpAngle(0, 90, 2);
            animator.SetBool("Walk", true);
        }
        else
        {
            rotate.z = Mathf.LerpAngle(90, 0, 2);
            animator.SetBool("Walk", false);
        }
        transform.position = pos;
        transform.rotation = Quaternion.Euler(rotate);
    }

    public void LaunchThousandsOfTroops()
    {
        GameObject.Find("_ECSscript").GetComponent<CreatThousandsOfTroops>().SpawnEntity();
        animator.SetTrigger("Attack");
        Debug.Log("LaunchThousandsOfTroops");
    }
}
