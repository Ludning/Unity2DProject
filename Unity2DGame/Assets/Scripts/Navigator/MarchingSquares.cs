using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class MarchingSquares : MonoBehaviour
{
    /*  {0, 0, 0, 0}왼쪽부터 0번째 비트
    0번째 비트      1번째 비트
           ㅁㅁㅁㅁㅁㅁ
           ㅁ        ㅁ
           ㅁ        ㅁ
           ㅁ        ㅁ
           ㅁ        ㅁ
           ㅁㅁㅁㅁㅁㅁ
    3번째 비트      2번째 비트
    */
    int[] MarchingSquare =
    {
        0,    // 0000   없다
        1,    // 0001   좌하
        2,    // 0010   우하
        3,    // 0011   우하, 좌하
        4,    // 0100   우상
        5,    // 0101   우상, 좌하
        6,    // 0110   우상, 우하
        7,    // 0111   우상, 우하, 좌하
        8,    // 1000   좌상
        9,    // 1001   좌상, 좌하
        10,    // 1010  좌상, 우하
        11,    // 1011  좌상, 우하, 좌하
        12,    // 1100  좌상, 우상
        13,    // 1101  좌상, 우상, 좌하
        14,    // 1110  좌상, 우상, 우하
        15,    // 1111  좌상, 우상, 우하, 좌하
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
