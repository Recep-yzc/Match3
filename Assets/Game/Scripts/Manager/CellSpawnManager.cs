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

    private Vector2 CalculateCellPosition(int totalRowCount, int currentRow, int totalColumnCount, int currentColumn, float cellScale)
    {
        float resultRow = CalculateCellPositionRow(totalRowCount, currentRow, cellScale);
        float resultColumn = CalculateCellPositionColumn(totalColumnCount, currentColumn, cellScale);

        return new(resultColumn, resultRow);
    }

    private static float CalculateCellPositionColumn(int totalColumnCount, int currentColumn, float cellScale)
    {
        float centerOffsetColumn = (totalColumnCount * 0.5f) * cellScale;
        float halfScaleColumn = cellScale * 0.5f;
        float positionColumn = (currentColumn + 1) * cellScale;
        float resultColumn = positionColumn - centerOffsetColumn - halfScaleColumn;
        return resultColumn;
    }

    private static float CalculateCellPositionRow(int totalRowCount, int currentRow, float cellScale)
    {
        float centerOffsetRow = (totalRowCount * 0.5f) * cellScale;
        float halfScaleRow = cellScale * 0.5f;
        float positionRow = (currentRow + 1) * cellScale;
        float resultRow = positionRow - centerOffsetRow - halfScaleRow;
        return resultRow;
    }
}
