using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.WSA;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    CapsuleCollider2D CapsuleCollider;


    public GameManager gameManager;

    public int Score = 0;

    public float maxTime = 30;
     

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        CapsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    void Update()// 단발 입력은 업데이트가 좋음
    {

        //점프
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumping"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
        } 
           

        if (Input.GetButtonUp("Horizontal"))
        {
             
            // normalized = 왼쪽으로 갈떄 -1, 가만히 있을때 0 오른쪽으로 갈때 1 이렇게 현재 방향과 값을 가진 rigid.velocity를 이용해 방향 단위 알 수 있음.
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }

        //방향 전환
        if(Input.GetButton("Horizontal"))
        spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == 1;


        // 애니메이션
        if (Mathf.Abs(rigid.velocity.x) < 0.3)
        {
            anim.SetBool("isWalking", false);
        }
        else
        {
            anim.SetBool("isWalking", true);
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");

        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        if (rigid.velocity.x > maxSpeed) // 오른쪽 MAX speed
        {
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        }
        else if (rigid.velocity.x < maxSpeed * (-1)) // 왼쪽 MAX speed
        {
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
        }



        if (rigid.velocity.y < 0)
        {
            //점프 이후
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 2, 0));

            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            //rayHit 빔을 쏜것에 맞은 오브젝트의 정보!

            if (rayHit.collider != null)
            {

                if (rayHit.distance < 0.7f)
                {
                    anim.SetBool("isJumping", false);
                }
                // rayHit에 닿는 것이 무엇인지 알려줌!
                /* Debug.Log(rayHit.collider.name);*/
            }
        }
    }



    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y)
            {
                OnAttack(collision.transform);
            }
            else
            OnDamaged(collision.transform.position);
        }
           
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            Debug.Log("동전");
            gameManager.stagePoint += 100;
            collision.gameObject.SetActive(false);

        }
        else if (collision.gameObject.tag == "flag")
        {
            //Next Stage!
            gameManager.NextStage();


        }
    }

    void OnAttack(Transform enemy)
    {
        //점수
        gameManager.stagePoint += 100;


        //플레이어가 밟을 때 살짝 튀는것,\.
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

        //적의 죽음.
        EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();
        enemyMove.OnDamaged();
    }



    private void OnDamaged(Vector2 targetPos)
    {
        gameObject.layer = 11;

        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

       int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
       rigid.AddForce(new Vector2(dirc, 1)*7, ForceMode2D.Impulse);

       deleteHealth();
       Debug.Log(maxTime);

       Invoke("offDamaged", 3);
        
    }

    void offDamaged()
    {
        gameObject.layer = 10;

        spriteRenderer.color = new Color(1, 1, 1, 1);

    }

    void deleteHealth()
    {
        maxTime--;
    }

}
