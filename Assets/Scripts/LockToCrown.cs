using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockToCrown : MonoBehaviour
{
    [SerializeField]
    private GameObject Lock2DPlayer; // 락된 2D 플레이어
    [SerializeField]
    private int crownCount; // 스크립트 상으로 할 수 있지만 모든 오브젝트를 검색해야 하므로 직접입력

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
            // Raycast 정보를 저장할 변수

            if (Physics.Raycast(ray, out hit, 999f, LayerMask.GetMask("Player2D")))
            {
                crownCount++;
                Lock2DPlayer = hit.collider.gameObject;
                Lock2DPlayer.transform.GetChild(0).gameObject.SetActive(true);
            }// Raycast를 통해 충돌 여부 확인
        }
        else if (Input.GetMouseButtonDown(1) && crownCount > 0)
        {
            Debug.Log("조건 허용");
            RayTrace();

            RaycastHit hit;
            // Raycast 정보를 저장할 변수

            if (Physics.Raycast(ray, out hit, 999f, LayerMask.GetMask("Player2D")) && Lock2DPlayer == hit.collider.gameObject)
            {
                Debug.Log("실행");
                crownCount--;
                Lock2DPlayer.transform.GetChild(0).gameObject.SetActive(false);
            }// Raycast를 통해 충돌 여부 확인
        }
    }

    void RayTrace()
    {
        Camera mainCamera = Camera.main;
        // 현재 활성화된 카메라 가져오기

        ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        // 카메라에서 뷰포트 중앙으로 Ray 발사
    }
}
