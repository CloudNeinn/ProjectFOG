using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightCrawlerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private NightCrawlerBoss nightCrawlerBoss;
    // Start is called before the first frame update
    void Start()
    {
        nightCrawlerBoss = transform.parent.GetComponent<NightCrawlerBoss>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Mathf.Abs(nightCrawlerBoss._enemyrb.velocity.x) >= 0.25f && nightCrawlerBoss.isGrounded() && nightCrawlerBoss._enemyrb.velocity.y < 0.1f) animator.SetBool("isMoving", true);
        else animator.SetBool("isMoving", false);
        if(nightCrawlerBoss._enemyrb.velocity.y >= 0.1f) animator.SetBool("isJumping", true);
        else animator.SetBool("isJumping", false);
        if(!nightCrawlerBoss.eneHeaBlo.isAlive) animator.SetBool("isDead", true);
    }
}
