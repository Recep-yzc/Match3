using Game;
using Game.Events;
using System.Collections.Generic;
using UnityEngine;

public class CellItemSpawnManager : MonoBehaviour
{
    IGameEventSystem gameEventSystem = new GameEventSystem();

    [Header("Data")]
    public CellItemDataSo CellItemData;

    [Header("References")]
    [SerializeField] private Transform cellItemParent;

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
        gameEventSystem.SaveEvent(GameEvents.CreateCellItemActor, status, CreateCellItemActor);
        gameEventSystem.SaveEvent(GameEvents.CreateCustomCellItemActor, status, CreateCustomCellItemActor);

    }
    #endregion

    private void CreateCellItemActor(object[] a)
    {
        List<CellActor> cellActors = (List<CellActor>)a[0];

        for (int i = 0; i < cellActors.Count; i++)
        {
            CellActor cellActorTemp = cellActors[i];

            CellItemActor cellItemActorTemp = Instantiate(CellItemData.CellItemActorPrefab, cellItemParent);
            ItemType itemTypeTemp = GetRandomItemType();

            cellItemActorTemp.SetCellItemData(CellItemData);
            cellItemActorTemp.SetType(itemTypeTemp);
            cellItemActorTemp.SetSprite(CellItemData.CellItemSprites[itemTypeTemp]);
            cellItemActorTemp.SetPosition(cellActorTemp.GetPosition());
            cellItemActorTemp.SetScale(cellActorTemp.GetScale());

            cellActorTemp.SetCellItemActor(cellItemActorTemp);
        }
    }

    private void CreateCustomCellItemActor(object[] a)
    {
        float key = (float)a[0];
        List<CellActor> emptyCellActors = (List<CellActor>)a[1];

        for (int i = 0; i < emptyCellActors.Count; i++)
        {
            CellActor cellActorTemp = emptyCellActors[i];

            CellItemActor cellItemActorTemp = Instantiate(CellItemData.CellItemActorPrefab, cellItemParent);
            ItemType itemTypeTemp = GetRandomItemType();
            Vector2 originalPosition = cellActorTemp.GetPosition();
            Vector2 spawnPosition = new Vector2(originalPosition.x, originalPosition.y + 2);

            cellItemActorTemp.SetCellItemData(CellItemData);
            cellItemActorTemp.SetType(itemTypeTemp);
            cellItemActorTemp.SetSprite(CellItemData.CellItemSprites[itemTypeTemp]);
            cellItemActorTemp.SetPosition(spawnPosition);
            cellItemActorTemp.SetScale(cellActorTemp.GetScale());

            cellItemActorTemp.Move(originalPosition);

            cellActorTemp.SetCellItemActor(cellItemActorTemp);
        }
    }

    private ItemType GetRandomItemType()
    {
        int rndType;
        int maxRndValue = CellItemData.CellItemSprites.Count;
        int minRndValue = 0;

        rndType = Random.Range(minRndValue, maxRndValue);

        return (ItemType)rndType;
    }
}
