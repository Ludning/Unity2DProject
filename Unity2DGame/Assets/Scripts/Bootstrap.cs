using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Bootstrap
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    static void Initialize()
    {
        /*UserData userdata = DataManager.LoadData<UserData>("UserData");
        GameData gameData = Addressables.LoadAssetAsync<GameData>("Data/data.asset").WaitForCompletion();

        gameData.Init();

        if (userdata == null)
        {
            userdata = MakeFirstUserData();

            userdata.Save();
        }
        //�׽�Ʈ
        TestUserSetting(userdata, gameData);*/


        GameManager.Instance.UserData = DataManager.Instance.LoadJsonData<UserData>("UserData");
        if (GameManager.Instance.UserData == null)
        {
            GameManager.Instance.UserData = MakeFirstUserData();
            GameManager.Instance.UserData.Save();
        }

        TestUserData(GameManager.Instance.UserData);
    }



    #region ���ӽ��� �ʱ�ȭ
    //�׽�Ʈ�� ���� ������ ����
    static void TestUserData(UserData userData)
    {
        userData.inventoryItem[0] = 1;
        userData.inventoryItem[1] = 2;

        userData.equipmentAttack[0] = 1;
        userData.equipmentAttack[1] = 2;
        userData.equipmentAttack[2] = 3;

        userData.equipmentSkill = 4;

        userData.equipmentSpecial = 6;
    }

    // ù ���� ������ ����
    private static UserData MakeFirstUserData()
    {
        UserData userData = new UserData();

        userData.playerStatus = new Status();
        userData.gold = 100;

        var json = JsonConvert.SerializeObject(userData);
        DataManager.Instance.SaveJsonData(typeof(UserData).Name, json);

        return userData;
    }
    #endregion

}
