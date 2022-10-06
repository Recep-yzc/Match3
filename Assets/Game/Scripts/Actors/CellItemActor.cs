using DG.Tweening;
using UnityEngine;

public class CellItemActor : MonoBehaviour
{
    public ItemType ItemType { get; set; }

    [Header("References")]
    [SerializeField] private SpriteRenderer imgBg;

    #region private
    private ItemManager itemManager;
    #endregion

    private void Awake()
    {
        itemManager = ItemManager.Instance;
    }

    public void SetType(ItemType itemType) => ItemType = itemType;

    public void SetSprite(Sprite sprite) => imgBg.sprite = sprite;
    public void SetPosition(Vector2 position) => transform.position = position;
    public void SetScale(Vector3 scale) => transform.localScale = scale;

    public void Move(Vector2 position)
    {
        transform.DOMove(position, itemManager.CellItemData.MoveSpeed).SetSpeedBased().SetEase(itemManager.CellItemData.MoveEase);
    }

    public void CustomDestroy()
    {
        Destroy(gameObject);
    }
}
