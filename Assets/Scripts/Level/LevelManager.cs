using DG.Tweening;
using MEC;
using Newtonsoft.Json;
using Syntax.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Vault;
using Object = UnityEngine.Object;

public class LevelManager
{
    protected LevelHandler Handler;

    #region EventHandler
    protected async void InitialLevelSetupEventHandler(IntialLevelSetUpEvent e)
    {
        await FirebaseManager.Instance.LoadCollectionDataAsync<PlayerData>(GameConstants.PlayerData, GameConstants.PlayerDataDoc, (data) =>
        {
            Handler.LevelDetails = Handler.LevelScriptables[data.CurrentSelectedLevel - 1];
            Handler.CoinCount.text = data.Currency.ToString();
            Handler.CurrentCoins = data.Currency;
            Handler.CurrentLevel = data.CurrentLevel;
        });
        Handler.CurrentWave = Handler.LevelDetails.WaveData.Waves[Handler.CurrentWaveCount - 1];
        Handler.WaveCount.text = "Wave: " + Handler.CurrentWaveCount + "/" + Handler.LevelDetails.WaveData.Waves.Count;

        Handler.Timer = Handler.CurrentWave.WaveTime;

        Handler.RestartButton.onClick.AddListener(async () =>
        {
            await FirebaseManager.Instance.LoadCollectionDataAsync<PlayerData>(GameConstants.PlayerData, GameConstants.PlayerDataDoc, async (data) =>
            {
                data.Currency = Handler.CurrentCoins;
                data.CurrentLevel = Handler.CurrentLevel;

                await FirebaseManager.Instance.UpdateCollectionDataAsync(GameConstants.PlayerData, GameConstants.PlayerDataDoc, data);
            }, () =>
            {
                MonoHelper.Instance.PrintMessage("Data noesn't Exsists check firebase", "black");
            });
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });

        Handler.PlayAgainButton.onClick.AddListener(() => { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); });

