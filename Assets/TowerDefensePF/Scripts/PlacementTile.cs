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

            int mX = Mathf.RoundToInt(mousePos.x);
            int mY = Mathf.RoundToInt(mousePos.y);

            int x = Mathf.RoundToInt(transform.position.x);
            int y = Mathf.RoundToInt(transform.position.y);

            if (mX == x && mY == y)
            {
                //Remove tile
                DesignManager.Instance.RemovePlacedTile(transform.position, Cost);
                Destroy(gameObject);
            }

        }
    }
}
public enum TileCode
{
    Free = 0,
    Unit_Soldier_A = 1,
    Unit_Soldier_D = 2,
    Unit_Hero = 3,
    Turret_T1 = 4,
    Turret_T2 = 5,
    Wall_Rock = 6,
    Wall_Concrete = 7,
    Core = 8
}