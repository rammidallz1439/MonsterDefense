using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Vault;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private NavMeshAgent _agent;
    public float Health;
    [SerializeField] private Slider _healthBar;
    [SerializeField] private Canvas _worldCanvas;
    [SerializeField] private int _dropValue;
    [SerializeField] private GameObject _Coin;

    public float AttackPower;
    public Animator Animator;
    public bool Reached;
    public EnemyWeapon EnemyWeapon;


    public bool IsEnemyBoss;


    private void Start()
    {
        _agent.speed = _speed;
        _healthBar.maxValue = Health;
        _healthBar.value = Health;
        _worldCanvas.worldCamera = Camera.main;
        EnemyWeapon.AttackPower = AttackPower;
    }
    private void Update()
    {
        MonoHelper.Instance.FaceCamera(Camera.main, _healthBar.transform);

        EventManager.Instance.TriggerEvent(new EnemyMovementEvent(_agent, this));


    }


    /// <summary>
    /// Damages the Enemy as per the value
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(float damage)
    {
        if (Health > 0)
        {
            Health -= damage;
            _healthBar.value = Health;
        }

        if (Health <= 0)
        {
          
          
            GenericEventsController.Instance.ChangeAnimationEvent(Animator, GameConstants.EnemyDeath);
            DropCoins();
        }
    }

    /// <summary>
    /// drops coins and aniamatevthem as adobber
    /// </summary>
    void DropCoins()
    {
        GameObject coin = null;
        EventManager.Instance.TriggerEvent(new UpdateCurrentCoinsEvent(_dropValue));
        if (_dropValue <= 10)
        {
            for (int i = 0; i < GameConstants.SmallDropValue; i++)
            {
                coin = Instantiate(_Coin, transform.position, Quaternion.identity);
            }
        }
        else if (_dropValue <= 20 && _dropValue > 10)
        {
            for (int i = 0; i < GameConstants.MediumDropValue; i++)
            {
                coin = Instantiate(_Coin, transform.position, Quaternion.identity);
            }
        }
        else if (_dropValue > 20 && _dropValue < 100)
        {
            for (int i = 0; i < GameConstants.LargeDropValue; i++)
            {
                coin = Instantiate(_Coin, transform.position, Quaternion.identity);
            }
        }
        else if (_dropValue >= 100)
        {
            for (int i = 0; i < GameConstants.ExtraLargeDropValue; i++)
            {
                coin = Instantiate(_Coin, transform.position, Quaternion.identity);
            }
        }

    }


 

}
