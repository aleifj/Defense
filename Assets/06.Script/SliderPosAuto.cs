using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderPosAuto : MonoBehaviour
{
    [SerializeField] private Vector3 distance = Vector3.down * 40.0f;//HPbar위치지정
    private Transform targetTransform;//붙여야 할 대상
    private RectTransform rectTransform;//슬라이더의 rectTF

    public void Setup(Transform target)
    {
        //붙여야 할 대상 지정
        targetTransform = target;
        //rt컴퍼넌트 연결
        rectTransform = GetComponent<RectTransform>();
    }
    void LateUpdate()
    {
        //대상이 사라졌거나 없으면 리턴
        if(targetTransform == null)
        {
            Destroy(gameObject);
            return;
        }
        //실제 표시할 좌효 계산
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetTransform.position);

        //지정할 위치만큼 떨어져서 붙이기
        rectTransform.position = screenPosition + distance;
    }
}
