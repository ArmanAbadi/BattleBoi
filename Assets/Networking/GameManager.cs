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
	[Tooltip("The prefab to use for representing the Pig Monster")]
	[SerializeField]
	private GameObject PigPrefab;
	[Tooltip("The prefab to use for representing the Dirto Monster")]
	[SerializeField]
	private GameObject DirtoPrefab;
	[Tooltip("The prefab to use for representing the TreeMan Monster")]
	[SerializeField]
	private GameObject TreeManPrefab;

	#endregion

	void Start()
	{
		Instance = this;

		if (!PhotonNetwork.IsConnected)
		{
			return;
		}

		PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
		Vector2 RandomSpawn;
		for(int i = 0; i < 100; i++)
        {
			RandomSpawn = Random.insideUnitCircle*50;
			GameObject go = PhotonNetwork.Instantiate(this.PigPrefab.name, new Vector3(RandomSpawn.x, RandomSpawn.y, 0f), Quaternion.identity, 0);
			go.transform.parent = transform;
		}
		for (int i = 0; i < 100; i++)
		{
			RandomSpawn = Random.insideUnitCircle * 50;
			GameObject go = PhotonNetwork.Instantiate(this.DirtoPrefab.name, new Vector3(RandomSpawn.x, RandomSpawn.y, 0f), Quaternion.identity, 0);
			go.transform.parent = transform;
		}
		for (int i = 0; i < 50; i++)
		{
			RandomSpawn = Random.insideUnitCircle * 50;
			GameObject go = PhotonNetwork.Instantiate(this.TreeManPrefab.name, new Vector3(RandomSpawn.x, RandomSpawn.y, 0f), Quaternion.identity, 0);
			go.transform.parent = transform;
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
