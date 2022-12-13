using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetWordManager : Photon.MonoBehaviour
{

    [SerializeField] public string version;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(version);
    }


    public void OnConnectedToMaster() {

        PhotonNetwork.JoinOrCreateRoom("unica", new RoomOptions() { MaxPlayers = 50 }, null);

    }


    void OnJoinedRoom()
    {
        
        PhotonNetwork.Instantiate("Cubo",transform.position,transform.rotation,0);

    }
}
