using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowInfoBox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject infoBox;
    public void OnPointerEnter(PointerEventData eventData)
    {
        infoBox.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        infoBox.SetActive(false);
    }
}
