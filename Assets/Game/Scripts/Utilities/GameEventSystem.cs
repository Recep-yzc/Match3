namespace Game
{
    public class GameEventSystem : IGameEventSystem
    {
        public void Publish(string managerEvent, params object[] args) => EventManager.Instance.Publish(managerEvent, args);
        public void Subscribe(string managerEvent, Subscription subscription) => EventManager.Instance.Subscribe(managerEvent, subscription);
        public void Unsubscribe(string managerEvent, Subscription subscription) => EventManager.Instance.Unsubscribe(managerEvent, subscription);

        public void SaveEvent(string managerEvent, bool status, Subscription subscription)
        {
            if (status)
            {
                Subscribe(managerEvent, subscription);
            }
            else
            {
                Unsubscribe(managerEvent, subscription);
            }
        }
    }
}