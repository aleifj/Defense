using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Timeline;
using UnityEngine;

public enum WeaponState//타워 상태
{
    SearchTarget,//적 탐색
    AttactToTarget//적 공격
}
public class TowerWeapon : MonoBehaviour
{
    [SerializeField] private GameObject projectilePreFab;//발사체 프리팹.
    [SerializeField] private Transform spawnPoint;//탄환 생성 위치.
    [SerializeField] private float attactRate = 5.0f;//탄환 발사 간격.
    [SerializeField] private float attackRange = 2.0f;//탄환 생성 범위.
    [SerializeField] private float attakDamage = 1.0f;//공격력
    private WeaponState weaponState = WeaponState.SearchTarget;
    private Transform attackTarget = null;//공격 목표.
    public void Init()
    {
        ChangeState(WeaponState.SearchTarget);//적 탐색으로 초기화
    }
    private void ChangeState(WeaponState newState)
    {
        StopCoroutine(weaponState.ToString());//이름을 코루틴 파라미터로 설정할 수 있는 것을 이용하여 현재 코루틴 종료
        weaponState = newState;//상태 바꾸기
        StartCoroutine(weaponState.ToString());//새로운 코루틴 시작
    }
    void Update()
    {
        //공격 중
        if(attackTarget != null)
        {
            //타워 회전(위를 향하고 있는 스프라이트이므로 up이다)
            transform.up = attackTarget.position - transform.position;
        }
    }
    private IEnumerator SearchTarget()
    {
        while(true)
        {
            //가장 가까운 거리를 찾기 위해 가장 큰 수로 먼저 초기화.
            float closestDistance = Mathf.Infinity;
            //리스트에 있는 모든 오브젝트 검사.
            foreach (var item in EnemyManager.instance.EnemyList)
            {
                //두 오브젝트 사이의 거리를 측정
                float bothObject = Vector3.Distance(item.transform.position, this.transform.position);
                //공격 사정거리 안에 있ㅇ면서 가장 긴 거리보다 작으면 
                if((bothObject < attackRange) && (bothObject <= closestDistance))
                {
                    //현재 거리를 최단 거리로 지정
                    closestDistance = bothObject;
                    //공격 목표를 현재 오브젝트로 지정
                    attackTarget = item.transform;
                }
                    

            }
            //공격 목표가 null이 아니면
            if(attackTarget != null)
            {
                //공격 상태로 변경.
                ChangeState(WeaponState.AttactToTarget);
            }
        //코루틴로 계속 적 찾기
        yield return null;
        }

    }
    private IEnumerator AttactToTarget()
    {
        while(true)
        {
            //공격 목표가 사라지면
            if(attackTarget == null)
            {
                //적 찾기 상태로 변경
                ChangeState(WeaponState.SearchTarget);
                //바로 while문 빠져 나오기
                break;
            }
            //적과의 거리 측정
            float distance = Vector3.Distance(attackTarget.position, this.transform.position);
            //거리가 공격범위를 벗어나면
            if(distance > attackRange)
            {
                //공격 목표를 없에고
                attackTarget = null;
                //적 찾기 상태로 변경
                ChangeState(WeaponState.SearchTarget);
                //바로 while문 빠져 나오기
                break;
            }
            //발사간격만큼 기다린 후 다시 공격
            yield return new WaitForSeconds(attactRate);
            //발사체 생성
            SpawnProjectile();
        }
        
    }
    private void SpawnProjectile()
    {
        //발사체프리팹서 탄환 생성
        GameObject clone = Instantiate(projectilePreFab, spawnPoint.position, Quaternion.identity, transform);
        //발사체에 공격 목표 설정
        clone.GetComponent<Projectile>().SetTarget(attackTarget, attakDamage);
    }
}
