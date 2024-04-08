using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelaunayTriangulation
{
    public void Triangulation()
    {
        //점 하나로 시작
        // 1.모든 점을 검색하여 가장 가까운 점 두개를 찾는다
        //2.점 두개와 시작점으로 cell을 만든다
        //3.다른 점이 cell이 있는지 확인하고 없으면 2번은 반복한다
        //남은 점이 없을때까지 반복한다

        //점, 거리 저장
        Dictionary<Vector2, float> distanceDic;
        //기준 점 루프
        /*foreach (var edgeData1 in marchingSquares.EdgeDatas)
        {
            distanceDic = new Dictionary<Vector2, float>();
            //상대 점 루프
            foreach (var edgeData2 in marchingSquares.EdgeDatas)
            {
                //같은 점이면 continue;
                if (edgeData1.start == edgeData2.start)
                    continue;
                //거리 계산
                float distance = Vector2.Distance(edgeData1.start, edgeData2.start);
                //distanceDic가 비어있으면 집어넣는다, 안의 값보다 작으면 대체한다
                if (distanceDic.Count == 0)
                    distanceDic.Add(edgeData1.start, distance);
            }
            //점의 cell이 이미 있는지 확인
            //IsIncludedCell();
        }*/

    }
}
