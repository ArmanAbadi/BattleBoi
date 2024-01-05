using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
	#region Public Fields

	static public GameManager Instance;

	#endregion

	#region Private Fields

	private GameObject instance;

	[Tooltip("The prefab to use for representing the player")]
	[SerializeField]
	private GameObject playerPrefab;
	[Tooltip("The prefab to use for representing the player")]
	[SerializeField]
	private GameObject PigPrefab;
	[Tooltip("The prefab to use for representing the player")]
	[SerializeField]
	private GameObject EvilPigPrefab;
	#endregion

	void Start()
	{
		Instance = this;

		if (!PhotonNetwork.IsConnected)
		{
			return;
		}

		PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);

		for(int i = 0; i < 10; i++)
        {
			PhotonNetwork.Instantiate(this.PigPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
		}
	}
	public override void OnLeftRoom()
	{
	}
	public bool LeaveRoom()
	{
		return PhotonNetwork.LeaveRoom();
	}

	public void QuitApplication()
	{
		Application.Quit();
	}
}
