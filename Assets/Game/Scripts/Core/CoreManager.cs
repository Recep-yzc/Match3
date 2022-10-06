using UnityEngine;

namespace Game
{

    [ScriptOrder(-9999)]
    public class CoreManager : MonoBehaviour
    {
        #region private
        private EventManager eventManager;
        #endregion

        private void OnEnable()
        {
            eventManager = new EventManager();
            eventManager.Init();
        }

        private void OnDisable()
        {

        }
    }
}