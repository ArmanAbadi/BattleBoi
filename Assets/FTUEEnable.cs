using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FTUEEnable : MonoBehaviour
{
    public FTUEKey ftueKey;
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        if (ftueKey == FTUEKey.FirstGame && PlayerPrefs.GetInt(FTUEKey.FirstGame.ToString()) == 0)
        {
            target.SetActive(true);
            PlayerPrefs.GetInt(FTUEKey.FirstGame.ToString(), 1);
        }
        if (ftueKey == FTUEKey.FirstPorkChop && PlayerPrefs.GetInt(FTUEKey.FirstPorkChop.ToString()) == 0)
        {
            PlayerBag.Instance.PorkChopOwned.AddListener(FirstPorkChop);
            PlayerPrefs.GetInt(FTUEKey.FirstPorkChop.ToString(), 1);
        }
    }
    void FirstPorkChop()
    {
        target.SetActive(true);
        PlayerBag.Instance.PorkChopOwned.RemoveListener(FirstPorkChop);
    }
}
public enum FTUEKey
{
    FirstGame,
    FirstPorkChop
}
