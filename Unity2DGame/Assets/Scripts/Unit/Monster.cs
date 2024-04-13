using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Unit
{
    public override void Init(string statusBarName)
    {
        base.Init(statusBarName);
        #region uiStatusBar √ ±‚»≠
        uiStatusBar.Init(gameObject.name, HpRatio);
        #endregion
    }
    public override void OnDie()
    {
        GameManager.Instance.RemoveMonster(this);
        base.OnDie();
    }
}
