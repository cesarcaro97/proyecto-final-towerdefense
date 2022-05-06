using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlacementTile : MonoBehaviour
{
    SpriteRenderer spriteRenderer = null;

    public Sprite Icon { get => spriteRenderer.sprite; set => spriteRenderer.sprite = value; }
    public int Cost { get; set; }


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingLayerName = "TileCode";
    }

    private void Update()
    {
        //Right click
        if(Input.GetMouseButton(1))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            int mX = Mathf.FloorToInt(mousePos.x);
            int mY = Mathf.CeilToInt(mousePos.y);

            int x = Mathf.FloorToInt(transform.position.x);
            int y = Mathf.CeilToInt(transform.position.y);

            if (mX == x && mY == y)
            {
                //Remove tile
                DesignManager.Instance.RemovePlacedTile(transform.position, Cost);
                Destroy(gameObject);
            }

        }
    }
}
