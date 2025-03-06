using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class HideErrorScript : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] TMP_Text error;   

    public void OnPointerClick(PointerEventData eventData)
    {
        error.text="";
    }
}
