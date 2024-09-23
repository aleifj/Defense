using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float MoveSpeed = 1.0f;
    private int currentIndex;//현재 경로 인덱스.
    private EnemyManager emi;//너무 길어서 요약함.
    public void Init()
    {
        emi = EnemyManager.instance;//요약 여기서 선언함.
        currentIndex = 0;//인덱스 0시작.
        //위치는 시작인덱스에서 해당하는 경로 위치롤 지정
        transform.position = emi.WayPoints[currentIndex].position;
    }
    private void Update()
    {
        //이동지점 배열 인덱스. 0 부터 배열크기 -1까지
        if(currentIndex < emi.WayPoints.Length)
        {
            //현재 위치를 frame처리시간비율로 계산한 속도만큼 옮겨줌.
            transform.position = Vector3.MoveTowards(transform.position, emi.WayPoints[currentIndex].position, MoveSpeed * Time.deltaTime);
            //현재위치가 이동지점의 위치라면 배열 인덱스+1하여 다음 포인트로 이동하도록 만듬
            if(Vector3.Distance(emi.WayPoints[currentIndex].position, transform.position) == 0f)
            {
                currentIndex++;
            }
        }
        else
        {
            OnDie();//목표에 도달하면 삭제.
        }
    }
    public void OnDie()
    {
        EnemyManager.instance.DestroyEnemy(this);//메니저에서 삭제 처리.
    }
}
