using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapState : ActionBaseState
{
   public override void EnterState(ActionStateManager actions)
   {
      actions.anim.SetTrigger("SwapWeapon");
      
   }
   public override void UpdateState(ActionStateManager actions)
   {
      
   }
}
