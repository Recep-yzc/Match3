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
    [SerializeField] private CellItemActor cellItemActorPrefab;
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
    }
    #endregion

    private void CreateCellItemActor(object[] a)
    {
        List<CellActor> cellActors = (List<CellActor>)a[0];

        int rndType;
        int maxRndValue = CellItemData.CellItemSprites.Count;
        int minRndValue = 0;

        for (int i = 0; i < cellActors.Count; i++)
        {
            CellActor cellActorTemp = cellActors[i];
            rndType = Random.Range(minRndValue, maxRndValue);

            ItemType itemTypeTemp = (ItemType)rndType;

            CellItemActor cellItemActorTemp = Instantiate(cellItemActorPrefab, cellItemParent);

            cellItemActorTemp.SetCellItemData(CellItemData);
            cellItemActorTemp.SetType(itemTypeTemp);
            cellItemActorTemp.SetSprite(CellItemData.CellItemSprites[itemTypeTemp]);
            cellItemActorTemp.SetPosition(cellActorTemp.GetPosition());
            cellItemActorTemp.SetScale(cellActorTemp.GetScale());

            cellActorTemp.SetCellItemActor(cellItemActorTemp);
        }
    }
}
