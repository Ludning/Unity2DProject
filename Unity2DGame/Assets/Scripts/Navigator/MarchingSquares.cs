using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class MarchingSquares : MonoBehaviour
{
    /*  {0, 0, 0, 0}図楕採斗 0腰属 搾闘
    0腰属 搾闘      1腰属 搾闘
           けけけけけけ
           け        け
           け        け
           け        け
           け        け
           けけけけけけ
    3腰属 搾闘      2腰属 搾闘
    */
    int[] MarchingSquare =
    {
        0,    // 0000   蒸陥
        1,    // 0001   疎馬
        2,    // 0010   酔馬
        3,    // 0011   酔馬, 疎馬
        4,    // 0100   酔雌
        5,    // 0101   酔雌, 疎馬
        6,    // 0110   酔雌, 酔馬
        7,    // 0111   酔雌, 酔馬, 疎馬
        8,    // 1000   疎雌
        9,    // 1001   疎雌, 疎馬
        10,    // 1010  疎雌, 酔馬
        11,    // 1011  疎雌, 酔馬, 疎馬
        12,    // 1100  疎雌, 酔雌
        13,    // 1101  疎雌, 酔雌, 疎馬
        14,    // 1110  疎雌, 酔雌, 酔馬
        15,    // 1111  疎雌, 酔雌, 酔馬, 疎馬
    };
    bool[,] mapData = new bool[15, 15]
    {
        {false,false,false,false,false,false,false,false,false,false,false,false,false,false,false },
        {false, true, true, true, true, true, true, true, true, true, true, true, true, true,false },
        {false, true, true, true, true, true, true, true, true, true, true, true, true, true,false },
        {false, true, true, true, true, true, true, true, true, true, true, true, true, true,false },
        {false, true, true, true, true, true, true, true, true, true, true, true, true, true,false },
        {false, true, true, true, true, true, true, true, true, true, true, true, true, true,false },
        {false, true, true, true, true, true, true, true, true, true, true, true, true, true,false },
        {false, true, true, true, true, true, true, true, true, true, true, true, true, true,false },
        {false, true, true, true, true, true, true, true, true, true, true, true, true,false,false },
        {false, true, true, true, true, true, true, true, true, true, true, true,false,false,false },
        {false, true, true, true, true, true, true, true, true, true, true,false,false,false,false },
        {false, true, true, true, true, true, true, true, true, true,false,false,false,false,false },
        {false, true, true, true, true, true, true, true, true,false,false,false,false,false,false },
        {false, true, true, true, true, true, true, true,false,false,false,false,false,false,false },
        {false,false,false,false,false,false,false,false,false,false,false,false,false,false,false },
    };
    int[,] mapMSData = new int[14, 14];

    // Start is called before the first frame update
    void Start()
    {
        GetMSData();
        StringBuilder sb = new StringBuilder();
        for (int h = 0; h < mapMSData.GetLength(1); h++)
        {
            for(int w = 0; w < mapMSData.GetLength(0); w++)
            {
                sb.Append($"{Convert.ToString(mapMSData[h, w], 2)}_");
            }
            sb.AppendLine();
        }
        Debug.Log(sb);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void GetMSData()
    {
        int temp = 0;
        for (int h = 0; h < mapMSData.GetLength(0); h++)
        {
            for (int w = 0; w < mapMSData.GetLength(1); w++)
            {
                temp |= (mapData[h + 1, w] == true) ? 1 << 3 : 0;
                temp |= (mapData[h + 1, w + 1] == true) ? 1 << 2 : 0;
                temp |= (mapData[h, w + 1] == true) ? 1 << 1 : 0;
                temp |= (mapData[h, w] == true) ? 1 << 0 : 0;
                mapMSData[h, w] = temp;
                temp = 0;
            }
        }
    }
}
