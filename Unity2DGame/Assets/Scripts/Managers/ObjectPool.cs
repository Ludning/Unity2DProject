using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class ObjectPool : Manager<ObjectPool>
{
    GameObject sceneRig;
    public GameObject SceneRig
    {
        get 
        {
            if (sceneRig == null)
                sceneRig = GameObject.Find("SceneInstaller");
            return sceneRig; 
        }
        set { sceneRig = value; }
    }

    Dictionary<string, Queue<GameObject>> pool = new Dictionary<string, Queue<GameObject>>();
    int startPoolSize = 10;

    public GameObject GetGameObject(GameObject go)
    {
        if (!pool.ContainsKey(go.name))
            CreatePool(go);
        if (pool[go.name].Count <= 1)
            ExpansionPool(go);
        GameObject copy = pool[go.name].Dequeue();
        copy.SetActive(true);
        copy.transform.parent = SceneRig.transform;
        copy.transform.parent = null;
        return copy;
    }
    public void ReturnToPool(GameObject go)
    {
        if(go == null) 
            return;
        if (!pool.ContainsKey(go.name))
            CreatePool(go);
        go.transform.parent = transform;
        go.SetActive(false);
        pool[go.name].Enqueue(go);
    }
    private void CreatePool(GameObject go)
    {
        if (pool.ContainsKey(go.name))
            return;
        pool.Add(go.name, new Queue<GameObject>());

        for (int i = 0; i < startPoolSize; i++) 
        {
            GameObject copy = Instantiate(go);
            copy.name = go.name;
            ReturnToPool(copy);
        }
    }
    private void ExpansionPool(GameObject go)
    {
        for (int i = 0; i < startPoolSize; i++)
        {
            GameObject copy = Instantiate(go);
            copy.name = go.name;
            ReturnToPool(copy);
        }
    }
}
