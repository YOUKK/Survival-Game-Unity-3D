using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

// 데이터 직렬화
[System.Serializable]
public class SaveData
{
    public Vector3 playerPos;
    public Vector3 playerRot;

    public List<int> invenArrayNumber = new List<int>();
    public List<string> invenItemName = new List<string>();
    public List<int> invenItemNumber = new List<int>();
}

public class SaveAndLoad : MonoBehaviour
{
    private SaveData saveData = new SaveData();

    private string SAVE_DATA_DIRECTORY;
    private string SAVE_FILENAME = "/SaveFile.txt";

    private PlayerController thePlayer;
    private Inventory theinven;

    void Start()
    {
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Saves/";

		if (!Directory.Exists(SAVE_DATA_DIRECTORY))
		{
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY);
		}
    }

    public void SaveData()
	{
        thePlayer = FindObjectOfType<PlayerController>();
        theinven = FindObjectOfType<Inventory>();

        saveData.playerPos = thePlayer.transform.position;
        saveData.playerRot = thePlayer.transform.eulerAngles;

        Slot[] slots = theinven.GetSlots();
		for (int i = 0; i < slots.Length; i++)
		{
            if(slots[i].item != null)
			{
                saveData.invenArrayNumber.Add(i);
                saveData.invenItemName.Add(slots[i].item.itemName);
                saveData.invenItemNumber.Add(slots[i].itemCount);
			}
		}

        string json = JsonUtility.ToJson(saveData);

        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME, json);

        Debug.Log("저장 완료");
        Debug.Log(json);
	}

    public void LoadData()
	{
        if (File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILENAME))
        {
            string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME);
            saveData = JsonUtility.FromJson<SaveData>(loadJson);

            thePlayer = FindObjectOfType<PlayerController>();
            theinven = FindObjectOfType<Inventory>();

            thePlayer.transform.position = saveData.playerPos;
            thePlayer.transform.eulerAngles = saveData.playerRot;

			for (int i = 0; i < saveData.invenItemName.Count; i++)
			{
                theinven.LoadToInven(saveData.invenArrayNumber[i], saveData.invenItemName[i], saveData.invenItemNumber[i]);
			}

            Debug.Log("로드 완료");
        }
	}
}
