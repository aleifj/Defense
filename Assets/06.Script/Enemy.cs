using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float MoveSpeed = 1.0f;
    [SerializeField] private float maxHP = 5.0f;//최대 체력
    [SerializeField] private int gold = 10;//적 처치시 획득골드
    private bool isDie;//사망상태
    private int currentIndex;//현재 경로 인덱스.
    private float currentHP;//현재 체력
    private Animator anim;//에니매이션 제어용 에니매이터
    private EnemyManager emi;//너무 길어서 요약함.
    private SpriteRenderer spriteRenderer;
    public float MaxHP => maxHP;//최대 체력 프로퍼티
    public float CurrentHP => currentHP;//현재 체력 프로퍼티.

    /// <summary>
    /// 적을 생성한 후 반드시 처음에 한번은 호출해줘야 함. 적을 초기화.
    /// </summary>
    public void Init()
    {

        emi = EnemyManager.instance;//요약 여기서 선언함.
        currentIndex = 0;//인덱스 0시작.
        //위치는 시작인덱스에서 해당하는 경로 위치롤 지정
        transform.position = emi.WayPoints[currentIndex].position;
        //애니메이터 연결
        anim = GetComponent<Animator>();
        //스프라이트렌더러 연결
        spriteRenderer = GetComponent<SpriteRenderer>();
        //현재 체력을 최대치로 초기화
        currentHP = maxHP;
        //살아있는 상태로 시작
        isDie = false;
    }
    private void Update()
    {
        //이동지점 배열 인덱스. 0 부터 배열크기 -1까지
        if(currentIndex < emi.WayPoints.Length)
        {
            //현재 위치를 frame처리시간비율로 계산한 속도만큼 옮겨줌.
            transform.position = Vector3.MoveTowards(transform.position, emi.WayPoints[currentIndex].position, MoveSpeed * Time.deltaTime);

            //현재 오브젝트가 어느방향으로 이동하는 지 검사
            Vector3 vec = emi.WayPoints[currentIndex].position - transform.position;

            //MoveTowards에서 target - current를 뺀 값의 x가 0보다 큰지 작은지 판단.
            if((vec.x > 0) || (vec.y >0))
            {
                //0보다 크면 오른쪽이므로 spriteRender의 FlipX를 true.
                spriteRenderer.flipX = true;
            }
            else if((vec.x < 0) || (vec.y <0))
            {
                spriteRenderer.flipX = false;
            }
            //spriteRenderer.flipX = (vec.x > 0) || (vec.y > 0) ? true : false;삼항연산자

            //현재위치가 이동지점의 위치라면 배열 인덱스+1하여 다음 포인트로 이동하도록 만듬
            if(Vector3.Distance(emi.WayPoints[currentIndex].position, transform.position) == 0f)
            {
                currentIndex++;
            }
        }
        else
        {
            OnDie(true);//목표에 도달하면 삭제.
        }
    }
    /// <summary>
    /// 적이 Goal에 도달하거나 체력이 다해 죽을 경우 호출.
    /// </summary>
    /// <param name="isArrivedGoal">Goal에 도착했는지 여부.</param>
    public void OnDie(bool isArrivedGoal = false)
    {
        EnemyManager.instance.DestroyEnemy(this, gold, isArrivedGoal);//메니저에서 삭제 처리하면서 골드 처리.
    }
    /// <summary>
    /// 적에게 딜 넣기.
    /// </summary>
    /// <param name="damage">데미지 양</param>
    public void TakeDamage(float damage)
    {
        if(isDie)
        {//죽은 생태이면 더 이상 데미지를 받지 않도록 리턴
            return;
        }
        if(!isDie) 
        {//데미지만큼 현재 체력 감소
            currentHP = currentHP - damage;
        }
        if(currentHP <= 0)//체력이 0 이하인지 검사
        {
            isDie = true;//죽은 상태로 만들고
            OnDie();//삭제
        }
        else//아니면
        {
            anim.SetTrigger("HIT");//피격 애니메이션 실행
        }
    }
}
