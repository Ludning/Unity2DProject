using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelaunayTriangulation
{
    public void Triangulation()
    {
        //�� �ϳ��� ����
        // 1.��� ���� �˻��Ͽ� ���� ����� �� �ΰ��� ã�´�
        //2.�� �ΰ��� ���������� cell�� �����
        //3.�ٸ� ���� cell�� �ִ��� Ȯ���ϰ� ������ 2���� �ݺ��Ѵ�
        //���� ���� ���������� �ݺ��Ѵ�

        //��, �Ÿ� ����
        Dictionary<Vector2, float> distanceDic;
        //���� �� ����
        /*foreach (var edgeData1 in marchingSquares.EdgeDatas)
        {
            distanceDic = new Dictionary<Vector2, float>();
            //��� �� ����
            foreach (var edgeData2 in marchingSquares.EdgeDatas)
            {
                //���� ���̸� continue;
                if (edgeData1.start == edgeData2.start)
                    continue;
                //�Ÿ� ���
                float distance = Vector2.Distance(edgeData1.start, edgeData2.start);
                //distanceDic�� ��������� ����ִ´�, ���� ������ ������ ��ü�Ѵ�
                if (distanceDic.Count == 0)
                    distanceDic.Add(edgeData1.start, distance);
            }
            //���� cell�� �̹� �ִ��� Ȯ��
            //IsIncludedCell();
        }*/

    }
}
