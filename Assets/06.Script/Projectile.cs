using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 6.0f;//탄환속도
    private Transform target;//공격 목표.
    private float damage = 0f;//데미지.
    public void SetTarget(Transform tr, float indamage)
    {
        target = tr;//공격목표 지정.
        this.damage = indamage;//데미지 값 설정
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
        if(!collision.CompareTag("ENEMY"))//태그가 적이 아니면 리턴
        {
            return;
        }
        if(collision.transform != target)//충돌한 오브젝트가 목표가 아니면 리턴
        {
            return;
        }
        collision.GetComponent<Enemy>().TakeDamage(damage);//충돌한 적에게 데미지 주기
        Destroy(gameObject);
    }
}
