using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Summary:
 * 关卡1玩家控制器
 */
public class PlayerControllor_1 : MonoBehaviour
{
    public static PlayerControllor_1 instance;

    public Rigidbody2D rig;

    public Vector2 currentPosition;
    [HideInInspector]
    public Vector2 moveVector, targetMoveVector, currentMoveVector;

    public float targetMoveSpeed, currentMoveSpeed;
    [HideInInspector]
    public float speedAccValue, speedDecValue;  //移动加减速权重
    [HideInInspector]
    public Vector2 forwardVector, currentForwardVector, targetForwardVector;  //最终移动向量/飞行向量/瞄准向量
    [HideInInspector]
    public Vector2 mouseWorldPos, mouseScreenPos;
    [HideInInspector]
    public float aimAngle;
    [HideInInspector]
    public Vector2 aimVector, currentAimVector, targetAimVector;


    public float minMoveSpeed, maxMoveSpeed;
    public float rotateSpeed;

    public GameObject bulletPrefeb;
    public List<GameObject> bullets = new List<GameObject>();
    public GameObject colorMask;
    public float targetEnergy, currentEnergy;

    public bool isGround;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        targetEnergy = 1f;
    }

    private void Update()
    {
        mouseScreenPos = Input.mousePosition;
        mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

        if (Input.GetMouseButtonUp(0))
        {
            Shoot();
        }

        colorMask.GetComponent<SpriteRenderer>().material.SetFloat("_FadeIntensity", Mathf.Lerp (currentEnergy,targetEnergy,0.1f));

        currentPosition = transform.position;
    }

    private void FixedUpdate()
    {
        MoveControllor();
    }
    public void Shoot()
    {
        if(bullets.Count <= 5)
        {
            GameObject bulletTemp = Instantiate(bulletPrefeb, transform.position, Quaternion.identity);
            bulletTemp.GetComponent<BulletControl>().moveVector = (mouseWorldPos - (Vector2)transform.position).normalized;
            bullets.Add(bulletTemp);

            targetEnergy -= 0.02f;
            transform.localScale -= Vector3.one * 0.5f;
           
        }

    }
    //四方移动
    public void MoveControllor()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        rig.velocity = new Vector2(x * currentMoveSpeed, y * currentMoveSpeed);
        currentMoveSpeed = Mathf.Lerp(currentMoveSpeed, targetMoveSpeed, 0.1f);
    }
    //横板移动
    public void MoveControllor_1()
    {
        var raycastAll = Physics2D.OverlapBoxAll(transform.position, new Vector2(0.4f, 1.1f), 0, LayerMask.GetMask("Ground"));
        if (raycastAll.Length > 0)
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        }

        #region  横向运动
        if (isGround)
        {
            float x = Input.GetAxis("Horizontal");
            rig.velocity = new Vector2(x * currentMoveSpeed, 0);
            currentMoveSpeed = Mathf.Lerp(currentMoveSpeed, targetMoveSpeed, 0.1f);
        }

        #endregion

        #region 跳跃

        #endregion
    }

    //鼠标瞄准移动
    public void MoveControllor_2()
    {
        #region  转向
        currentAimVector = transform.up;
        transform.up = Vector2.Lerp(transform.up, mouseWorldPos - (Vector2)transform.position, rotateSpeed * Time.deltaTime);
        #endregion

        #region  合成速度
        Vector2 targetVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        targetForwardVector = targetVector * 0.9f + currentAimVector;

        if (currentMoveVector.magnitude < targetMoveVector.magnitude)
        {
            currentForwardVector = Vector2.Lerp(currentForwardVector, targetForwardVector, 0.1f);
        }
        else
        {
            currentForwardVector = Vector2.Lerp(currentForwardVector, targetForwardVector, 0.02f);
        }

        currentMoveSpeed = Mathf.Lerp(currentMoveSpeed, maxMoveSpeed, 0.002f);
        rig.velocity = currentForwardVector * currentMoveSpeed;
        #endregion
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("bullet"))
        {
            bullets.Remove(collision.gameObject);
            Destroy(collision.gameObject);
            transform.localScale += Vector3.one * 0.5f;
        }
    }
}
