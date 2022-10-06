using UnityEngine;

[CreateAssetMenu(fileName = "CellData", menuName = "Data/CellDataSo")]
public class CellDataSo : ScriptableObject
{
    [Range(0, 10)]
    public int RowCount;

    [Range(0, 10)]
    public int ColumnCount;

    [Space(5)]
    public CellActor CellActorPrefab;
    public float CellScale;
}
