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

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			GameObject.Destroy(Instance.gameObject);
		}
	}

	public void AddPlayer(PlayerController player)
    {
		players.Add(player);
		player.SpawnParameters = spawnParameters;

	}
	public void RemovePlayer(PlayerController player)
	{
		players.Remove(player);
	}

	[Networked] private TickTimer delay { get; set; }

	public override void FixedUpdateNetwork()
	{
		if (!NetworkManager.Instance.IsConnected) return;

		if (GetInput(out NetworkInputData data))
		{
			if (HasStateAuthority)
			{
				//delay = TickTimer.CreateFromSeconds(Runner, 2f);
				//Runner.Spawn(PigPrefab,transform.position, Quaternion.identity);
				foreach(PlayerController player in players)
				{
                    foreach (SpawnParameter spawnParameter in player.SpawnParameters)
					{
						if (player.transform.position.magnitude <= spawnParameter.Distance)
                        {
                            if (spawnParameter.delay.ExpiredOrNotRunning(Runner) && spawnParameter.CurrentAmount < spawnParameter.MaxAmount)
                            {
								spawnParameter.delay = TickTimer.CreateFromSeconds(Runner, spawnParameter.SpawnTimer);

								Vector3 spawnDir = UnityEngine.Random.onUnitSphere;
								spawnDir.z = 0;

								spawnDir.Normalize();

								// and now use it in your computation for line 42 above!
								Runner.Spawn(spawnParameter.Prefab, transform.position + spawnDir* spawnParameter.SpawnRadius, Quaternion.identity);
								spawnParameter.CurrentAmount++;
							}
							return;
						}
					}
                }
			}
		}
	}
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