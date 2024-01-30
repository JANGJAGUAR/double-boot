using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockToCrown : MonoBehaviour
{
    [SerializeField]
    private GameObject Lock2DPlayer; // ���� 2D �÷��̾�
    [SerializeField]
    private int crownCount; // ��ũ��Ʈ ������ �� �� ������ ��� ������Ʈ�� �˻��ؾ� �ϹǷ� �����Է�

    private Ray ray;
    private RaycastHit hit;
    public static LockToCrown Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && crownCount < 1)
        {
            RayTrace();

            RaycastHit hit;
            // Raycast ������ ������ ����

            if (Physics.Raycast(ray, out hit, 999f, LayerMask.GetMask("Player2D")))
            {
                crownCount++;
                Lock2DPlayer = hit.collider.gameObject;
                Lock2DPlayer.transform.GetChild(0).gameObject.SetActive(true);
            }// Raycast�� ���� �浹 ���� Ȯ��
        }
        else if (Input.GetMouseButtonDown(1) && crownCount > 0)
        {
            Debug.Log("���� ���");
            RayTrace();

            RaycastHit hit;
            // Raycast ������ ������ ����

            if (Physics.Raycast(ray, out hit, 999f, LayerMask.GetMask("Player2D")) && Lock2DPlayer == hit.collider.gameObject)
            {
                Debug.Log("����");
                crownCount--;
                Lock2DPlayer.transform.GetChild(0).gameObject.SetActive(false);
            }// Raycast�� ���� �浹 ���� Ȯ��
        }
    }

    void RayTrace()
    {
        Camera mainCamera = Camera.main;
        // ���� Ȱ��ȭ�� ī�޶� ��������

        ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        // ī�޶󿡼� ����Ʈ �߾����� Ray �߻�
    }
}
