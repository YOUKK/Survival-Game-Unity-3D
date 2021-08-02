using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
	[SerializeField] private GameObject go_BaseUI;
	[SerializeField] private SaveAndLoad theSaveAndLoad;

    void Start()
    {
        
    }

    void Update()
    {
		if (Input.GetKeyDown(KeyCode.P))
		{
			if (!GameManager.isPause)
			{
                CallMenu();
			}
			else
			{
				CloseMenu();
			}
		}
    }

	private void CallMenu()
	{
		GameManager.isPause = true;
		go_BaseUI.SetActive(true);
		Time.timeScale = 0f;
	}

	private void CloseMenu()
	{
		GameManager.isPause = false;
		go_BaseUI.SetActive(false);
		Time.timeScale = 1f;
	}

	public void ClickSave()
	{
		theSaveAndLoad.SaveData();
	}

	public void ClickLoad()
	{
		theSaveAndLoad.LoadData();
	}

	public void ClickExit()
	{
		Application.Quit();
	}
}
