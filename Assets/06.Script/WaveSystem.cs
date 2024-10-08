using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//유니티 에디터에서 사용하거나 파일로 저장하기 위해 시리얼라이즈
[System.Serializable]
public struct Wave
{
    public bool isRandom; //랜덤인지 아닌지
    public float spawnTiem;//적 생성 주기
    public int maxEnemyCount;//적 최대 숫자
    public int runMode;//런모드 발생 시기
    public GameObject[] enemyPrefabs;//적 종류
    public float[] spawnTimeStatic;//고정일 때 적 생성 주기
}
//JSON사용을 위한 wrapper class
public class WaveWrapper
{
    public Wave[] waveArray;
}

public class WaveSystem : MonoBehaviour
{
    private string jsonFileName = "waves.json";
    [SerializeField] private Wave[] waves;//웨이브 배열
    private int currentWaveIndex = -1;//0으로 설정하면 시작해 버리니 초기값은-1, 현재 웨이브 인덱스
    /// <summary>
    /// 현재 인댁스에 해당하는 웨이브 실행.
    /// </summary>
    public void StartWave()
    {
        //적이 없고 웨이브가 남아 있다면 가능
        if((EnemyManager.instance.EnemyList.Count == 0) && (currentWaveIndex < waves.Length - 1))
        {
            //웨이브 인댁스를 하나 증가시킨 후
            currentWaveIndex++;
            //현재 인덱스에 해당하는 웨이브를 실행
            EnemyManager.instance.StartWave(waves[currentWaveIndex]);
        }
    }
    /// <summary>
    /// [현재 웨이브 / 총 웨이브] 문자열 얻어오기
    /// </summary>
    /// <returns></returns>
    public string GetWaveInfoString()
    {
        return $"{currentWaveIndex +1}\n---\n{waves.Length}";
    }

    //에디터의 상단 메뉴가 아닌 waveSystem이 붙어있는 콤포넌트에서 메뉴가 열림.
    [ContextMenu("defenseTower/Make Json data", false, 1)]
    public void MakeJSONData()//json 공부 필요.
    {
        //저장 데이터가 배열인 경우 wrapper class로 감싸기
        WaveWrapper data = new WaveWrapper();
        data.waveArray = waves;

        string jsonDate = JsonUtility.ToJson(data, true);
        string path = Path.Combine(Application.dataPath, jsonFileName);
        File.WriteAllText(path, jsonDate);
        Debug.Log("Make JSON data is done.");
    }

    [ContextMenu("defenseTower/Load From Json data", false, 2)]
    public void LoadFromJSON()
    {
        string path = Path.Combine(Application.dataPath, jsonFileName);
        string JsonDtae = File.ReadAllText(path);

        var json = JsonUtility.FromJson<WaveWrapper>(JsonDtae);
        waves = json.waveArray;
        Debug.Log("Loaded JSON data from file.");
    }
}
