using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RecycleObject : MonoBehaviour
{
    System.Action<RecycleObject> restore;

    public virtual void InitializeByFactory(System.Action<RecycleObject> restore)
    {
        this.restore = restore;
    }

    public virtual void Restore()
    {
        restore(this);
    }
}
