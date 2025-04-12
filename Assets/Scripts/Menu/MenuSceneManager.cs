using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vault;
public class MenuSceneManager
{
    protected MenuSceneHandler Handler;

    #region Handlers
    protected void MenuSceneInitEventHandler(MenuSceneInitEvent e)
    {
        SetPlayerData();

        Handler.LevelText.text = "LV " + Handler.SelectedLevelIndex.ToString();
        if (Handler.SelectedLevelIndex == 1)
            Handler.PreviousButton.gameObject.SetActive(false);
        Handler.EnemiesLevelDescription.text = "Enemies Health x" + Handler.SelectedLevelIndex;

        Handler.NextButton.onClick.AddListener(() => { OnNextButtonClicked(); });
        Handler.PreviousButton.onClick.AddListener(() => { OnPreviousButtonClicked(); });

        Handler.SkillTreeButton.onClick.AddListener(() => { OnSkillTreeButtonClicked(); });
        Handler.BattleButton.onClick.AddListener(() => { OnBattleButtonClicked(); });



    }

    #endregion

    #region Methods

    private async void OnNextButtonClicked()
    {
        Handler.SelectedLevelIndex++;

        if (!Handler.PreviousButton.gameObject.activeSelf)
        {
            Handler.PreviousButton.gameObject.SetActive(true);
        }
        ButtonsCommonData();
        if (Handler.LevelData != null)
        {
            if (Handler.SelectedLevelIndex > 1)
            {
                if (Handler.LevelDict.ContainsKey(Handler.SelectedLevelIndex.ToString()))
                {
                    Handler.SelectedLevel = Handler.LevelDict[Handler.SelectedLevelIndex.ToString()];
                    if (Handler.SelectedLevelIndex <= Handler.PlayerData.CurrentLevel)
                    {
                        Handler.PlayerData.CurrentSelectedLevel = Handler.SelectedLevelIndex;
                    }

                }
                else
                {
                    Handler.SelectedLevel = Random.Range(0, 3);
                    Handler.LevelDict.Add(Handler.SelectedLevelIndex.ToString(), Handler.SelectedLevel);
                    if (Handler.SelectedLevelIndex <= Handler.PlayerData.CurrentLevel)
                    {
                        Handler.PlayerData.CurrentSelectedLevel = Handler.SelectedLevelIndex;
                    }

                    Handler.LevelData.LevelDict = Handler.LevelDict;
                    await FirebaseManager.Instance.SaveCollectionDataAsync(GameConstants.LevelData, GameConstants.DefinedLevelData, Handler.LevelData);


                }

            }
        }
    }

    private void OnPreviousButtonClicked()
    {
        if (Handler.SelectedLevelIndex > 1)
        {
            Handler.SelectedLevelIndex--;
        }

        if (Handler.SelectedLevelIndex < 2)
        {
            Handler.PreviousButton.gameObject.SetActive(false);
        }
        ButtonsCommonData();
    }

    private void OnSkillTreeButtonClicked()
    {
        Handler.SkillTreeHolder.SetActive(true);
        SetSkillTreeData();
    }
    private void ButtonsCommonData()
    {
        Handler.LevelText.text = "LV " + Handler.SelectedLevelIndex.ToString();
        if (Handler.SelectedLevelIndex > Handler.PlayerData.CurrentLevel)
        {
            Handler.LockObject.SetActive(true);
        }
        else
        {
            Handler.LockObject.SetActive(false);
        }
    }

    private async void SetSkillTreeData()
    {
        Handler.SkillTreeRect.verticalNormalizedPosition = 0f;
        for (int i = 0; i < Handler.SkillPieces.Count; i++)
        {

            Handler.SkillPieces[i].Description.text = FirebaseManager.Instance.SkillTreeData.SkillTreeDetails[i].Description;
            Handler.SkillPieces[i].CostText.text = FirebaseManager.Instance.SkillTreeData.SkillTreeDetails[i].Price.ToString();
            Handler.SkillPieces[i].Price = FirebaseManager.Instance.SkillTreeData.SkillTreeDetails[i].Price;
        }

        await FirebaseManager.Instance.LoadCollectionDataAsync<LevelData>(GameConstants.LevelDictCollection, GameConstants.LevelDictDocument, (data) =>
        {
            Handler.LevelData = data;
        }, () =>
        {
            MonoHelper.Instance.PrintMessage("Sorry You Dont Have the level data saved in cloud yet", "blue");
        });
    }

    private async void SetPlayerData()
    {
        await FirebaseManager.Instance.LoadCollectionDataAsync<PlayerData>(GameConstants.PlayerData, GameConstants.PlayerDataDoc, async (data) =>
        {
            Handler.PlayerData = data;
            Handler.CurrencyText.text = data.Currency.ToString();
            Handler.SkillCurrencyText.text = data.Currency.ToString();
            Handler.SelectedLevelIndex = data.CurrentSelectedLevel;
        }, async () =>
        {
            PlayerData data = new PlayerData
            {
                Currency = 500,
                CurrentLevel = 1,
                CurrentSelectedLevel = 1,

            };

            await FirebaseManager.Instance.SaveCollectionDataAsync(GameConstants.PlayerData, GameConstants.PlayerDataDoc, data, async () =>
            {
                Handler.PlayerData = data;
                Handler.CurrencyText.text = data.Currency.ToString();
                Handler.SkillCurrencyText.text = data.Currency.ToString();
                Handler.PlayerData.CurrentSelectedLevel = data.CurrentSelectedLevel;
            });
        });

        await FirebaseManager.Instance.LoadCollectionDataAsync<LevelData>(GameConstants.LevelData, GameConstants.DefinedLevelData,
            async (data) =>
            {
                Handler.LevelData = data;
                Handler.LevelDict = Handler.LevelData.LevelDict;
            }, async () =>

            {
                Handler.LevelData.LevelDict = new Dictionary<string, int>();
                for (int i = 0; i < Handler.Levels.Count; i++)
                {
                    Handler.LevelData.LevelDict.Add((i + 1).ToString(), Handler.Levels[i].Id);
                }
                Handler.LevelDict = Handler.LevelData.LevelDict;
                await FirebaseManager.Instance.SaveCollectionDataAsync(GameConstants.LevelData, GameConstants.DefinedLevelData, Handler.LevelData);

            });


    }

    private async void OnBattleButtonClicked()
    {

        if (Handler.SelectedLevelIndex <= Handler.PlayerData.CurrentLevel)
        {
            await FirebaseManager.Instance.UpdateCollectionDataAsync(GameConstants.PlayerData, GameConstants.PlayerDataDoc, Handler.PlayerData, () =>
            {
                SceneManager.LoadScene(2);
            });
        }
      
    }
    #endregion
}
