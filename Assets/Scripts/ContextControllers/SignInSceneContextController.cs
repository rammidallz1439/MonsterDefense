using BGS.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vault;
public class SignInSceneContextController : Registerer
{
    [SerializeField] private LoginHandler _loginHandler;
    public override void Enable()
    {
    }

    public override void OnAwake()
    {
        AddController(new LoginController(_loginHandler));
    }

    public override void OnStart()
    {
    }
}
