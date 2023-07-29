using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Dash : AbilityBase
{
    public float DashSpeed;

    public override void Activate(GameObject parent)
    {
        PlayerStateMachine player = parent.GetComponent<PlayerStateMachine>();
        Vector2 movementInput = player.playerInput.Movement;

        float angleDirection = Mathf.Atan2(movementInput.x, movementInput.y) * Mathf.Rad2Deg;
        angleDirection += player.Character.transform.eulerAngles.y;

        Vector3 movement = Quaternion.Euler(0f, angleDirection, 0f) * Vector3.forward;
        movement *= player.Speed;

        player.Character.SimpleMove(movement * DashSpeed);  
    }
}
