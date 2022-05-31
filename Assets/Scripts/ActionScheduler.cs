using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* using an action scheduler to break cyclic dependency of enemyfighter and enemymover with each other
     Used to reduce complications in the code and make debugging easier in case one class fails 
   */
public class ActionScheduler : MonoBehaviour
{
    IAction currentAction;

    public void StartAction(IAction action)
    {
        if (currentAction == action) return;
        if (currentAction != null)
        {
            currentAction.Cancel();  // here cancel is individually defined for each different action using the same interface IAction
        }
        currentAction = action;


    }

    public void CancelCurrentAction()
    {
        StartAction(null);
    }
}
