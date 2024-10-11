using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow : MonoBehaviour
{
    private TowerWeapon towerWeapon;
    void Start()
    {
        towerWeapon = GetComponentInParent<TowerWeapon>();
    }

    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag("ENEMY"))
        {
            return;
        }
        //적일 경우 slow 값을 적에게 적용
        collision.GetComponent<Enemy>().Slow = towerWeapon.Slow;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(!collision.CompareTag("ENEMY"))
        {
            return;
        }
        //적일 경우 slow 값을 적에게 적용
        collision.GetComponent<Enemy>().Slow = 0.0f;
    }
}
