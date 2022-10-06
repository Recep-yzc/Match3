using Game;
using Game.Events;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CellManager : MonoBehaviour
{
    IGameEventSystem gameEventSystem = new GameEventSystem();

    [Header("Data")]
    public CellDataSo CellData;

    #region private
    private Coroutine scrollCoroutine;
    private int minDestroyCount = 2;

    private List<CellActor> cellActors = new();
    private Dictionary<Vector2, CellActor> cellActorsWithPosition = new();
    public FloatCellActors cellActorsVertical = new();
    public FloatCellActors cellActorsHorizontal = new();
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
        gameEventSystem.SaveEvent(GameEvents.ClickCellActor, status, ClickCellActor);
    }

    #endregion

    private void Start()
    {
        StartCoroutine(Wait());
        IEnumerator Wait()
        {
            yield return null;

            CalculateAxisList(cellActorsVertical, SnapAxis.X);
            //CalculateAxisList(cellActorsHorizontal, SnapAxis.Y);

            FindCellNeighbor();

            gameEventSystem.Publish(GameEvents.CreateCellItemActor, cellActors);
        }
    }

    private void ClickCellActor(object[] a)
    {
        CellActor clickedCellActor = (CellActor)a[0];

        if (clickedCellActor.IsEmpty) return;

        ItemType itemType = clickedCellActor.GetCellItemActor().ItemType;

        List<CellActor> clickedCellActors = new();
        FindClickedCellNeighbor(ref clickedCellActors, clickedCellActor, itemType);
        DestroySameItems(clickedCellActors);

        if (scrollCoroutine != null)
        {
            StopCoroutine(scrollCoroutine);
        }
        scrollCoroutine = StartCoroutine(ScrollItems());
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
            CellActor cellActorTemp = cellActors[i];

            foreach (var cellActor in cellActorsWithPosition)
            {
                List<CellActor> neighborCellActors = new();

                if (cellActorTemp == cellActor.Value)
                {
                    Vector2 v2 = cellActorTemp.GetPosition();

                    foreach (var neighborCellActor in cellActorsWithPosition)
                    {
                        for (int n = 0; n < Codes.neighborV2s.Length; n++)
                        {
                            if (neighborCellActor.Key == v2 + (Codes.neighborV2s[n] * CellData.CellScale))
                            {
                                neighborCellActors.Add(neighborCellActor.Value);
                            }
                        }

                        cellActorTemp.FetchNeighborCellActor(neighborCellActors);
                    }
                }
            }
        }
    }

    private List<CellActor> FindClickedCellNeighbor(ref List<CellActor> clickedCellActors, CellActor cellActor, ItemType firstItemType)
    {
        if (clickedCellActors.Contains(cellActor)) return clickedCellActors;

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

    private void DestroySameItems(List<CellActor> clickedCellActors)
    {
        if (clickedCellActors.Count < minDestroyCount) return;

        for (int i = 0; i < clickedCellActors.Count; i++)
        {
            CellActor cellActorTemp = clickedCellActors[i];

            if (!cellActorTemp.IsEmpty)
            {
                CellItemActor cellItemActorTemp = cellActorTemp.GetCellItemActor();
                cellItemActorTemp.CustomDestroy();
                cellActorTemp.RemoveCellItem();
            }
        }
    }

    private IEnumerator ScrollItems()
    {
        yield return new WaitForSeconds(0.1f);

        foreach (var item in cellActorsVertical)
        {
            List<CellActor> cellActors = item.Value;

            for (int c = 0; c < cellActors.Count; c++)
            {
                for (int p = 1; p < cellActors.Count; p++)
                {
                    CellActor cellActorTemp1 = cellActors[p - 1];
                    CellActor cellActorTemp2 = cellActors[p];

                    if (cellActorTemp1.IsEmpty && !cellActorTemp2.IsEmpty)
                    {
                        CellItemActor cellItemActorTemp2 = cellActorTemp2.GetCellItemActor();
                        cellActorTemp1.SetCellItemActor(cellItemActorTemp2);

                        cellItemActorTemp2.Move(cellActorTemp1.GetPosition());
                        cellActorTemp2.RemoveCellItem();
                    }
                }
            }
        }
    }
}