using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResourceCardIconUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    Image image = null;
    BattleResourceCardUI cardInfo = null;

    private bool isDragging = false;
    private bool canActivateTooltip = true;

    private void Awake()
    {
        image = GetComponent<Image>();
        cardInfo = transform.parent.parent.parent.GetComponent<BattleResourceCardUI>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
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
        canActivateTooltip = true;
        isDragging = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isDragging) return;
        if (!canActivateTooltip) return;
        canActivateTooltip = false;

        TooltipUI.Instance.Show(eventData.position, true, cardInfo.Resource.TooltipInfo);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipUI.Instance.Show(default, false);
        canActivateTooltip = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        TooltipUI.Instance.Show(default, false);
    }
}
