using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionController : MonoBehaviour {
    float v;//垂直轴值
    float h;//水平轴值

    /*
     Animator Parameter
     */
    int XDir;//角色X轴移动的方向，1为右，-1为左，0为不移动
    int YDir;//角色Y轴的速度，1为向上，-1为向下，0为不移动
    bool isGround;//角色是否在地面上
    float currentRunSpeedX;//X轴当前移动速度，矢量，右边为正方向

    Animator m_animator;
    [System.NonSerialized]
    public Rigidbody2D m_rigidbody;
    TriggerSensor forwordSensor;
    TriggerSensor backSensor;
    TriggerSensor groundSensor;
    CharacterInfo info;

    public float CurrentRunSpeedX {
        get {
            return currentRunSpeedX;
        }
        set {
            currentRunSpeedX = value < -Info.MaxRunSpeedX ? -Info.MaxRunSpeedX : value > Info.MaxRunSpeedX ? Info.MaxRunSpeedX : value;
        }
    }

    public bool IsForwardOnWall {
        get {
            return forwordSensor.IsTrigger;
        }
    }

    public bool IsBackOnWall {
        get {
            return backSensor.IsTrigger;
        }
    }

    public CharacterInfo Info {
        get {
            return info;
        }

        private set {
            info = value;
        }
    }

    #region MonoBehaviour Event Functions
    void Awake() {
        m_animator = GetComponentInChildren<Animator>();
        m_rigidbody = GetComponent<Rigidbody2D>();
        forwordSensor = transform.Find("ForwordTriggerSensor").GetComponent<TriggerSensor>();
        backSensor = transform.Find("BackTriggerSensor").GetComponent<TriggerSensor>();
        groundSensor = transform.Find("GroundTriggerSensor").GetComponent<TriggerSensor>();
    }

    void Start() {
        Info = Demo2GameStart.playerInfo;
    }

    void Update() {
        //UpdateRigidBodyFreezePositionX();
        UpdateParameterYDir();
        UpdateParameterIsGround();
    }

    void FixedUpdate() {
        m_rigidbody.velocity = new Vector2(CurrentRunSpeedX, m_rigidbody.velocity.y);
    }
    #endregion

    void UpdateRigidBodyFreezePositionX() {
        if (isGround && Mathf.Abs(currentRunSpeedX) <= Vector3.kEpsilon) {
            m_rigidbody.constraints |= RigidbodyConstraints2D.FreezePositionX;
        }
        else {
            m_rigidbody.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
        }
    }

    void UpdateParameterIsGround() {
        isGround = groundSensor.IsTrigger;
        m_animator.SetBool("isGround", isGround);
    }

    void UpdateParameterYDir() {
        YDir = m_rigidbody.velocity.y > 0f ? 1 : m_rigidbody.velocity.y < 0f ? -1 : 0;
        m_animator.SetInteger("YDir", YDir);
    }

    /// <summary>
    /// 移动Update，在该类中不调用，由StateMachineBehaviour调用
    /// </summary>
    public void MoveAction() {
        h = Input.GetAxisRaw("Horizontal");
        if (h < 0f || h > 0) {
            XDir = h < 0 ? -1 : 1;
            transform.localScale = new Vector3(XDir, 1f, 1f);

            CurrentRunSpeedX = Mathf.Lerp(CurrentRunSpeedX, XDir * Info.MaxRunSpeedX, Info.RunAccelerationX * Time.deltaTime * Info.MaxRunSpeedX);
        }
        else {
            XDir = 0;
            if (isGround) {
                CurrentRunSpeedX = Mathf.Lerp(CurrentRunSpeedX, 0f, Info.RunAccelerationX * Time.deltaTime * Info.MaxRunSpeedX);
            }
        }
        m_animator.SetInteger("XDir", XDir);

        if (IsForwardOnWall) {
            CurrentRunSpeedX = 0f;
        }
    }



    public void TujinAction() {
        if (Input.GetKey(KeyCode.LeftShift)) {
            m_animator.SetTrigger("Tujin");
        }
    }

    public void JumpAction() {
        if (Input.GetKey(KeyCode.X)) {
            m_animator.SetTrigger("Jump");
        }
    }
}
