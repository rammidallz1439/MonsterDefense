using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vault;

public class EnemyAniamtionEventsController : MonoBehaviour
{
    [SerializeField] private Animator m_Animator = null;
    [SerializeField] private GameObject m_Weapon = null;
    [SerializeField] private Enemy Enemy = null;
    /// <summary>
    /// Destroys Enemy Gameobject after Death Animation Completed
    /// </summary>
    public void OnDeathAniamtionCompleted()
    {
        Destroy(transform.parent.gameObject);
        if (Enemy !=null)
        {
            if (Enemy.IsEnemyBoss)
            {
                EventManager.Instance.TriggerEvent(new GameOverEvent());
            }
        }
    }

    public void RevertBossAnimation()
    {
        if (m_Animator != null && m_Weapon != null)
        {
           
            EventManager.Instance.TriggerEvent(new RevertBossAnimationEvent(m_Animator, m_Weapon));

        }
    }

    public void BossIdleEvent()
    {
        if (m_Animator != null && m_Weapon != null)
        {
            EventManager.Instance.TriggerEvent(new RevertBossIdleAnimationEvent(m_Animator, m_Weapon));
        }
    }
}
