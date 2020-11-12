using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.WSA;

public class PlayerMove : MonoBehaviour
{
    public GameManager gameManager;
    //오디오 관리
    public AudioClip audioJump;
    public AudioClip audioAttack;
    public AudioClip audioDamaged;
    public AudioClip audioItem;
    public AudioClip audioDie;
    public AudioClip audioFinish;
    public float maxSpeed;
    public float jumpPower;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    CapsuleCollider2D CapsuleCollider;
    AudioSource audioSource;




    public int Score = 0;
    public float maxTime = 30;

  

    void Awake()
    {
        //뭐든지 사용하기 전에 겟 컴퍼넌트로 초기화값 가져와 초기화하기.
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        CapsuleCollider = GetComponent<CapsuleCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    

    void Update()// 단발 입력은 업데이트가 좋음
    {

        //점프
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumping"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
            PlaySound("JUMP");
            audioSource.clip = audioJump;
            audioSource.Play();

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
                audioSource.clip = audioAttack;
                audioSource.Play();
                //PlaySound("ATTACK");
            }
            else
            OnDamaged(collision.transform.position);
            audioSource.clip = audioDamaged;
            audioSource.Play();
            //PlaySound("DAMAGED");
        }
           
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            Debug.Log("동전");
            gameManager.stagePoint += 100;
            collision.gameObject.SetActive(false);
            audioSource.clip = audioItem;
            audioSource.Play();

            //PlaySound("ITEM");

        }
        else if (collision.gameObject.tag == "Flag")
        {
            //Next Stage!
            gameManager.NextStage();
            audioSource.clip = audioFinish;
            audioSource.Play();

            //PlaySound("FINISH");


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
        //목숨깍기
        gameManager.HealthDown();
        //반투명 레이어로 바꾸기
        gameObject.layer = 11;
        //반투명 색깔로 바꾸기
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        //몬스터에게 피해 입을때 튕기는 것.
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

    public void OnDie()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        spriteRenderer.flipY = true;

        CapsuleCollider.enabled = false;

        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        audioSource.clip = audioDie;
        audioSource.Play();
        //PlaySound("DIE");
    }

    void deleteHealth()
    {
        maxTime--;
    }

    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }


    void PlaySound(string action)
    {
        switch (action)
        {
            case "JUMP":
                audioSource.clip = audioJump;
                break;
            case "DAMAGED":
                audioSource.clip = audioDamaged;
                break;
            case "ATTACK":
                audioSource.clip = audioAttack;
                break;
            case "DIE":
                audioSource.clip = audioDie;
                break;
            case "FINISH":
                audioSource.clip = audioFinish;
                break;
            case "ITEM":
                audioSource.clip = audioItem;
                break;
        }

    }
}
