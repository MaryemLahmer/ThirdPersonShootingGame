
using JetBrains.Annotations;

public abstract class MovementBaseState
{
   public abstract void EnterState(MovementStateManager movement );
   
   public abstract void UpdateState(MovementStateManager movement, [CanBeNull] AimStateManager aim );
}
