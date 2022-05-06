using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResourceCardIconUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Image image = null;
    BattleResourceCardUI cardInfo = null;

    private void Awake()
    {
        image = GetComponent<Image>();
        cardInfo = transform.parent.parent.parent.GetComponent<BattleResourceCardUI>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, .5f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = transform.parent.position;
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
        DesignManager.Instance.TryPlacement(cardInfo.Resource.TileCode, cardInfo.Resource.Cost, cardInfo.Resource.Icon, Camera.main.ScreenToWorldPoint(eventData.position));
    }
}
