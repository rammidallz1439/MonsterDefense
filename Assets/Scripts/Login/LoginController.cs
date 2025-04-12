using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vault;

namespace BGS.Game
{
    public class LoginController : LoginManager, IController
    {
        public LoginController(LoginHandler handler)
        {
            Handler = handler;
        }
        public void OnInitialized()
        {
            EventManager.Instance.TriggerEvent(new LoginSceneInitEvent());
        }

        public void OnRegisterListeners()
        {
            EventManager.Instance.AddListener<LoginSceneInitEvent>(LoginSceneInitEventHandler);
            EventManager.Instance.AddListener<SwitchLoginStateEvent>(SwitchLoginStateEventHandler);
        }

        public void OnRelease()
        {
        }

        public void OnRemoveListeners()
        {
            EventManager.Instance.RemoveListener<LoginSceneInitEvent>(LoginSceneInitEventHandler);
            EventManager.Instance.RemoveListener<SwitchLoginStateEvent>(SwitchLoginStateEventHandler);

        }

        public void OnStarted()
        {
        }

        public void OnVisible()
        {
        }
    }
}
