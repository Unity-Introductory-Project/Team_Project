using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumpCharacter : CharacterBase
{

    public override void Start()
    {
        base.Start();
        fullJumpCount = 3;
    }

    public override void Ability()
    {
        Debug.Log("3단 점프 가능");
    }

}

