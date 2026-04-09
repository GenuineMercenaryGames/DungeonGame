using Assets.Scripts.Generation;
using System;
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
            onLogic: state =>
            {
                enemy.MoveTowardsPlayer();
            }
        );
    }
    public State CreateStateFlee(float dist)
    {
        return new State(
            onLogic: _ => enemy.Flee(dist)
        );
    }
    public HybridStateMachine CreateStateAttack()
    {
        var sm = new HybridStateMachine();

        sm.AddState("AttackBegin", new State(
            onEnter: _ =>
            {
                enemy.attackFinished = false;
                enemy.agent.ResetPath();

                enemy.animator.ResetTrigger("MeleeAttack");
                enemy.animator.SetTrigger("MeleeAttack");
            },
            onLogic: _ =>
            {
                enemy.SmoothLookToPlayer();
            },
            onExit: _ =>
            {
                enemy.animator.ResetTrigger("MeleeAttack");
                enemy.animator.SetTrigger("StopAttack");
            },
            canExit: _ => enemy.HasFinshedAttack(),
            needsExitTime: true
        ));

        sm.AddState("AttackEnd", new State(
            onEnter: _ => {
                enemy.GetComponent<WeaponController>().AttackPressed();
                enemy.GetComponent<WeaponController>().AttackReleased();
            }
        ));

        sm.AddTransition("AttackBegin", "AttackEnd");
        sm.AddTransition("AttackEnd", "AttackBegin", _ => enemy.InAttackRange());
        sm.AddExitTransition("AttackEnd", _ => !enemy.InAttackRange());

        sm.SetStartState("AttackBegin");
        return sm;
    }

    public HybridStateMachine CreateStateSpinShotAttack(float spinDuration, float totalRotation, float shootInterval)
    {
        var sm = new HybridStateMachine();

        float shootTimer = 0f;
        float startY = 0f;

        sm.AddState("AttackBegin", new State(
            onEnter: _ =>
            {
                enemy.attackFinished = false;
                enemy.agent.ResetPath();

                shootTimer = 0f;

                enemy.SmoothLookToPlayer();
                startY = enemy.transform.eulerAngles.y;

                //enemy.animator.ResetTrigger("MeleeAttack");
                //enemy.animator.SetTrigger("MeleeAttack");
            },
            onLogic: state =>
            {
                shootTimer += Time.deltaTime;

                float normalizedTime = Mathf.Clamp01(state.timer.Elapsed / spinDuration);
                float currentY = startY + totalRotation * normalizedTime;

                enemy.transform.rotation = Quaternion.Euler(0f, currentY, 0f);

                if (shootTimer >= shootInterval)
                {
                    shootTimer = 0f;

                    var weaponController = enemy.GetComponent<WeaponController>();
                    weaponController.AttackPressed();
                    weaponController.AttackReleased();
                }
            },
            onExit: _ =>
            {
                //enemy.animator.ResetTrigger("MeleeAttack");
                //enemy.animator.SetTrigger("StopAttack");
            },
            canExit: state => state.timer.Elapsed >= spinDuration,
            needsExitTime: true
        ));

        sm.AddState("AttackEnd", new State(
            onEnter: _ =>
            {
                enemy.attackFinished = true;
            }
        ));

        sm.AddTransition("AttackBegin", "AttackEnd");
        sm.AddTransition("AttackEnd", "AttackBegin", _ => enemy.InAttackRange());
        sm.AddExitTransition("AttackEnd", _ => !enemy.InAttackRange());

        sm.SetStartState("AttackBegin");
        return sm;
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
    public State CreateStateDeath(float fallTime = 5f)
    {
        Quaternion startRotation = Quaternion.identity;
        Quaternion targetRotation = Quaternion.identity;
        bool destroyed = false;

        return new State(
            onEnter: _ =>
            {
                enemy.StayStill();
                enemy.enemyUIController.HideBar();
                VFXManager.Instance.InstantiateVFX("DeathSkull", enemy.transform.position + new Vector3(0,2 * enemy.vfxScale, 0), enemy.vfxScale);

                startRotation = enemy.transform.localRotation;
                targetRotation = startRotation * Quaternion.Euler(0.0f, 0.0f, 90.0f);
                destroyed = false;
            },
            onLogic: state =>
            {
                enemy.animator.SetTrigger("Die");
                enemy.GetComponent<BoxCollider>().enabled = false;
                //float t = Mathf.Clamp01(state.timer.Elapsed / fallTime); // Recuerda que tengo que usar el state timer más, que se me olvida que existe.
                //enemy.transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, t);

                if (!destroyed && state.timer.Elapsed >= fallTime)
                {
                    destroyed = true;
                    VFXManager.Instance.InstantiateVFX("DeathExplosion", enemy.transform.position, enemy.vfxScale);
                    enemy.Die();
                    GameObject.Destroy(enemy.gameObject);
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

    public HybridStateMachine CreateMeleeCombatFSM()
    {

        var sm = new HybridStateMachine(
            
        );

        sm.AddState("Warn", new State(
            onEnter: _ =>
            {
                enemy.StayStill();
                enemy.enemyUIController.ShowWarnSign();
            },
            onLogic: state =>
            {
                enemy.SmoothLookToPlayer();
            },
            canExit: state => state.timer.Elapsed > 0.5f,
            needsExitTime: true
        ));

        sm.AddState("FollowPlayer", CreateStateFollowPlayer());
        sm.AddState("Attack", CreateStateAttack());

        sm.AddTransition("Warn", "FollowPlayer");
        sm.AddTransition("FollowPlayer", "Attack", transition => enemy.InAttackRange());
        sm.AddTransition("Attack", "FollowPlayer", transition => !enemy.InAttackRange());

        sm.SetStartState("Warn");
        return sm;
    }

    public HybridStateMachine CreateBossCombatFSM()
    {
        var sm = new HybridStateMachine();

        sm.AddState("Wait", CreateStateWait(1));
        sm.AddState("Attack", CreateStateAttack());
        sm.AddState("SpinAttack", CreateStateSpinShotAttack(1.2f, 360f, 0.08f));

        sm.AddTransition("Wait", "Attack");
        sm.AddTransition(new TransitionAfter("Attack", "SpinAttack", 3f));
        sm.AddTransition(new TransitionAfter("SpinAttack", "Wait", 1.2f));

        sm.SetStartState("Wait");
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
