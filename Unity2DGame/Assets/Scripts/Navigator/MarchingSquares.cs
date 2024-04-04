using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class MarchingSquares : MonoBehaviour
{
    /*  {0, 0, 0, 0}���ʺ��� 0��° ��Ʈ
    0��° ��Ʈ      1��° ��Ʈ
           ������������
           ��        ��
           ��        ��
           ��        ��
           ��        ��
           ������������
    3��° ��Ʈ      2��° ��Ʈ
    */
    int[] MarchingSquare =
    {
        0,    // 0000   ����
        1,    // 0001   ����
        2,    // 0010   ����
        3,    // 0011   ����, ����
        4,    // 0100   ���
        5,    // 0101   ���, ����
        6,    // 0110   ���, ����
        7,    // 0111   ���, ����, ����
        8,    // 1000   �»�
        9,    // 1001   �»�, ����
        10,    // 1010  �»�, ����
        11,    // 1011  �»�, ����, ����
        12,    // 1100  �»�, ���
        13,    // 1101  �»�, ���, ����
        14,    // 1110  �»�, ���, ����
        15,    // 1111  �»�, ���, ����, ����
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
