using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Vault
{
    public class GenericEventsController : IController
    {
        private static GenericEventsController instance;
        string currnetState;
        public static GenericEventsController Instance
        {
            get
            {
                instance = new GenericEventsController();
                return instance;
            }

        }

        /// <summary>
        /// Changes Animation State without links in the animator
        /// </summary>
        /// <param name="anim"></param>
        /// <param name="animName"></param>
        public void ChangeAnimationEvent(Animator anim, string animName)
        {
            if (currnetState == animName) return;
            anim.Play(animName);
            currnetState = animName;
        }

        /// <summary>
        /// play a non loop animation in a loop
        /// </summary>
        /// <param name="anim"></param>
        /// <param name="animName"></param>
        public void PlayNonloopAnimation(Animator anim, string animName)
        {
            anim.Play(animName, -1, 0);
            currnetState = animName;
        }



        /// <summary>
        /// Animates Button Click
        /// </summary>
        /// <param name="button"></param>
        public void AnimateButtonClick(Button button)
        {
            button.transform.DOScale(0.8f, 0.1f).OnComplete(() =>
            {
                button.transform.DOScale(1f, 0.1f);
            });
        }

        /// <summary>
        /// use this for ui popup animation 
        /// </summary>
        /// <param name="transfromToPop"></param>
        public void PopUpEvent(GameObject transfromToPop)
        {
            transfromToPop.transform.DOScale(1, 0.5f).SetEase(Ease.OutBounce);
        }

        /// <summary>
        /// Gets a Random vaue from a Enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetRandomEnumValue<T>()
        {
            Array values = Enum.GetValues(typeof(T));
            System.Random random = new System.Random();
            int randomIndex = random.Next(values.Length);
            return (T)values.GetValue(randomIndex);
        }

        /// <summary>
        /// Shuffles the given List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public void ShuffleList<T>(List<T> list)
        {
            System.Random rng = new System.Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        /// <summary>
        /// uses playerprefs to save an integer and checks if playerpref already exsists and creates one with a default value of 1 if not
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SaveInt(string name, int SetValue, int defaultValue = 1)
        {
            if (PlayerPrefs.HasKey(name))
            {
                int value = PlayerPrefs.GetInt(name);
                SetValue = value;
            }
            else
            {
                SetValue = defaultValue;
                PlayerPrefs.SetInt(name, SetValue);
                PlayerPrefs.Save();
            }

        }

        /// <summary>
        /// Adds Wobble effect to the Transform
        /// </summary>
        public void StartWobble(Transform transform, float angle, float duration, float originalRotation)
        {
            // Create a sequence for the wobble
            Sequence wobbleSequence = DOTween.Sequence();

            // Rotate to one side
            wobbleSequence.Append(transform.DOLocalRotate(new Vector3(0, 0, angle), duration / 2, RotateMode.Fast))
                          .SetEase(Ease.InOutSine);

            // Rotate to the opposite side
            wobbleSequence.Append(transform.DOLocalRotate(new Vector3(0, 0, -angle), duration / 2, RotateMode.Fast))
                          .SetEase(Ease.InOutSine);

            // Return to original rotation
            wobbleSequence.Append(transform.DOLocalRotate(new Vector3(0, 0, originalRotation), duration / 2, RotateMode.Fast))
                          .SetEase(Ease.InOutSine);

            // Loop infinitely
            //wobbleSequence.SetLoops(-1, LoopType.Restart);
        }

        /// <summary>
        /// Use to check the saved state of int type data in playerprefs and initialize with a default value if not saved
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="data"></param>
        /// <param name="defaultValue"></param>
        public void CheckIntData(string tag, ref int data, int defaultValue)
        {
            if (PlayerPrefs.HasKey(tag))
            {
                data = PlayerPrefs.GetInt(tag);
            }
            else
            {
                data = defaultValue;
                PlayerPrefs.SetInt(tag, data);
                PlayerPrefs.Save();
            }
        }

        /// <summary>
        /// Save the data to Player prefs in a string format
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="data"></param>
        /// <param name="onFinish"></param>
        public void SaveStringPlayerPrefs(string tag, string data, Action onFinish = null)
        {
            PlayerPrefs.SetString(tag, data);
            onFinish?.Invoke();
        }

        /// <summary>
        /// Fetch string type Data from Player prefs and returns null if Data doesn't exsists
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="onFinish"></param>
        /// <returns></returns>
        public string GetStringPlayerPrefs(string tag)
        {
            if (PlayerPrefs.HasKey(tag))
            {
                return PlayerPrefs.GetString(tag);
            }
            else
            {

                return "";
            }
        }
        #region Contracts
        public void OnInitialized()
        {
        }

        public void OnVisible()
        {
        }

        public void OnStarted()
        {
        }

        public void OnRegisterListeners()
        {
        }

        public void OnRemoveListeners()
        {
        }

        public void OnRelease()
        {
        }


        #endregion
    }
}
