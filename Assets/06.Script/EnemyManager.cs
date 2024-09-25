using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnTime;
    [SerializeField] private Transform[] wayPoints;//이동위치 배열.
    private List<Enemy> enemyList;//생성된 적 리스트.
    public List<Enemy> EnemyList => enemyList;//람다식, 생성된 적 리스트 프로퍼티.
    public Transform[] WayPoints => wayPoints;//이동위치배열의 프로퍼티.
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    IEnumerator Start()//start는 코루틴으로 사용 할 수 있다.
    {
        enemyList = new List<Enemy>();
        while (true)
        {
            //적 프레팝으로 오브젝트를 생성하고 enemy 스크립트 연결
            Enemy enemy = Instantiate(enemyPrefab, transform).GetComponent<Enemy>();
            //적 초기화
            enemy.Init();
            //적을 리스트에 넣기
            enemyList.Add(enemy);
            yield return new WaitForSeconds(spawnTime);//생성시간 기다렸다 다음 적 생성
        }
    }
    public void DestroyEnemy(Enemy enemy)
    {
        //적 리스트에서 지정한 적 지우기
        enemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }
}
