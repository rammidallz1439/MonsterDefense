using BGS.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vault;

public class MenuSceneContextController : Registerer
{
    [SerializeField] private MenuSceneHandler _menuSceneHandler;

    public override void Enable()
    {
    }

    public override void OnAwake()
    {
        AddController(new MenuSceneController(_menuSceneHandler));
    }

    public override void OnStart()
    {
    }
}
