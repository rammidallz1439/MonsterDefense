using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vault;

public class HouseHandler : MonoBehaviour
{
    [SerializeField] private Slider _healthBar;
    [SerializeField] private float _health;
    [SerializeField] private GameObject _gameOverPanel;

    private void Start()
    {
        _healthBar.maxValue = _health;
        _healthBar.value = _health;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag is "Enemy")
        {
            other.transform.GetComponent<Enemy>().Reached = true;
            GenericEventsController.Instance.ChangeAnimationEvent(other.transform.GetComponent<Enemy>().Animator, GameConstants.EnemyAttack);
        }

        if (other.gameObject.tag is "EnemyWeapon")
        {
            TakeDamage(other.transform.GetComponent<EnemyWeapon>().AttackPower);
        }
    }

    async void TakeDamage(float damage)
    {
        if (_health > 0)
        {
            _health -= damage;
            _healthBar.value = _health;
        }

        if (_health <= 0)
        {
            _gameOverPanel.SetActive(true);
            await FirebaseManager.Instance.LoadCollectionDataAsync<PlayerData>(GameConstants.PlayerData, GameConstants.PlayerDataDoc, async (data) =>
            {
                EventManager.Instance.TriggerEvent(new GetCurrentAvailableCurrencyEvent((handler) =>
                {
                    data.Currency = handler.CurrentCoins;
                    data.CurrentLevel = handler.CurrentLevel;
                }));


                await FirebaseManager.Instance.UpdateCollectionDataAsync(GameConstants.PlayerData, GameConstants.PlayerDataDoc, data);
            }, () =>
            {
                MonoHelper.Instance.PrintMessage("Data noesn't Exsists check firebase", "black");
            });
        }
    }
}
