using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTrap : MonoBehaviour
{
    private Rigidbody[] rigid;
    [SerializeField] private GameObject go_Meat;

    [SerializeField] private int damage;

    private bool isActivated = false; // false일 때 트랩 가동

    private AudioSource theAudio;
    [SerializeField] private AudioClip sound_Activate;

    void Start()
    {
        rigid = GetComponentsInChildren<Rigidbody>();
        theAudio = GetComponent<AudioSource>();
    }

	private void OnTriggerEnter(Collider other)
	{
        if (!isActivated)
        {
            if (other.transform.tag != "Untagged") // 자기자신과 지형이 아니면
            {
                isActivated = true;
                theAudio.clip = sound_Activate;
                theAudio.Play();
                Destroy(go_Meat);

                for (int i = 0; i < rigid.Length; i++)
                {
                    rigid[i].useGravity = true;
                    rigid[i].isKinematic = false;
                }

                if (other.transform.name == "Player")
                {
                    other.transform.GetComponent<StatusController>().DecreaseHP(damage);
                }
            }
        }
	}
}
