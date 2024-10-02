using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]//유니티 에디터 메뉴에 나오게 된다.
public class TowerTemplate : ScriptableObject//모노비헤이비 뭐시기 아님.
{
    public GameObject towerPrefab;//타워 생성을 위한 프리팹
    public Weapon[] weapon;//레벨 별 타워 정보
    
    [System.Serializable]
    public struct Weapon
    {
        public Sprite sprite;//타워이미지
        public float damage;//공격력
        public float rate;//공격속도
        public float range;//공격범위
        public int cost;//건설필요골드
        public int sell;//판매골드
    }
}
