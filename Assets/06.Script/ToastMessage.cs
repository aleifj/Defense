using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum ToastType//메시지 종류
{
    Money,//골드 모라랄 때
    Build //건설 불가능 할 때
}
public class ToastMessage : MonoBehaviour
{
    private TextMeshProUGUI toastMsg;
    private TMPAlpha tmpAlpha;
    void Start()
    {
        toastMsg = GetComponent<TextMeshProUGUI>();
        tmpAlpha = GetComponent<TMPAlpha>();
    }

    public void ShowToast(ToastType Type)
    {
        switch(Type)
        {
            case ToastType.Money:
                toastMsg.text = "Not Enough Money";
                break;

            case ToastType.Build:
                toastMsg.text = "Invalid Build tower";
                break;
        }
        tmpAlpha.FadeOut();
    }
}
