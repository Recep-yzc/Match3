using Game;
using Game.Events;
using UnityEngine;
using UnityEngine.EventSystems;

public class CellClickActor : MonoBehaviour, IPointerClickHandler
{
    IGameEventSystem gameEventSystem = new GameEventSystem();

    [Header("References")]
    [SerializeField] private CellActor cellActor;

    public void OnPointerClick(PointerEventData eventData)
    {
        gameEventSystem.Publish(GameEvents.ClickCellActor, cellActor);
    }
}
