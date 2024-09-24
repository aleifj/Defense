using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject TowerPrefeb;
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
        Instantiate(TowerPrefeb,tileTransform.position, Quaternion.identity);
    }
}
