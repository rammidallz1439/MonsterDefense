using Syntax.Game;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Vault;

public class GlobalAnimationsController : MonoBehaviour
{

    [Header("PlayerAnimation")]
    [SerializeField] private Animator CharacterAnimator;
    [SerializeField] private ShootingMachineBase _shootingMachineBase;

    public void OnAttackAnimationCompleted()
    {
        EventManager.Instance.TriggerEvent(new ChangeToIdleAnimationEvent(CharacterAnimator));

    }



 
}
