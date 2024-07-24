using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : NetworkBehaviour
{
	#region Public Fields

	static public SpawnManager Instance;

	public List<PlayerController> players = new List<PlayerController>();

	public SpawnParameter[] spawnParameters;

	#endregion

	private List<SpawnerForPlayer> SpawnerForPlayers = new List<SpawnerForPlayer>();

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			GameObject.Destroy(gameObject);
		}
	}

	public void AddPlayer(PlayerController player)
	{
		players.Add(player);
		player.SpawnParameters = (SpawnParameter[])spawnParameters.Clone();
		SpawnerForPlayer spawnerForPlayer = new SpawnerForPlayer();
		spawnerForPlayer.PlayerController = player;
		spawnerForPlayer.SpawnParameters = (SpawnParameter[])spawnParameters.Clone();
		SpawnerForPlayers.Add(spawnerForPlayer);
	}
	public void RemovePlayer(PlayerController player)
	{
		//players.Remove(player);
	}

	[Networked] private TickTimer delay { get; set; }

	public override void FixedUpdateNetwork()
	{
		if (!NetworkManager.Instance.IsConnected) return;

		if (HasStateAuthority)
		{
			//delay = TickTimer.CreateFromSeconds(Runner, 2f);
			//Runner.Spawn(PigPrefab,transform.position, Quaternion.identity);
			foreach (SpawnerForPlayer spawnerForPlayer in SpawnerForPlayers)
			{
				if (spawnerForPlayer.PlayerController == null)
				{
					continue;
				}
				foreach (SpawnParameter spawnParameter in spawnerForPlayer.SpawnParameters)
				{
					if (spawnerForPlayer.PlayerController.transform.position.magnitude <= spawnParameter.Distance)
					{
						Debug.Log(spawnParameter.CurrentAmount);
						if (spawnParameter.delay.ExpiredOrNotRunning(Runner) && spawnParameter.CurrentAmount < spawnParameter.MaxAmount)
						{
							spawnParameter.delay = TickTimer.CreateFromSeconds(Runner, spawnParameter.SpawnTimer);

							Vector3 spawnDir = UnityEngine.Random.onUnitSphere;
							spawnDir.z = 0;

							spawnDir.Normalize();

							// and now use it in your computation for line 42 above!
							Runner.Spawn(spawnParameter.Prefab, spawnerForPlayer.PlayerController.transform.position + spawnDir * spawnParameter.SpawnRadius, Quaternion.identity, inputAuthority: null, (Runner, NO) => SetSpawnParameter(Runner, NO, spawnParameter));
							spawnParameter.CurrentAmount++;
						}
						break;
					}
				}
			}
		}
	}
	private void SetSpawnParameter(NetworkRunner Runner, NetworkObject NO, SpawnParameter spawnParameter)
	{
		NO.gameObject.GetComponent<AIController>().spawnParameter = spawnParameter;
	}
}

public class SpawnerForPlayer
{
	public PlayerController PlayerController;
	public SpawnParameter[] SpawnParameters;
}

[Serializable]
public class SpawnParameter
{
	public GameObject Prefab;
	public float SpawnTimer = 5f;
	public float Distance = 0f;
	public float SpawnRadius = 10f;
	public int MaxAmount = 10;
	public int CurrentAmount = 0;
	[Networked] public TickTimer delay { get; set; }
}