using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vault
{

    public class RunTimeInitialization
    {
        [RuntimeInitializeOnLoadMethod]
        private static void OnProjectInitialize()
        {
            var controller = Object.FindObjectOfType<ContextController>();

            if (controller == null)
            {
                var obj = Resources.Load<ContextController>("ProjectContextControler");
                if (obj)
                {
                    controller = Object.Instantiate(obj);
                    Object.DontDestroyOnLoad(controller.gameObject);

                }
                else
                {
                    Debug.LogError("[Context Controller] : ProjectContextController Not Found Please Check Resources");
                }
            }
            else
            {
                Object.DontDestroyOnLoad(controller.gameObject);
            }
        }
    }
}

