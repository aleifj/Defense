using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private TowerTemplate towerTemplate;
    [SerializeField] private GameObject projectilePreFab;//발사체 프리팹.
    [SerializeField] private Transform spawnPoint;//탄환 생성 위치.
    private WeaponState weaponState = WeaponState.SearchTarget;
    private Transform attackTarget = null;//공격 목표.
    private SpriteRenderer spriteRenderer;
    private int level = 0;//타워 레벨
    
    #region Property
    public Sprite TowerSprite => towerTemplate.weapon[level].sprite;//레벨에 따른 타워 이미지 프로퍼티
    public int Level => level +1;//타워 레벨 프로퍼티
    public float Damage => towerTemplate.weapon[level].damage;//공격력 프러퍼티
    public float Rate => towerTemplate.weapon[level].rate;//발사간격 프로퍼티
    public float Range => towerTemplate.weapon[level].range;//생성범위 프로퍼티
    public int CostUpgrade => Level < MaxLevel ? towerTemplate.weapon[level + 1].cost : 0;//
    public int CostSell => towerTemplate.weapon[level].sell;//판매비용 프로퍼티
    public int MaxLevel => towerTemplate.weapon.Length;//최대 레벨

    #endregion Property


    /// <summary>
    /// 타워 생성 후 초기화로 반드시 한번 호출
    /// </summary>
    public void Init()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ChangeState(WeaponState.SearchTarget);//적 탐색으로 초기화
    }
    private void ChangeState(WeaponState newState)
    {
        StopCoroutine(weaponState.ToString());//이름을 코루틴 파라미터로 설정할 수 있는 것을 이용하여 현재 코루틴 종료
        weaponState = newState;//상태 바꾸기
        StartCoroutine(weaponState.ToString());//새로운 코루틴 시작
    }
    private void Update()
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
                if((bothObject < towerTemplate.weapon[level].range) && (bothObject <= closestDistance))
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
            if(distance > towerTemplate.weapon[level].range)
            {
                //공격 목표를 없에고
                attackTarget = null;
                //적 찾기 상태로 변경
                ChangeState(WeaponState.SearchTarget);
                //바로 while문 빠져 나오기
                break;
            }
            //발사간격만큼 기다린 후 다시 공격
            yield return new WaitForSeconds(towerTemplate.weapon[level].rate);
            //발사체 생성
            SpawnProjectile();
        }
        
    }
    private void SpawnProjectile()
    {
        //발사체프리팹서 탄환 생성
        GameObject clone = Instantiate(projectilePreFab, spawnPoint.position, Quaternion.identity, transform);
        //발사체에 공격 목표 설정, 공격력도 전달
        clone.GetComponent<Projectile>().SetTarget(attackTarget, towerTemplate.weapon[level].damage);
    }

    /// <summary>
    /// 타워가 업그레이드 가능한지 검사하고 가능하면 업그레이드.
    /// </summary>
    /// <returns>업그레이드 가능 여부</returns>
    public bool UpGrade()
    {
        //가진 돈이 현재 레벨보다 1 큰 비용보다 적은지 검사
        if(PlayerManager.instance.CurrentGold < towerTemplate.weapon[level + 1].cost)
        {
            return false;//실패 리턴
        }
        level++;
        spriteRenderer.sprite = towerTemplate.weapon[level].sprite;
        PlayerManager.instance.CurrentGold -= towerTemplate.weapon[level].cost;
        return true;
    }

    /// <summary>
    /// 타워 판매
    /// </summary>
    public void Sell()
    {
        //판매 비용 추가
        PlayerManager.instance.CurrentGold += towerTemplate.weapon[level].sell;
        //타워 오브젝트 삭제
        Destroy(gameObject);
    }
}
