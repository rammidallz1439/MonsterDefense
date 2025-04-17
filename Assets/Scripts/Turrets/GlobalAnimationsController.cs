using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vault;

public class GlobalAnimationsController : MonoBehaviour
{

    [Header("PlayerAnimation")]
    [SerializeField] private Animator CharacterAnimator;

    public void OnAttackAnimationCompleted()
    {
       EventManager.Instance.TriggerEvent(new ChangeToIdleAnimationEvent(CharacterAnimator));

    }
}
