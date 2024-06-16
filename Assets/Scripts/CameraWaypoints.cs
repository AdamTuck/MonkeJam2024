using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CameraWaypoints : MonoBehaviour
{
    [SerializeField] private Image[] waypointImgs;
    [SerializeField] private Transform[] targets;
    [SerializeField] private TextMeshProUGUI[] meterTxts;

    void Update()
    {
        PositionWaypoints();
    }

    private void PositionWaypoints()
    {
        for (int i = 0; i < waypointImgs.Length; i++)
        {
            float minX = waypointImgs[i].GetPixelAdjustedRect().width / 2;
            float maxX = Screen.width - minX;

            float minY = waypointImgs[i].GetPixelAdjustedRect().height / 2;
            float maxY = Screen.width - minY;

            Vector2 pos = Camera.main.WorldToScreenPoint(targets[i].position);

            if (Vector3.Dot((targets[i].position - transform.position), transform.forward) < 0)
            {
                if (pos.x < Screen.width / 2)
                {
                    pos.x = maxX;
                }
                else
                {
                    pos.x = minX;
                }
            }

            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);

            waypointImgs[i].transform.position = pos;
            meterTxts[i].text = ((int)Vector3.Distance(targets[i].position, transform.position)).ToString() + "m";
        }
    }
}
