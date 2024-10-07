using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class TowerSpawner : MonoBehaviour
{
    [SerializeField] private TowerTemplate towerTemplate;
    [SerializeField] private infoTower infoTower; // 타워정보 패널
    [SerializeField] private ToastMessage ToastMSG;//토스트 메시지
    private ContactFilter2D filter;//raycast용 파라미터
    private List<RaycastHit2D> rcList;//Raycast결과 저장용 리스트.
    private bool isOnTowerButton = false;//타워건설버튼 굴렸는지 체크
    private GameObject followTowerClone = null;//임시타워 사용완료시 삭제를 위해 저장하는 변수.
    
    void Start()
    {
        filter = new ContactFilter2D();//파라미터 생성
        rcList = new List<RaycastHit2D>();//리스트 생성
    }
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(EventSystem.current.IsPointerOverGameObject() == true)//마우스가 UI에 있을 때 리턴.
            {
                return;
            }

            rcList.Clear();//리스트를 클리어 먼저해야하나?
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);//카메라 위치기준 월드포지션값?
            Physics2D.Raycast(worldPosition, Vector2.zero, filter, rcList);//ray를 쏴서 검출되는 오브젝트 찾기

            foreach(var item in rcList)
            {
                if(item.transform.CompareTag("TOWER"))//TOWER태그인 아이템이 있으면
                {
                    infoTower.OnPanel(item.transform);// 타워 정보 패널에 표시할 정보를 넘기고 패널 켜기

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
        //타워건설버튼을 눌렀을 때만 건설 가능
        if(isOnTowerButton == false)
        {
            return;
        }
        //다시 건설버튼 눌러서 건설하도록 설정
        isOnTowerButton = false;
        //소지골드에서 타워템플릿에 등록된[0번에] 금액 차감
        PlayerManager.instance.CurrentGold = PlayerManager.instance.CurrentGold - towerTemplate.weapon[0].cost;

        //타워 뎀플릿에 있는 타워 프리펩으로 생성
        GameObject clone = Instantiate(towerTemplate.towerPrefab, tileTransform.position, Quaternion.identity, transform);
        //타워 무기 초기화.
        clone.GetComponent<TowerWeapon>().Init();
        //임시타워 삭제
        Destroy(followTowerClone);
        //
        StartCoroutine(OnTowerCancleSystem());
    }
    public void ReadyToSpawnTower()
    {
        //버튼 중복해서 누르는 경우 방지
        if(isOnTowerButton)
        {
            return;
        }

        //타워템플릿에 등록된[0번에]금액이 소지골드보다 크면 리턴
        if(towerTemplate.weapon[0].cost > PlayerManager.instance.CurrentGold)
        {
            //골드부족으로 건설 불가 메세지 출력.
            ToastMSG.ShowToast(ToastType.Money);//토스트 메시지 표시방법 ShowToast();
            return;
        }
        //타워 건설버튼 눌렸다고 설정
        isOnTowerButton = true;
        //마우스를 따라다니는 임시타워 생성
        followTowerClone = Instantiate(towerTemplate.followPrefab);
        //타워 건설 취소하는 코루틴 시작.
        StartCoroutine(OnTowerCancleSystem());
    }
    private IEnumerator OnTowerCancleSystem()
    {
        while (true)
        {
            //esc 또는 마우스 우클릭 시 타워 건설 취소.
            if(Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
            {
                isOnTowerButton = false;
                Destroy(followTowerClone);
                break;
            }
            yield return null;
        }
    }
}
