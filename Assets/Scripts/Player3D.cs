using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player3D : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Stage1Camera _stage1Camera;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float cursorSpeed;
    [SerializeField] private float jumpPower;

    private CharacterController _characterController;
    private Vector3 _dir;

    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (_stage1Camera != null)
        {
            var rot = transform.eulerAngles;
            rot.x = 0;
            rot.z = 0;
            _mainCamera.transform.rotation = Quaternion.Euler(rot);
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            SceneController.instance.LoadStage("MainMenu");
        }

        if (_stage1Camera != null)
        {
            return;
        }

        // 카메라 회전
        float mouseInputX = Input.GetAxis("Mouse X");
        float mouseInputY = Input.GetAxis("Mouse Y");
        var rotation = _mainCamera.transform.eulerAngles;
        rotation.x -= mouseInputY * cursorSpeed;
        rotation.y += mouseInputX * cursorSpeed;
        _mainCamera.transform.rotation = Quaternion.Euler(rotation);
        transform.rotation = Quaternion.Euler(0, rotation.y, 0); // 플레이어는 위아래로 기울어지지 않음

        // 이동
        var h = Input.GetAxis("Horizontal3D");
        var v = Input.GetAxis("Vertical3D");

        var rotationY = Mathf.Deg2Rad * transform.eulerAngles.y;
        var forward = new Vector3(Mathf.Cos(rotationY), 0, -Mathf.Sin(rotationY)) * moveSpeed;
        var right = Vector3.Cross(forward, Vector3.up);

        _dir.x = 0;
        _dir.z = 0;
        _dir += h * forward + v * right;

        if (_characterController.isGrounded)
        {
            _dir.y = 0;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _dir.y = jumpPower;
            }
        }
        _dir.y += Physics.gravity.y * Time.deltaTime;
        _characterController.Move(_dir * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("ClearArea"))
        {
            Debug.Log("Clear");
            AudioManager.Instance.PlaySFX("Success");
            StartCoroutine(WaitAndMoveScene());
        }
    }

    private IEnumerator WaitAndMoveScene()
    {
        yield return new WaitForSeconds(2f);
        var level = Convert.ToInt32(SceneManager.GetActiveScene().name.Replace("Stage", string.Empty));
        if (level == SceneController.instance.maxLevel)
        {
            SceneController.instance.LoadStage("MainMenu");
        }
        else
        {
            SceneController.instance.LoadStage($"Stage{level + 1}");
        }
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}