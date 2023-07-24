using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerAttack : MonoBehaviour
{
    private GameObject hand;
    [SerializeField]
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        hand = playerController.hand;
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
    }

    void Attack()
    {
        Animator handAnimator = hand.GetComponent<Animator>();
        if (playerController.playerInput.clicked)
        {
            handAnimator.SetTrigger("Attack");
            playerController.playerInput.clicked = false;
        }
    }
}
