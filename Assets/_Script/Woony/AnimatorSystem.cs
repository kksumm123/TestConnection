using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// EX
// animationSystem = new AnimationSystem<NPCAnimationStateType>(GetComponentInChildren<Animator>(true),
//                                                              NPCAnimationStateType.Idle,
//                                                              NPCAnimationStateType.Walk);

// void PlayAnimation(NPCAnimationStateType npcAnimationStateType)
// {
//     switch (npcAnimationStateType)
//     {
//         case NPCAnimationStateType.Idle:
//         case NPCAnimationStateType.Walk:
//             animationSystem.SetTrigger(npcAnimationStateType);
//             break;
//     }
// }
public class AnimatorSystem<T> where T : System.Enum
{
    private class AnimationMapInfo<U>
    {
        public U customEnum;
        public int hash;
    }

    public Animator Animator => _animator;

    private Animator _animator;
    private Dictionary<T, int> _animationMap = new Dictionary<T, int>();

    public AnimatorSystem(Animator animator)
    {
        this._animator = animator;
        AnimationMapInfo<T> info;

        var enums = Enum.GetNames(typeof(T));
        for (int i = 0; i < enums.Length; i++)
        {
            info = GetAnimationMapInfo((T)Enum.Parse(typeof(T), enums[i]));
            _animationMap[info.customEnum] = info.hash;
        }
    }

    public AnimatorSystem(Animator animator, params T[] customEnums)
    {
        _animator = animator;
        AnimationMapInfo<T> info;

        for (int i = 0; i < customEnums.Length; i++)
        {
            info = GetAnimationMapInfo(customEnums[i]);
            _animationMap[info.customEnum] = info.hash;
        }
    }

    private AnimationMapInfo<T> GetAnimationMapInfo(T _customEnum)
    {
        return new AnimationMapInfo<T>()
        {
            customEnum = _customEnum,
            hash = Animator.StringToHash(_customEnum.ToString())
        };
    }

    public void SetTrigger(T customEnum)
    {
        _animator.SetTrigger(_animationMap[customEnum]);
    }

    public void SetBool(T customEnum, bool value)
    {
        _animator.SetBool(_animationMap[customEnum], value);
    }

    public void SetFloat(T customEnum, float value)
    {
        _animator.SetFloat(_animationMap[customEnum], value);
    }
}
