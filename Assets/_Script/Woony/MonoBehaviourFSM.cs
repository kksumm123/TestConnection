using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoBehaviourFSM : MonoBehaviour
{
    #region FSM
    Func<IEnumerator> _currentFSM;
    /// <summary>
    /// CurrentFSM 호출바로 아래라인에 new WaitForSeconds 호출하지마세요.
    /// </summary>
    protected Func<IEnumerator> CurrentFSM
    {
        get => _currentFSM;
        set
        {
            this.StopCo(currentFSMHandle);

            _currentFSM = value;
            currentFSMHandle = null;
        }
    }
    Coroutine currentFSMHandle;
    #endregion FSM

    IEnumerator Start()
    {
        OnStart();

        var isTrue = true;
        while (isTrue)
        {
            Func<IEnumerator> preFSM = CurrentFSM;

            currentFSMHandle = StartCoroutine(CurrentFSM());

            //FSM안에서 에러 발생시 무한 루프 도는 것 방지
            if (currentFSMHandle == null && preFSM == CurrentFSM)
                yield return null;

            while (currentFSMHandle != null)
                yield return null;
        }
    }

    /// <summary>
    /// must be setted, CurrentFSM = FSM;
    /// </summary>
    protected abstract void OnStart();
}
