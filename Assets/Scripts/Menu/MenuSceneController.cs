using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vault;

namespace BGS.Game
{
    public class MenuSceneController : MenuSceneManager, IController
    {
        public MenuSceneController(MenuSceneHandler handler)
        {
            Handler = handler;
        }
        public void OnInitialized()
        {
            EventManager.Instance.TriggerEvent(new MenuSceneInitEvent());
        }

        public void OnRegisterListeners()
        {
            EventManager.Instance.AddListener<MenuSceneInitEvent>(MenuSceneInitEventHandler);
        }

        public void OnRelease()
        {
        }

        public void OnRemoveListeners()
        {
            EventManager.Instance.RemoveListener<MenuSceneInitEvent>(MenuSceneInitEventHandler);

        }

        public void OnStarted()
        {
        }

        public void OnVisible()
        {
        }
    }
}

