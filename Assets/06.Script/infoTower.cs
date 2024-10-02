using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;

public class infoTower : MonoBehaviour
{
    [SerializeField] private Image imageTower;
    [SerializeField] private TextMeshProUGUI textLevel;
    [SerializeField] private TextMeshProUGUI textDamage;
    [SerializeField] private TextMeshProUGUI textRate;
    [SerializeField] private TextMeshProUGUI textRange;
    [SerializeField] private TowerAttackRange towerAttackRange;
    [SerializeField] private TextMeshProUGUI textBtnUpgrade; 
    [SerializeField] private TextMeshProUGUI textBtnSell;
    [SerializeField] private Button buttonUpgrade;
    [SerializeField] private Button buttonSell;

    private TowerWeapon currentTower;

    private void Start()
    {
        OffPanel();
    }

    private void Update()
    {
        // esc키 눌리면 패널 끄기
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OffPanel();
        }
    }

    /// <summary>
    /// 타워 무기 정보를 넘겨 받아 정보 패널과 공격 범위를 켜고 보여준다
    /// </summary>
    /// <param name="towerWeapon">타워 무기 정보가 있는 transform</param>
    public void OnPanel(Transform towerWeapon)
    {
        // 파라미터로 받은 transform안에 있는 TowerWeapon 정보를 현재 타워로 연결
        currentTower = towerWeapon.GetComponent<TowerWeapon>();
        // 패널을 켠다
        gameObject.SetActive(true);
        // 타워 정보를 갱신
        UpdateTowerData();
        // 공격 범위 이미지도 켠다
        towerAttackRange.OnAttackRange(currentTower.transform.position, currentTower.Range);
    }

    /// <summary>
    /// 공격범위 이미지를 포함하여, 정보 패널을 끈다
    /// </summary>
    public void OffPanel()
    {
        // 패널 전체를 끈다
        gameObject.SetActive(false);
        // 공격 범위 이미지도 끈다
        towerAttackRange.OffAttackRange();
    }

    private void UpdateTowerData()
    {
        imageTower.sprite = currentTower.TowerSprite;//타워 이미지 변화
        textLevel.text = $"Level : {currentTower.Level}"; // 레벨
        textDamage.text = $"Damage : {currentTower.Damage}"; // 데미지
        textRate.text = $"Rate : {currentTower.Rate}"; // 간격
        textRange.text = $"Range : {currentTower.Range}"; // 범위

        textBtnUpgrade.text = $"Upgrade\n{currentTower.CostUpgrade}";//업그레이드 비용
        textBtnSell.text = $"Sell\n{currentTower.CostSell}";//판매 비용

        //todo 업그레이드가 더 이상 안되는 상황이면 버튼이 안 눌리게 처리
        buttonUpgrade.interactable = (currentTower.Level < currentTower.MaxLevel) ? true : false;//삼항연산자.
    }
    public void OnClickUpGrade()
    {
        if(currentTower.UpGrade() == true)
        {
            UpdateTowerData();
            towerAttackRange.OnAttackRange(currentTower.transform.position, currentTower.Range);
        }
        else
        {
            //todo 안된다
        }
    }

    /// <summary>
    /// SEll버튼이 눌릴 때 동작
    /// </summary>
    public void OnClickSell()
    {
        currentTower.Sell();
        OffPanel();
    }
}
