using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 6.0f;//탄환속도
    private Transform target;//공격 목표.
    public void SetTarget(Transform tr)
    {
        target = tr;//공격목표 지정.
    }
    void Start()
    {
        
    }
    void Update()
    {
        if(target != null)//목표가 있으면
        {
            Vector3 direction = (target.position - transform.position).normalized;//방향 설정
            transform.position += moveSpeed * Time.deltaTime * direction;//이동
        }
        else//목표가 없으면
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag("ENEMY"))
        {
            return;
        }
        if(collision.transform != target)
        {
            return;
        }
        collision.GetComponent<Enemy>().OnDie();
        Destroy(gameObject);
    }
}
