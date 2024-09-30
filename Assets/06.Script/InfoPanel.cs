using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Microsoft.Win32.SafeHandles;
public class InfoPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textPlayerHP;//유저 체럭
    [SerializeField] private TextMeshProUGUI textGold;//골드.
    [SerializeField] private TextMeshProUGUI textWave;
    [SerializeField] private TextMeshProUGUI textEnemyCount;
    [SerializeField] private WaveSystem waveSystem;
    void Update()
    {
        //체력표시
        textPlayerHP.text = $"{PlayerManager.instance.CurrentHP}/{PlayerManager.instance.MaxHP}";
        //골드표시
        textGold.text = PlayerManager.instance.CurrentGold.ToString();
        //
        textWave.text = waveSystem.GetWaveInfoString();

        textEnemyCount.text = $"{EnemyManager.instance.CurrentEnemyCount}/{EnemyManager.instance.MaxEnemyCount}";
    }
}
