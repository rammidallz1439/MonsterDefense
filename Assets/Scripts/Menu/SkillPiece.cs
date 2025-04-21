using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vault;

public class SkillPiece : MonoBehaviour
{
    public Image Icon;
    public TMP_Text Description;
    public TMP_Text CostText;
    public Button BuyButton;
    public int Price;
    public int Index;
    public GameObject TickMark;


    public void Start()
    {
        BuyButton.onClick.AddListener(OnBuyButton);
    }

    private void OnBuyButton()
    {
        EventManager.Instance.TriggerEvent(new OnSkilluyButtonEvent(this)); 
    }
}
