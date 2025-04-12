using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vault
{
    public interface IState<T> 
    {
        /// <summary>
        /// Initial State of Object
        /// </summary>
        /// <param name="owner"></param>
        void Enter(T owner);

        /// <summary>
        /// Excute any Additional behaviour of the Object
        /// </summary>
        /// <param name="owner"></param>
        void Execute(T owner);

        /// <summary>
        /// logic before the state Exit
        /// </summary>
        /// <param name="owner"></param>
        void Exit(T owner);
    }
}

