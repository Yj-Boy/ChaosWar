using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyList : MonoBehaviour
{
    public GameObject enemy;

    List<GameObject> enemyList = new List<GameObject>();

    private void Start()
    {
        enemyList.Add(enemy);
    }
}
