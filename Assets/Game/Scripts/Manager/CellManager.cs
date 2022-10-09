using Game;
using Game.Events;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CellManager : MonoBehaviour
{
    IGameEventSystem gameEventSystem = new GameEventSystem();

    [Header("Data")]
    public CellDataSo CellData;

    [Header("Properties")]
    public int MinDestroyCount = 2;

    #region private
    private List<CellActor> cellActors = new();
    private Dictionary<Vector2, CellActor> cellActorsWithPosition = new();
    private FloatCellActors cellActorsVertical = new();
    #endregion

    #region Event
    private void OnEnable()
    {
        Listen(true);
    }

    private void OnDisable()
    {
        Listen(false);
    }

    private void Listen(bool status)
    {
        gameEventSystem.SaveEvent(GameEvents.SaveCellActor, status, SaveCellActor);
        gameEventSystem.SaveEvent(GameEvents.ClickCellActor, status, ClickedCellActor);
    }

    #endregion

    private void Start()
    {
        StartCoroutine(Wait());
        IEnumerator Wait()
        {
            yield return null;

            CalculateAxisList(cellActorsVertical, SnapAxis.X);

            FindCellNeighbor();

            gameEventSystem.Publish(GameEvents.CreateCellItemActor, cellActors);
        }
    }

    private void ClickedCellActor(object[] a)
    {
        CellActor clickedCellActor = (CellActor)a[0];

        if (clickedCellActor.IsEmpty) return;

        List<CellActor> clickedCellActors = new();
        ItemType itemType = clickedCellActor.GetCellItemActor().ItemType;
        
        FindClickedCellNeighbor(ref clickedCellActors, clickedCellActor, itemType);
        DestroyCellItemActors(clickedCellActors);

        ScrollCellItemActors();
        CreateCellItemActors();
    }

    private void SaveCellActor(object[] a)
    {
        CellActor cellActorTemp = (CellActor)a[0];

        cellActors.Add(cellActorTemp);
        cellActorsWithPosition.Add(cellActorTemp.GetPosition(), cellActorTemp);
    }

    private void CalculateAxisList(FloatCellActors axisCellActors, SnapAxis snapAxis)
    {
        for (int i = 0; i < cellActors.Count; i++)
        {
            CellActor cellActor = cellActors[i];
            Vector2 position = cellActor.GetPosition();
            float key = 0;

            if (snapAxis == SnapAxis.X)
            {
                key = position.x;
            }
            else if (snapAxis == SnapAxis.Y)
            {
                key = position.y;
            }

            List<CellActor> cellActorsTemp;
            if (axisCellActors.ContainsKey(key))
            {
                axisCellActors.TryGetValue(key, out cellActorsTemp);
                cellActorsTemp.Add(cellActor);
            }
            else
            {
                cellActorsTemp = new();
                cellActorsTemp.Add(cellActor);
                axisCellActors.Add(key, cellActorsTemp);
            }
        }
    }

    private void FindCellNeighbor()
    {
        for (int i = 0; i < cellActors.Count; i++)
        {
            CellActor cellActor = cellActors[i];

            foreach (var dictionary in cellActorsWithPosition)
            {
                List<CellActor> neighborCellActors = new();

                if (cellActor == dictionary.Value)
                {
                    Vector2 position = cellActor.GetPosition();

                    foreach (var neighborCellActor in cellActorsWithPosition)
                    {
                        for (int n = 0; n < Codes.neighborV2s.Length; n++)
                        {
                            Vector2 key = position + (Codes.neighborV2s[n] * CellData.CellScale);

                            if (neighborCellActor.Key == key)
                            {
                                neighborCellActors.Add(neighborCellActor.Value);
                                break;
                            }
                        }
                    }
                    cellActor.FetchNeighborCellActor(neighborCellActors);
                }
            }
        }
    }

    private List<CellActor> FindClickedCellNeighbor(ref List<CellActor> clickedCellActors, CellActor cellActor, ItemType firstItemType)
    {
        if (clickedCellActors.Contains(cellActor))
            return clickedCellActors;

        clickedCellActors.Add(cellActor);

        for (int i = 0; i < cellActor.neighborCellActors.Count; i++)
        {
            CellActor neighborCellActor = cellActor.neighborCellActors[i];

            if (!neighborCellActor.IsEmpty)
            {
                ItemType itemType = neighborCellActor.GetCellItemActor().ItemType;

                if (itemType == firstItemType)
                {
                    FindClickedCellNeighbor(ref clickedCellActors, neighborCellActor, itemType);
                }
            }
        }

        return clickedCellActors;
    }

    private void DestroyCellItemActors(List<CellActor> clickedCellActors)
    {
        if (clickedCellActors.Count < MinDestroyCount) return;

        for (int i = 0; i < clickedCellActors.Count; i++)
        {
            CellActor cellActor = clickedCellActors[i];

            if (!cellActor.IsEmpty)
            {
                CellItemActor cellItemActor = cellActor.GetCellItemActor();
                cellItemActor.CustomDestroy();
                cellActor.RemoveCellItem();
            }
        }
    }

    private void ScrollCellItemActors()
    {
        foreach (var floatCellActors in cellActorsVertical)
        {
            List<CellActor> cellActors = floatCellActors.Value;

            bool isExistsEmpty = IsExistsEmpty(cellActors);
            if (isExistsEmpty)
            {
                for (int c1 = 0; c1 < cellActors.Count; c1++)
                {
                    for (int c2 = 1; c2 < cellActors.Count; c2++)
                    {
                        CellActor currentCellActor = cellActors[c2 - 1];
                        CellActor nextCellActor = cellActors[c2];

                        if (currentCellActor.IsEmpty && !nextCellActor.IsEmpty)
                        {
                            CellItemActor nextCellItemActor = nextCellActor.GetCellItemActor();
                            currentCellActor.SetCellItemActor(nextCellItemActor);

                            nextCellItemActor.Move(currentCellActor.GetPosition());
                            nextCellActor.RemoveCellItem();
                        }
                    }
                }
            }
        }
    }

    private void CreateCellItemActors()
    {
        foreach (var floatCellActors in cellActorsVertical)
        {
            List<CellActor> cellActors = floatCellActors.Value;

            int emptyCount = cellActors.Count(x => x.IsEmpty);
            if (emptyCount > 0)
            {
                List<CellActor> emptyCellActors = cellActors.TakeLast(emptyCount).ToList();
                gameEventSystem.Publish(GameEvents.CreateCustomCellItemActor, emptyCellActors);
            }
        }
    }

    private bool IsExistsEmpty(List<CellActor> cellActors)
    {
        bool value = false;
        for (int i = 0; i < cellActors.Count; i++)
        {
            CellActor cellActor = cellActors[i];

            if (cellActor.IsEmpty)
            {
                value = true;
                break;
            }
        }

        return value;
    }
}
