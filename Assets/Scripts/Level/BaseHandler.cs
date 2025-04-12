using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Vault;

public class BaseHandler : MonoBehaviour,IPointerDownHandler
{
    public float InitialYPos;
    public string hexColorCode = "#3A3A3A";
    public Transform SpawnPoint;
    public bool Occupied;
    public int Id;
    private void Start()
    {
        InitialYPos = transform.position.y;
    }

    public Color GetColor()
    {
        Color newColor;

        if (ColorUtility.TryParseHtmlString(hexColorCode, out newColor))
        {
            return newColor;
        }
        else
        {
            Debug.LogError("Invalid hex Color Code");
            return Color.black;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
      
        if (!Occupied)
            EventManager.Instance.TriggerEvent(new BaseSelectedEvent(this));
    }
}
