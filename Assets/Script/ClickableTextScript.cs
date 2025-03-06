using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ClickableTextScript : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GameObject currscreen;
    [SerializeField] GameObject nextscreen;

    public void OnPointerClick(PointerEventData eventData)
    {
        currscreen.SetActive(false);
        nextscreen.SetActive(true);
    }
}
