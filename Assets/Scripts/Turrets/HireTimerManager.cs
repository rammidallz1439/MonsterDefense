using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vault;

namespace BGS.Game
{
    public class HireTimerManager
    {
        protected HireTimerHandler Handler;

        #region Events

        protected void InitHireTimersEventHandler(InitHireTimersEvent e)
        {
            
               // Handler.SavedHireData = DataManager.Instance.LoadJson<SavedHireData>(GameConstants.SaveHireData);
            if(Handler.SavedHireData != null)
            {
                Debug.Log("Came to load the data for saved characters");
                foreach (var item in Handler.SavedHireData.HireData)
                {
                    Handler.HireDataToSave.Add(item);
                    float timer = item.SavedTimer;
                    DateTime lastSaveTime = item.LastSavedTime;

                    TimeSpan timeElapsed = DateTime.UtcNow - lastSaveTime;
                    timer = Mathf.Max(0, timer - (float)timeElapsed.TotalSeconds);

                    if (timer > 0)
                    {
                        BaseHandler baseHandler = Handler.BaseHandlers.Find(x => x.Id == item.BaseIndex);
                        baseHandler.Occupied = true;
                        ShootingMachine machine = Handler.Characters.Find(x => x.Index == item.CharacterIndex);
                        GameObject character = MonoHelper.Instance.InstantiateObject(machine.gameObject);
                        character.transform.position = baseHandler.SpawnPoint.position;
                        character.transform.GetComponent<ShootingMachine>().Timer = timer;
                       // Vault.ObjectPoolManager.Instance.InitializePool(machine.TurretDataScriptable.Bullet.gameObject, 15);
                    }
                    else
                    {
                        Debug.Log("Hire Time Is Ended: Hire new Character");
                    }

                }

            }

        }


        protected void AddSpawnedBasesEventHandler(AddSpawnedBasesEvent e)
        {
            Handler.BaseHandlers.Add(e.BaseHandler);
        }
        #endregion

        #region Methods


        #endregion
    }
}