        Handler.SkipButton.onClick.AddListener(() =>
        {
            Handler.CurrentSelectedBase.transform.GetComponent<MeshRenderer>().material.color = Handler.CurrentSelectedBase.GetColor();
            Handler.CurrentSelectedBase = null;
            Handler.BaseSelected = false;
        });
    }

    protected void BaseSelectedEventHandler(BaseSelectedEvent e)
    {
        if (!Handler.BaseSelected)
        {
            Handler.BaseSelected = true;
            Handler.CurrentSelectedBase = e.BaseHandler;
            Handler.SelectionPanel.gameObject.SetActive(true);
            e.BaseHandler.transform.GetComponent<MeshRenderer>().material.color = Color.red;

        }

        /*    if (Handler.CurrentSelectedBase == null)
            {
                Handler.CurrentSelectedBase = e.BaseHandler;
            }
            else
            {
                //Handler.CurrentSelectedBase.transform.DOMoveY(Handler.CurrentSelectedBase.InitialYPos, 0.5f);
               // Handler.CurrentSelectedBase.transform.GetComponent<MeshRenderer>().material.color = Handler.CurrentSelectedBase.GetColor();


            }*/
        // e.BaseHandler.transform.DOMoveY(1f, 0.5f);
        // 
    }

    protected void SpawnTurretEventHandler(SpawnTurretEvent e)
    {
        if (Handler.CurrentSelectedBase != null && Handler.CurrentSelectedBase.Occupied == false)
        {
            GameObject obj = MonoHelper.Instance.InstantiateObject(e.Turret, Handler.CurrentSelectedBase.SpawnPoint.transform.position, Quaternion.identity);
            Handler.CurrentSelectedBase.Occupied = true;
            ShootingMachineBase machine = obj.transform.GetComponent<ShootingMachineBase>();
            Vault.ObjectPoolManager.Instance.InitializePool(machine.TurretDataScriptable.Bullet.gameObject, 15, machine.AmmoPoint);
            /*   machine.Timer = Handler.HireDuration;
               machine.SpawnedBase = Handler.CurrentSelectedBase;
               machine.BaseIndex = Handler.CurrentSelectedBase.Id;*/
            Handler.SpawnedCharacters.Add(machine);
            Handler.BaseSelected = false;
        }

    }

    protected void AddBossAsTargetEventHandler(AddBossAsTargetEvent e)
    {
        foreach (var item in Handler.SpawnedCharacters)
        {
            item.Target = e.BossEnemy;
        }
    }



    protected void UpdateTimerEventHandler(UpdateTimerEvent e)
    {
        if (Handler.LevelDetails != null)
        {
            if (Handler.CurrentWaveCount <= Handler.LevelDetails.WaveData.Waves.Count)
            {
                if (Handler.Timer > 0)
                {
                    Handler.Timer -= Time.deltaTime;
                    UpdateTimerDisplay(Handler.Timer);
                }
                else
                {

                    Handler.Timer = 0;
                    TimerEnded();
                    UpdateTimerDisplay(Handler.Timer);
                }

            }
        }


    }

    protected void CoinDobberAnimationHandler(CoinDobberAnimation e)
    {
        Vector3 coinScreenPosition = Camera.main.WorldToScreenPoint(e.CoinPrefab.transform.position);

        Vector3 targetScreenPosition = Handler.CoinCount.rectTransform.position;

        Vector3 targetWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(targetScreenPosition.x, targetScreenPosition.y, coinScreenPosition.z));

        Timing.RunCoroutine(DelayCoinsDeath(e.CoinPrefab, targetWorldPosition).CancelWith(e.CoinPrefab));
    }


    /*    protected void RemoveSpawnedCharactersEventHandler(RemoveSpawnedCharactersEvent e)
        {
            e.Machine.SpawnedBase.transform.GetComponent<MeshRenderer>().material.color = Handler.CurrentSelectedBase.GetColor();
            e.Machine.SpawnedBase.Occupied = false;
            Handler.SpawnedCharacters.Remove(e.Machine);
        }
    */

    protected void UpdateCurrentCoinsEventHandler(UpdateCurrentCoinsEvent e)
    {
        Handler.CurrentCoins += e.Value;
        /*      await FirebaseManager.Instance.LoadCollectionDataAsync<PlayerData>(GameConstants.PlayerData, GameConstants.PlayerDataDoc, async (data) =>
              {
                  data.Currency = Handler.CurrentCoins;
                  await FirebaseManager.Instance.UpdateCollectionDataAsync(GameConstants.PlayerData, GameConstants.PlayerDataDoc, data);
              }, () =>
              {
                  MonoHelper.Instance.PrintMessage("Data noesn't Exsists check firebase", "black");
              });*/
    }

    protected void GetCurrentAvailableCurrencyEventHandler(GetCurrentAvailableCurrencyEvent e)
    {
        e.Handler?.Invoke(Handler);
    }
    protected async void GameOverEventHandler(GameOverEvent e)
    {
        Handler.LevelCompleted = true;
        Handler.LevelSucessPanel.SetActive(true);
        Handler.CurrentLevel++;
        await FirebaseManager.Instance.LoadCollectionDataAsync<PlayerData>(GameConstants.PlayerData, GameConstants.PlayerDataDoc, async (data) =>
        {
            data.Currency = Handler.CurrentCoins;
            data.CurrentLevel = Handler.CurrentLevel;

            await FirebaseManager.Instance.UpdateCollectionDataAsync(GameConstants.PlayerData, GameConstants.PlayerDataDoc, data);
        }, () =>
        {
            MonoHelper.Instance.PrintMessage("Data noesn't Exsists check firebase", "black");
        });
    }


    protected void OnAttackSkillEventHandler(OnAttackSkillEvent e)
    {
        if (Handler.SkillTreeScriptable.SkillTreeData.SkillTreeItems[0].Brought)
        {
            float percent = e.ShootingMachineBase.AttackPower / 100;
            float increased = percent * 10;
            e.AttackPower.Invoke(increased);
        }
        else if (Handler.SkillTreeScriptable.SkillTreeData.SkillTreeItems[3].Brought)
        {
            float percent = e.ShootingMachineBase.AttackPower / 100;
            float increased = percent * 30;
            e.AttackPower.Invoke(increased);
        }
        else if (Handler.SkillTreeScriptable.SkillTreeData.SkillTreeItems[7].Brought)
        {
            float percent = e.ShootingMachineBase.AttackPower / 100;
            float increased = percent * 50;
            e.AttackPower.Invoke(increased);
        }
        else
        {
            MonoHelper.Instance.PrintMessage("No attack skill brought", "green");
        }
    }
    protected void OnHealthSkillEventHandler(OnHealthSkillEvent e)
    {
        if (Handler.SkillTreeScriptable.SkillTreeData.SkillTreeItems[1].Brought)
        {
            float percent = e.Health / 100;
            float increased = percent * 10;
            e.HealthIncreased.Invoke(percent);
        }
        else if (Handler.SkillTreeScriptable.SkillTreeData.SkillTreeItems[5].Brought)
        {
            float percent = e.Health / 100;
            float increased = percent * 30;
            e.HealthIncreased.Invoke(percent);
        }
        else if (Handler.SkillTreeScriptable.SkillTreeData.SkillTreeItems[8].Brought)
        {
            float percent = e.Health / 100;
            float increased = percent * 50;
            e.HealthIncreased.Invoke(percent);
        }
        else
        {
            MonoHelper.Instance.PrintMessage("No Health skill brought", "green");
        }
    }
    #endregion

    #region Functions
    IEnumerator<float> DelayCoinsDeath(GameObject coin, Vector3 targetWorldPosition)
    {
        yield return Timing.WaitForSeconds(0.5f);
        coin.transform.DOMove(targetWorldPosition, 0.5f)
              .SetEase(Ease.Linear)
              .OnComplete(() =>
              {
                  Handler.CoinCount.text = Handler.CurrentCoins.ToString();
                  MonoHelper.Instance.DestroyObject(coin);
              })
              .WaitForCompletion();
    }

    void UpdateTimerDisplay(float timeToDisplay)
    {
        int minutes = Mathf.FloorToInt(timeToDisplay / 60);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);
        Handler.TimerCount.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void TimerEnded()
    {


        if (Handler.CurrentWaveCount >= Handler.LevelDetails.WaveData.Waves.Count)
        {
            Enemy[] enemies = Object.FindObjectsOfType<Enemy>();
            if (enemies.Length > 0)
            {
                foreach (Enemy item in enemies)
                {
                    MonoHelper.Instance.DestroyObject(item.gameObject);
                }
            }

            EventManager.Instance.TriggerEvent(new SpawnBossEnemyEvent(Handler.LevelDetails.BossEnemy.EnemyPrefab, Handler.LevelDetails.BossEnemy.Health));

            Handler.Timer = 0;

            UpdateTimerDisplay(Handler.Timer);
            Handler.WavesCompleted = true;
        }
        else
        {
            Handler.CurrentWaveCount++;
            Handler.WaveCount.text = "Wave: " + Handler.CurrentWaveCount + "/" + Handler.LevelDetails.WaveData.Waves.Count;
            Handler.CurrentWave = Handler.LevelDetails.WaveData.Waves[Handler.CurrentWaveCount - 1];
            Handler.Timer = Handler.CurrentWave.WaveTime;
        }
    }

    /*    protected void SavedHiredDataOnDestroyEventHandler()
        {
            Debug.Log("<color=red> came to SaveData</color>");
            if (Handler.SpawnedCharacters.Count > 0)
            {
                foreach (var item in Handler.SpawnedCharacters)
                {
                    HireData data = new HireData
                    {
                        BaseIndex = item.BaseIndex,
                        CharacterIndex = item.Index,
                        SavedTimer = item.Timer,
                        LastSavedTime = DateTime.UtcNow
                    };
                    Handler.HireHandler.HireDataToSave.Add(data);
                }
                Handler.HireHandler.SavedHireData.HireData = Handler.HireHandler.HireDataToSave;
                // DataManager.Instance.SaveJson(Handler.HireHandler.SavedHireData, GameConstants.SaveHireData);
            }

        }*/

    #endregion
}
