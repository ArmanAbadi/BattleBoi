using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviourPunCallbacks
{

	#region Private Serializable Fields

	[Tooltip("Scene to load after connecting")]
	[SerializeField]
	private string SceneToLoad;

	[Tooltip("The maximum number of players per room")]
	[SerializeField]
	private byte maxPlayersPerRoom = 4;

	#endregion

	string gameVersion = "1";

	bool isConnecting = false;

	void Awake()
	{
		PhotonNetwork.AutomaticallySyncScene = true;
	}

	#region Public Methods

	public void Connect()
	{
		if (isConnecting) return;
		isConnecting = true;

		if (PhotonNetwork.IsConnected)
		{
			PhotonNetwork.JoinRandomRoom();
		}
		else
		{
			PhotonNetwork.ConnectUsingSettings();
			PhotonNetwork.GameVersion = this.gameVersion;
		}
	}
    #endregion

    #region MonoBehaviourPunCallbacks CallBacks

    public override void OnConnectedToMaster()
	{
		if (isConnecting)
		{
			PhotonNetwork.JoinRandomRoom();
		}
	}

	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = this.maxPlayersPerRoom });
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		isConnecting = false;
	}

	public override void OnJoinedRoom()
	{
		if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
		{
			PhotonNetwork.LoadLevel(SceneToLoad);
		}
	}

	#endregion
}
