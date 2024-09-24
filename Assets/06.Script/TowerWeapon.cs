using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private float attactRate;//탄환 발사 간격.
    [SerializeField] private float attackRange = 2.0f;//탄환 생성 범위.
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
        
    }
    private IEnumerator SearchTarget()
    {
        while(true)
        {
            //가장 가까운 거리를 찾기 위해 가장 큰 수로 먼저 초기화.
            float closestDistance = Mathf.Infinity;
            //리스트에 있는 모든 오브젝트 검사.
            
            
        }
        yield return null;
    }
    private IEnumerator AttactToTarget()
    {
        while(true)
        {
            
        }
        yield return null;
    }
    private void SpawnProjectile()
    {
        //발사체프리팹서 탄환 생성
        //발사체에 공격 목표 설정
    }
}
