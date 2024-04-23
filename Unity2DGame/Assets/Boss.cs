using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AI;

public class Boss : Monster
{
    private void Awake()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    public override void Init(string statusBarName)
    {
        string name = gameObject.name;
        if (statusBarPrefab == null)
        {
            statusBarPrefab = Addressables.LoadAssetAsync<GameObject>(statusBarName).WaitForCompletion();
        }
        statusBarBar = ObjectPool.Instance.GetGameObject(statusBarPrefab);
        statusBarBar.GetComponent<UI_TrackTarget>().Target = gameObject;
        uiStatusBar = statusBarBar.GetComponent<UIStatusBar>();

        statusBarBar.transform.SetParent(UIManager.Instance.GetCanvasData(CanvasType.SceneInformationCanvas).gameObject.transform);//Canvas.transform;
        UIManager.Instance.ShowUI_Status(statusBarBar);

        statusBarBar.transform.localScale = Vector3.one;


        MonsterData data = ResourceManager.Instance.GetScriptableData<MonsterData>($"{gameObject.name}Data");
        status.maxHp = data.maxHp;
        status.hp = data.maxHp;
        status.attack = data.attack;
        status.defence = data.defence;

        rigid = GetComponent<Rigidbody>();
        #region uiStatusBar √ ±‚»≠
        uiStatusBar.Init(gameObject.name, HpRatio);
        #endregion
    }
    public override void OnDie()
    {
        ItemData itemData = ResourceManager.Instance.GetScriptableData<ItemData>("ItemData");
        int index = Random.Range(0, itemData.items.Count * 3);

        if (index < itemData.items.Count)
        {
            GameObject go = ObjectPool.Instance.GetGameObject(ResourceManager.Instance.GetPrefab("DropItem"));
            go.GetComponent<DropItem>().Init(itemData.items[index]);
            go.transform.position = transform.position;
        }

        gameObject.GetComponent<NavMeshAgent>().enabled = true;

        statusBarBar.SetActive(false);
        ObjectPool.Instance.ReturnToPool(statusBarBar);
        statusBarBar = null;
        uiStatusBar = null;
        ObjectPool.Instance.ReturnToPool(gameObject);

        GameManager.Instance.UserData.Save();
        ScenesManager.Instance.LoadScene("EndScene");
    }

}
