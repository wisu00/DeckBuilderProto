using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnGameObjects : MonoBehaviour
{
    public Transform canvas;
    public GameObject InfluenceBar;
    public float InfluenceBarPosX;
    public float InfluenceBarPosY;
    // Start is called before the first frame update
    void Start()
    {
        Vector2 InfluenceBarPosition = new Vector2(InfluenceBarPosX,InfluenceBarPosY);
        GameObject IBar = PhotonNetwork.Instantiate(InfluenceBar.name, InfluenceBarPosition, Quaternion.identity);
        IBar.transform.SetParent(canvas);
        IBar.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
        IBar.GetComponent<RectTransform>().position = InfluenceBarPosition;
    }
}
