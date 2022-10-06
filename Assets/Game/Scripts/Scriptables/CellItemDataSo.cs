using UnityEngine;

[CreateAssetMenu(fileName = "CellItemDataSo", menuName = "Data/CellItemDataSo")]
public class CellItemDataSo : ScriptableObject
{
    public float MoveSpeed = 5f;
    public AnimationCurve MoveEase;

    [Space(5)]
    public CellItemActor CellItemActorPrefab;

    [Space(5)]
    public CellItemSprites CellItemSprites;
}
