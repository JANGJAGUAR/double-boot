using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField]
    private float pressedDepth = 0.1f; // ������ ���� �� ������ ����
    [SerializeField]
    private List<GameObject> Doors;
    [SerializeField]
    private Stage1Camera stage1Camera;

    [SerializeField]
    private GameObject buttonOff;
    [SerializeField]
    private GameObject buttonOn;

    private Vector2 originalPosition; // ������ ���� ��ġ
    private Vector3 originalRotation; // ���� ���� ����
    private List<float> targetRotationYs; // ���� ������ ����Y
    Quaternion quaternionRotation; // ������ ���� 


    void Start()
    {
        originalPosition = transform.localPosition;
        targetRotationYs = new();
        foreach (var door in Doors)
        {
            targetRotationYs.Add(door.transform.rotation.eulerAngles.y - 90f);
        }

        quaternionRotation = Quaternion.Euler(originalRotation.x, originalRotation.y, originalRotation.z);

        buttonOff.SetActive(true);
        buttonOn.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player2D"))
        {
            AudioManager.Instance.PlaySFX("Door");
            for (var i = 0; i < Doors.Count; i++)
            {
                Doors[i].transform.Rotate(originalRotation.x, targetRotationYs[i], originalRotation.z);
            }
            //MovePlatformDown();
            if (stage1Camera != null)
            {
                stage1Camera.OnOpenDoor();
            }
            buttonOff.SetActive(false);
            buttonOn.SetActive(true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player2D"))
        {
            for (var i = 0; i < Doors.Count; i++)
            {
                Doors[i].transform.rotation = quaternionRotation;
            }
            //MovePlatformUp();
            buttonOff.SetActive(true);
            buttonOn.SetActive(false);
        }
    }

    void MovePlatformDown()
    {
        transform.localPosition = new Vector2(originalPosition.x, originalPosition.y - pressedDepth);
    }

    void MovePlatformUp()
    {
        transform.localPosition = originalPosition;
    }
}
