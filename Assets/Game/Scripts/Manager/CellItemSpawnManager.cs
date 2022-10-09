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
            CellActor cellActor = cellActors[i];

            ItemType itemType = GetRandomItemType();
            Sprite sprite = CellItemData.CellItemSprites[itemType];
            Vector2 position = cellActor.GetPosition();
            Vector3 scale = cellActor.GetScale();

            CellItemActor cellItemActor = CreateCellItemActor(CellItemData, itemType, sprite, position, scale);

            cellActor.SetCellItemActor(cellItemActor);
        }
    }

    private void CreateCustomCellItemActor(object[] a)
    {
        List<CellActor> emptyCellActors = (List<CellActor>)a[0];

        for (int i = 0; i < emptyCellActors.Count; i++)
        {
            CellActor cellActor = emptyCellActors[i];

            ItemType itemType = GetRandomItemType();
            Sprite sprite = CellItemData.CellItemSprites[itemType];
            Vector2 position = cellActor.GetPosition();
            Vector2 spawnPosition = new Vector2(position.x, position.y + 2);
            Vector3 scale = cellActor.GetScale();

            CellItemActor cellItemActor = CreateCellItemActor(CellItemData, itemType, sprite, spawnPosition, scale);
            cellItemActor.Move(position);

            cellActor.SetCellItemActor(cellItemActor);
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

    private CellItemActor CreateCellItemActor(CellItemDataSo cellItemData,ItemType itemType,Sprite sprite, Vector2 position, Vector3 scale)
    {
        CellItemActor cellItemActor = Instantiate(CellItemData.CellItemActorPrefab, cellItemParent);

        cellItemActor.SetCellItemData(cellItemData);
        cellItemActor.SetType(itemType);
        cellItemActor.SetSprite(sprite);
        cellItemActor.SetPosition(position);
        cellItemActor.SetScale(scale);

        return cellItemActor;
    }
}
