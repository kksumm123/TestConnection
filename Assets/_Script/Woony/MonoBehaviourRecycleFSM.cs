using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoBehaviourRecycleFSM : RecycleObject
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

            if (_currentFSM != null)
            {
                preFSM = _currentFSM;
                currentFSMHandle = StartCoroutine(CurrentFSM());
            }
        }
    }
    Coroutine currentFSMHandle;
    #endregion FSM

    protected void StartMainFSM()
    {
        CurrentFSM = null;
        //MainFSMHandle = this.StopAndStartCo(MainFSMHandle, MainFSM());
    }

    private Coroutine MainFSMHandle;
    private Func<IEnumerator> preFSM;
    private IEnumerator MainFSM()
    {
        var isTrue = true;
        while (isTrue)
        {
            while (CurrentFSM == null) yield return null;

            preFSM = CurrentFSM;

            currentFSMHandle = StartCoroutine(CurrentFSM());

            //FSM안에서 에러 발생시 무한 루프 도는 것 방지
            if (currentFSMHandle == null && preFSM == CurrentFSM)
                yield return null;

            while (currentFSMHandle != null)
                yield return null;
        }
    }
}
