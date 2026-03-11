using UnityEngine;
using UnityHFSM;

public class EnemyStateFactory
{
    private Enemy enemy;

    public EnemyStateFactory(Enemy enemy) {  this.enemy = enemy; }

    #region States
    public State CreateStateFollowPlayer()
    {
        return new State(
            onLogic: _ => enemy.MoveTowardsPlayer()
        );
    }
    public State CreateStateMeleeAttack()
    {
        return new State(
            onEnter: _ => enemy.MeleeAttack()
        );
    }
    public State CreateStateRangeAttack()
    {
        return new State(
            onEnter: _ =>
            {
                enemy.LookToPlayer();
                enemy.RangeAttack();
            }
        );
    }
    public State CreateStateLookPlayer()
    {
        return new State(
            onLogic:_ =>
            {
                enemy.SmoothLookToPlayer();
            },
            canExit: _ => enemy.InViewAngle(),
            needsExitTime: true
        );
    }
    public State CreateStateDeath()
    {
        return new State(
            onEnter: _ => Object.Destroy(enemy.gameObject)
        );
    }
    public State CreateStateWait(float time)
    {
        return new State(
            onEnter: _ => enemy.StayStill(),
            canExit: s => s.timer.Elapsed >= time,
            needsExitTime: true
        );
    }
    public State CreateStateGoRandomPoint(float radius)
    {
        return new State(
            onEnter: _ =>
            {
                Vector3 randomPoint = enemy.GetRandomReachablePoint(enemy.transform.position, radius);
                enemy.MoveTowardsPoint(randomPoint);
            },
            canExit: _ => enemy.ReachedDestination(),
            needsExitTime: true
        );
    }

    public State CreateStateAimPlayer(float radius)
    {
        return new State(
            onEnter: _ =>
            {
                Vector3 randomPoint = enemy.GetRandomPlayerClosestReachablePoint(radius);
                enemy.MoveTowardsPoint(randomPoint);
            },
            canExit: _ => enemy.ReachedDestination(),
            needsExitTime: true
        );
    }
    #endregion

    #region FSMs

    public StateMachine CreateWanderFSM(float radius, float waitTime)
    {
        var sm = new StateMachine();

        sm.AddState("Move", CreateStateGoRandomPoint(radius));
        sm.AddState("Wait", CreateStateWait(waitTime));

        sm.AddTransition("Move", "Wait");
        sm.AddTransition("Wait", "Move");

        sm.SetStartState("Wait");
        return sm;
    }

    #endregion


}
