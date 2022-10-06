using UnityEngine;

public class CellSpawnManager : MonoBehaviour
{
    [Header("Data")]
    public CellDataSo CellData;

    [Header("References")]
    [SerializeField] private Transform parent;

    private void Awake()
    {
        CreateCellActor();
    }

    private void CreateCellActor()
    {
        int totalColumnCount = CellData.ColumnCount;
        int totalRowCount = CellData.RowCount;

        float cellScale = CellData.CellScale;
        CellActor cellActorPrefab = CellData.CellActorPrefab;

        for (int currentColumn = 0; currentColumn < totalColumnCount; currentColumn++)
        {
            for (int currentRow = 0; currentRow < totalRowCount; currentRow++)
            {
                CellActor cellActorTemp = Instantiate(cellActorPrefab, parent);

                Vector2 cellPosition = CalculateCellPosition(totalRowCount, currentRow, totalColumnCount, currentColumn, cellScale);
                cellActorTemp.SetPosition(cellPosition);
                cellActorTemp.SetScale(cellScale * Vector3.one);
            }
        }
    }

    private Vector2 CalculateCellPosition(int totalRowCount, int currentRow, int totalColumnCount, int currentColumn, float scale)
    {
        float centerOffsetRow = (totalRowCount * 0.5f) * scale;
        float halfScaleRow = scale * 0.5f;
        float positionRow = (currentRow + 1) * scale;
        float resultRow = positionRow - centerOffsetRow - halfScaleRow;

        float centerOffsetColumn = (totalColumnCount * 0.5f) * scale;
        float halfScaleColumn = scale * 0.5f;
        float positionColumn = (currentColumn + 1) * scale;
        float resultColumn = positionColumn - centerOffsetColumn - halfScaleColumn;

        return new(resultColumn, resultRow);
    }
}
