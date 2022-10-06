using DG.Tweening;
using UnityEngine;

public class CellItemActor : MonoBehaviour
{
    public ItemType ItemType { get; set; }

    [Header("References")]
    [SerializeField] private SpriteRenderer imgBg;

    #region private
    private CellItemDataSo cellItemData;
    #endregion

    public void SetCellItemData(CellItemDataSo cellItemData) => this.cellItemData = cellItemData;
    public void SetType(ItemType itemType) => ItemType = itemType;

    public void SetSprite(Sprite sprite) => imgBg.sprite = sprite;
    public void SetPosition(Vector2 position) => transform.position = position;
    public void SetScale(Vector3 scale) => transform.localScale = scale;

    public void Move(Vector2 position)
    {
        transform.DOMove(position, cellItemData.MoveSpeed)
            .SetSpeedBased()
            .SetEase(cellItemData.MoveEase)
            .SetLink(gameObject);
    }

    public void CustomDestroy()
    {
        Destroy(gameObject);
    }
}
