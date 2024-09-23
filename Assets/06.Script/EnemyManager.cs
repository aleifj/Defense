using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnTime;
    [SerializeField] private Transform[]wayPoints;//이동위치 배열.
    public Transform[] WayPoints => wayPoints;//이동위치배열의 프로퍼티.
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        //적 프레팝으로 오브젝트를 생성하고 enemy 스크립트 연결
        Enemy enemy = Instantiate(enemyPrefab, transform).GetComponent<Enemy>();
        //적 초기화
        enemy.Init();
    }
    public void DestroyEnemy(Enemy enemy)
    {
        Destroy(enemy.gameObject);
    }
}
