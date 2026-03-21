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
    public State CreateStateFlee(float dist)
    {
        return new State(
            onLogic: _ => enemy.Flee(dist)
        );
    }
    public State CreateStateAttack()
    {
        return new State(
            onLogic: _ =>
            {
                enemy.Attack();
            },
            canExit: _ => enemy.HasFinshedAttack(),
            needsExitTime: true
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
    public State CreateStateDeath(float fallTime = 2f)
    {
        Quaternion startRotation = Quaternion.identity;
        Quaternion targetRotation = Quaternion.identity;
        bool destroyed = false;

        return new State(
            onEnter: _ =>
            {
                enemy.StayStill();
                enemy.healthBarController.HideBar();

                startRotation = enemy.transform.localRotation;
                targetRotation = startRotation * Quaternion.Euler(0.0f, 0.0f, 90.0f);
                destroyed = false;
            },
            onLogic: state =>
            {
                float t = Mathf.Clamp01(state.timer.Elapsed / fallTime); // Recuerda que tengo que usar el state timer más, que se me olvida que existe.
                enemy.transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, t);

                if (!destroyed && state.timer.Elapsed >= fallTime)
                {
                    destroyed = true;
                    Object.Destroy(enemy.gameObject);
                }
            }
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

    public StateMachine CreateMeleeCombatFSM()
    {
        var sm = new StateMachine();

        sm.AddState("FollowPlayer", CreateStateFollowPlayer());
        sm.AddState("Attack", CreateStateAttack());

        sm.AddTransition("FollowPlayer", "Attack", transition => enemy.InAttackRange());
        sm.AddTransition("Attack", "FollowPlayer", transition => !enemy.InAttackRange());

        sm.SetStartState("FollowPlayer");
        return sm;
    }

    public StateMachine CreateRangeCombatFSM(float attackDistance)
    {
        var sm = new StateMachine();

        sm.AddState("GoAimPoint", CreateStateAimPlayer(attackDistance));
        sm.AddState("LookPlayer", CreateStateLookPlayer());
        sm.AddState("Attack", CreateStateAttack());

        sm.AddTransition("GoAimPoint", "LookPlayer");
        sm.AddTransition("LookPlayer", "Attack");
        //sm.AddTransition("Attack", "GoAimPoint", transition => !enemy.InDirectPlayerSight());
        sm.AddTransition("Attack", "LookPlayer");

        sm.SetStartState("GoAimPoint");
        return sm;
    }

    #endregion


}
