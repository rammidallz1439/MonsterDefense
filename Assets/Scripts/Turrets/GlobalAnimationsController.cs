using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vault;

public class GlobalAnimationsController : MonoBehaviour
{

    [Header("PlayerAnimation")]
    [SerializeField] private Animator CharacterAnimator;

    [Space(10)]
    [Header("Chararcter Ref")]
    [SerializeField] private ShootingMachine ShootingMachine;
    public void OnAttackAnimationCompleted()
    {
      // EventManager.Instance.TriggerEvent(new ChangeToIdleAnimationEvent(ShootingMachine,CharacterAnimator));

    }
}
