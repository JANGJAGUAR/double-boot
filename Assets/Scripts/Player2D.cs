using UnityEngine;

public class Player2D : MonoBehaviour
{
    private Screen _currentScreen;
    private Rigidbody _rigidBody;
    private float ySpeed = 0;
    private bool isGrounded = false;
    private float lastGroundedDelay;
    public float maxDelay = 0.2f;


    public float minYSpeed = -4;
    public float jumpSpeed = 1;
    public float maxJumpTime = 1f;
    public int maxJumpLast = 1;
    private int jumpLast = 1;
    private float jumpLapse = 0f;

    private bool isDying = false;

    [SerializeField]
    private float speed = 0.5f;
    [SerializeField]
    private float gravity = 1f;
    [SerializeField]
    private Transform foot;

    private BoxCollider stepBox;

    private Vector3 trueLocalPosition;

    private Vector3 originPos;
    private Screen _originScreen;

    private void Start()
    {
        _currentScreen = transform.parent.parent.GetComponent<Screen>();
        _rigidBody = GetComponent<Rigidbody>();
        if (foot == null)
            foot = transform.Find("Foot");
        //Start��
        originPos = trueLocalPosition = transform.localPosition;
        _originScreen = transform.parent.parent.GetComponent<Screen>();
    }

    private void SetGrounded()
    {
        isGrounded = true;
        lastGroundedDelay = maxDelay;
    }

    private void LateUpdate()
    {
        if (isDying)
        {
            isDying = false;
            return;
        }
        UpdateScreen(ref trueLocalPosition);
        Debug.Log(isGrounded);

        var upscale = 2 * transform.lossyScale.y;
        var rayStart = transform.position + transform.up * upscale; // transform.TransformDirection(transform.position);
        if (Physics.Raycast(rayStart, -transform.up, out var hitInfo, transform.lossyScale.y / 2 + upscale, LayerMask.GetMask("2D")))
        {
            SetGrounded();
            if (hitInfo.collider is BoxCollider box)
            {
                var boxTopY = box.transform.localPosition.y;
                boxTopY += box.transform.localScale.y * box.size.y / 2;
                boxTopY += transform.localScale.y / 2;
                trueLocalPosition.y = boxTopY;
                stepBox = box;
            }
        }
        else
        {
            Debug.DrawLine(rayStart, rayStart + transform.rotation * Vector3.down * (transform.lossyScale.y / 2 + upscale), Color.red);
        }

        if (transform.GetChild(0).gameObject.activeSelf)
            UpdateInput(ref trueLocalPosition);
        if (isGrounded)
        {
            ySpeed = Mathf.Max(0, ySpeed);
            jumpLapse = 0f;
            jumpLast = maxJumpLast;
        }
        else
        {
            ySpeed = Mathf.Max(minYSpeed, ySpeed - gravity * Time.deltaTime);
        }
        trueLocalPosition.y += ySpeed * Time.deltaTime;
        trueLocalPosition.z = 0;
        transform.localPosition = trueLocalPosition;
        if (lastGroundedDelay > 0)
        {
            lastGroundedDelay -= Time.deltaTime;
        }
        else if (stepBox != null &&
            (trueLocalPosition.x + transform.localScale.x / 2 < stepBox.transform.localPosition.x - stepBox.size.x * stepBox.transform.localScale.x / 2 ||
            stepBox.transform.localPosition.x + stepBox.size.x * stepBox.transform.localScale.x / 2 < trueLocalPosition.x - transform.localScale.x / 2 || stepBox.transform.parent != transform.parent))
        {
            isGrounded = false;
            stepBox = null;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        //if (collision.transform.localPosition.y < transform.parent.InverseTransformDirection(foot.position).y)
        //{
        //    isGrounded = true;
        //}
    }

    private void UpdateInput(ref Vector3 localPosition)
    {
        var x = Input.GetAxis("Horizontal2D");
        var y = Input.GetAxis("Vertical2D");

        localPosition += new Vector3(x, 0) * Time.deltaTime * speed;
        if (y > 0 && jumpLapse < maxJumpTime && jumpLast >= 1)
        {
            ySpeed = jumpSpeed;
            jumpLapse += Time.deltaTime;
            isGrounded = false;
            stepBox = null;
        }

        if (jumpLapse > 0 && y <= 0)
        {
            jumpLapse = maxJumpTime;
        }

        if (jumpLapse >= maxJumpTime)
        {
            jumpLapse = 0;
            jumpLast--;
        }
    }

    private void UpdateScreen(ref Vector3 localPosition)
    {
        //(transform.GetChild(0).gameObject.activeSelf)

        if (localPosition.x < -0.5f)
        {
            ChangeScreen(_currentScreen.LeftScreen, Vector3.right, ref localPosition);
        }
        else if (localPosition.x > 0.5f)
        {
            ChangeScreen(_currentScreen.RightScreen, Vector3.left, ref localPosition);
        }

        if (localPosition.y > 0.5f)
        {
            ChangeScreen(_currentScreen.UpScreen, Vector3.down, ref localPosition);
        }
        else if (localPosition.y < -0.5f)
        {
            if (!ChangeScreen(_currentScreen.DownScreen, Vector3.up, ref localPosition))
                SetGrounded();
        }

    }

    private bool ChangeScreen(Screen newScreen, Vector3 addingDirection, ref Vector3 localPosition)
    {
        if (newScreen == null)
        {
            localPosition += addingDirection * 0.01f;
            return false;
        }
        else
        {
            _currentScreen = newScreen;
            transform.parent = _currentScreen.transform.GetChild(0);
            localPosition += addingDirection;
            transform.localRotation = Quaternion.identity;
            return true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trap"))
        {
            isDying = true;
            transform.parent = _originScreen.transform.GetChild(0);
            transform.localPosition = originPos;
            _currentScreen = _originScreen;
            transform.localRotation = Quaternion.identity;
        }

    }
}