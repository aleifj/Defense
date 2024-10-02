using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField] private TowerTemplate towerTemplate;
    [SerializeField] private GameObject TowerPrefeb;//타워 프리팹 연결
    private ContactFilter2D filter;//raycast용 파라미터
    private List<RaycastHit2D> rcList;//Raycast결과 저장용 리스트.
    
    void Start()
    {
        filter = new ContactFilter2D();//파라미터 생성
        rcList = new List<RaycastHit2D>();//리스트 생성
    }
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            rcList.Clear();//리스트를 클리어 먼저해야하나?
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);//카메라 위치기준 월드포지션값?
            Physics2D.Raycast(worldPosition, Vector2.zero, filter, rcList);//ray를 쏴서 검출되는 오브젝트 찾기
            foreach(var item in rcList)
            {
                if(item.transform.CompareTag("TOWER"))//TOWER태그인 아이템이 있으면
                {
                    return;
                }
            }
            foreach(var item in rcList)
            {
                if(item.transform.CompareTag("TILE"))//TILE태그인 아이템이 있으면
                {
                    SpawnTower(item.transform);//그 자리에 타워 생성
                }
            }
        }
    }
    private void SpawnTower(Transform tileTransform)
    {
        //타워템플릿에 등록된[0번에]금액이 소지골드보다 크면 리턴
        if(towerTemplate.weapon[0].cost > PlayerManager.instance.CurrentGold)
        {
            //todo 건설 불가 메세지 출력.
            return;
        }
        //소지골드에서 타워템플릿에 등록된[0번에] 금액 차감
        PlayerManager.instance.CurrentGold = PlayerManager.instance.CurrentGold - towerTemplate.weapon[0].cost;

        //타워 프리팹으로 타워 생성
        GameObject clone = Instantiate(TowerPrefeb,tileTransform.position, Quaternion.identity, transform);
        //타워 무기 초기화.
        clone.GetComponent<TowerWeapon>().Init();
    }
}
