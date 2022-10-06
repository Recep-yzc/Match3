using Game;
using Game.Events;
using System.Collections.Generic;
using UnityEngine;

public class CellActor : MonoBehaviour
{
    IGameEventSystem gameEventSystem = new GameEventSystem();

    public bool IsEmpty => cellItemActor == null ? true : false;
    public List<CellActor> neighborCellActors { get; set; }

    #region private 
    private CellItemActor cellItemActor;

    #endregion

    private void Start()
    {
        gameEventSystem.Publish(GameEvents.SaveCellActor, this);
    }

    public void FetchNeighborCellActor(List<CellActor> cellActors)
    {
        neighborCellActors = new List<CellActor>(cellActors);
    }

    public void RemoveCellItem()
    {
        cellItemActor = null;
    }

    public void SetCellItemActor(CellItemActor cellItemActor) => this.cellItemActor = cellItemActor;
    public CellItemActor GetCellItemActor() => cellItemActor;

    public void SetPosition(Vector2 position) => transform.position = position;
    public Vector2 GetPosition() => transform.position;

    public void SetScale(Vector3 scale) => transform.localScale = scale;
    public Vector3 GetScale() => transform.localScale;
}
