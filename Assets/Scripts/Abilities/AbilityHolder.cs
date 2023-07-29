using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHolder : MonoBehaviour
{
    public PlayerInput InputManager;
    public AbilityBase Ability;
    public AbilitySlot Slot;
    
    private float _cooldownTime;
    private float _activeTime;
    private AbilityState _state = AbilityState.ready;

    public enum AbilitySlot
    {
        slot1,
        slot2,
        slot3
    }
    enum AbilityState
    {
        ready,
        active,
        cooldown
    }

    private void Update()
    {
        switch(_state)
        {
            case AbilityState.ready:
                if (InputManager.Ability[Slot.GetHashCode()])
                {
                    Ability.Activate(gameObject);
                    _state = AbilityState.active;
                    _activeTime = Ability.ActiveTime;
                }
                break;

           case AbilityState.active:
                if (_activeTime > 0)
                {
                    _activeTime -= Time.deltaTime;
                }
                else
                {
                    _state = AbilityState.cooldown;
                    _cooldownTime = Ability.CooldownTime;
                }
                break;

           case AbilityState.cooldown:
                if (_cooldownTime > 0)
                {
                    _cooldownTime -= Time.deltaTime;
                }
                else
                {
                    _state = AbilityState.ready;
                }
                break;
        }

        
    }
}
