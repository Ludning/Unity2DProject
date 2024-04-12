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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
