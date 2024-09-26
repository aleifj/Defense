using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEditor;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    [SerializeField] private GameObject[] enemyPrefab;
    [SerializeField] private GameObject enemyHPSliderPrefab;//체력을 나타내는 프리팹
    [SerializeField] private Transform canvasTransform;//UI를 나타낼 transform
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
            int index = Random.Range(0,enemyPrefab.Length);
            //적 프레팝으로 오브젝트를 생성하고 enemy 스크립트 연결
            Enemy enemy = Instantiate(enemyPrefab[index], transform).GetComponent<Enemy>();
            //적 초기화
            enemy.Init();
            //적을 리스트에 넣기
            enemyList.Add(enemy);
            //적 체력 슬라이드 표시
            SpawnEnemyHPSlider(enemy);

            yield return new WaitForSeconds(spawnTime);//생성시간 기다렸다 다음 적 생성
        }
    }
    public void DestroyEnemy(Enemy enemy)
    {
        //적 리스트에서 지정한 적 지우기
        enemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }

    private void SpawnEnemyHPSlider(Enemy enemy)
    {
        //슬라이더 클론 생성
        GameObject sliderClone = Instantiate(enemyHPSliderPrefab, canvasTransform);
        //크기 지정
        sliderClone.transform.localScale = Vector3.one;
        //위치지정
        sliderClone.GetComponent<SliderPosAuto>().Setup(enemy.transform);
        //값 지정
        sliderClone.GetComponent<EnemyHPViewer>().Setup(enemy);
    }
}
