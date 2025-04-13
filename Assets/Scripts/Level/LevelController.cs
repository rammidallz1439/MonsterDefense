using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vault;

public class LevelController : LevelManager, IController, ITick, IPausable
{
    public LevelController(LevelHandler handler)
    {
        Handler = handler;
    }

    public void OnApplicationPaused()
    {
        // SavedHiredDataOnDestroyEventHandler();
    }

    public void OnInitialized()
    {
        EventManager.Instance.TriggerEvent(new IntialLevelSetUpEvent());
    }

    public void OnRegisterListeners()
    {
        EventManager.Instance.AddListener<IntialLevelSetUpEvent>(InitialLevelSetupEventHandler);
        EventManager.Instance.AddListener<BaseSelectedEvent>(BaseSelectedEventHandler);
        EventManager.Instance.AddListener<SpawnTurretEvent>(SpawnTurretEventHandler);
        EventManager.Instance.AddListener<UpdateTimerEvent>(UpdateTimerEventHandler);
        EventManager.Instance.AddListener<CoinDobberAnimation>(CoinDobberAnimationHandler);
        EventManager.Instance.AddListener<RemoveSpawnedCharactersEvent>(RemoveSpawnedCharactersEventHandler);
        EventManager.Instance.AddListener<UpdateCurrentCoinsEvent>(UpdateCurrentCoinsEventHandler);
        EventManager.Instance.AddListener<GetCurrentAvailableCurrencyEvent>(GetCurrentAvailableCurrencyEventHandler);
        EventManager.Instance.AddListener<GameOverEvent>(GameOverEventHandler);
        EventManager.Instance.AddListener<AddBossAsTargetEvent>(AddBossAsTargetEventHandler);
    }

    public void OnRelease()
    {
        // SavedHiredDataOnDestroyEventHandler();
    }

    public void OnRemoveListeners()
    {

        EventManager.Instance.RemoveListener<IntialLevelSetUpEvent>(InitialLevelSetupEventHandler);
        EventManager.Instance.RemoveListener<BaseSelectedEvent>(BaseSelectedEventHandler);
        EventManager.Instance.RemoveListener<SpawnTurretEvent>(SpawnTurretEventHandler);
        EventManager.Instance.RemoveListener<UpdateTimerEvent>(UpdateTimerEventHandler);
        EventManager.Instance.RemoveListener<CoinDobberAnimation>(CoinDobberAnimationHandler);
        EventManager.Instance.RemoveListener<RemoveSpawnedCharactersEvent>(RemoveSpawnedCharactersEventHandler);
        EventManager.Instance.RemoveListener<UpdateCurrentCoinsEvent>(UpdateCurrentCoinsEventHandler);
        EventManager.Instance.RemoveListener<GetCurrentAvailableCurrencyEvent>(GetCurrentAvailableCurrencyEventHandler);
        EventManager.Instance.RemoveListener<GameOverEvent>(GameOverEventHandler);
        EventManager.Instance.RemoveListener<AddBossAsTargetEvent>(AddBossAsTargetEventHandler);

    }

    public void OnStarted()
    {
    }

    public void OnUpdate()
    {
        if (!Handler.WavesCompleted)
        {
            EventManager.Instance.TriggerEvent(new UpdateTimerEvent());
            EventManager.Instance.TriggerEvent(new EnemySpawnEvent(Handler.CurrentWave, Handler.Timer));
        }

        // MonoHelper.Instance.FaceCamera(Handler.Camera, Handler.HouseSlider.transform);
    }

    public void OnVisible()
    {
    }
}
