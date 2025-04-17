/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vault;

namespace BGS.Game
{
    public class HireTimerController : HireTimerManager, IController
    {
        public HireTimerController(HireTimerHandler handler)
        {
            Handler = handler;
        }
        public void OnInitialized()
        {

        }

        public void OnRegisterListeners()
        {
            EventManager.Instance.AddListener<InitHireTimersEvent>(InitHireTimersEventHandler);
            EventManager.Instance.AddListener<AddSpawnedBasesEvent>(AddSpawnedBasesEventHandler);

        }

        public void OnRelease()
        {
        }

        public void OnRemoveListeners()
        {
            EventManager.Instance.RemoveListener<InitHireTimersEvent>(InitHireTimersEventHandler);
            EventManager.Instance.RemoveListener<AddSpawnedBasesEvent>(AddSpawnedBasesEventHandler);

        }

        public void OnStarted()
        {

        }

        public void OnVisible()
        {

        }
    }
}
*/