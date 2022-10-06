using UnityEngine;

[CreateAssetMenu(fileName = "CellData", menuName = "Data/CellDataSo")]
public class CellDataSo : ScriptableObject
{
    [Range(0, 10)]
    public int RowCount;

    [Range(0, 10)]
    public int ColumnCount;

    public CellActor CellActorPrefab;
    public float CellScale;
}
