using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

// Enemigo básico de melee. Empezará a perseguirte al cruzar un rango, cuando se acerque demasiado a ti, atacará.
// Diagrama de la máquina de estados:
// https://drive.google.com/file/d/14AdsVPKWjIdQPbeXotiFKwIEhfFmiZsU/view?usp=sharing

public class MeleeEnemy : Enemy
{

    protected override StateMachine MainFSM()
    {

        EnemyStateFactory f = new EnemyStateFactory(this);

        var sm = new StateMachine();
        sm.AddState("Wandering", f.CreateWanderFSM(10, 5));
        sm.SetStartState("Wandering");

        sm.AddState("FollowPlayer", f.CreateStateFollowPlayer());
        sm.AddState("AttackPlayer", f.CreateStateMeleeAttack());
        sm.AddState("Dead", f.CreateStateDeath());

        sm.AddTransition("Wandering", "FollowPlayer",
            transition => DistanceToPlayer() < playerScanningRange);

        sm.AddTransition("FollowPlayer", "Wandering",
            transition => DistanceToPlayer() > playerScanningRange);

        sm.AddTransition("FollowPlayer", "AttackPlayer",
            transition => DistanceToPlayer() < playerAttackRange);

        sm.AddTransition("AttackPlayer", "FollowPlayer",
            transition => DistanceToPlayer() > playerAttackRange);

        sm.AddTransitionFromAny("Dead",
            transition => IsDead());



        return sm;
    }
}