using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InfoPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textPlayerHP;//유저 체럭
    [SerializeField] private TextMeshProUGUI textGold;//골드
    void Update()
    {
        //체력표시
        textPlayerHP.text = $"{PlayerManager.instance.CurrentHP}/{PlayerManager.instance.MaxHP}";
        //골드표시
        textGold.text = PlayerManager.instance.CurrentGold.ToString();
    }
}
