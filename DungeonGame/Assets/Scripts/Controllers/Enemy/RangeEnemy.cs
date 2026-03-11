using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;


// Enemigo básico de distancia. Cuando estés en su vista, buscará un punto cercano donde pueda dispararte. Una vez alcanzado el punto, atacará.
// Diagrama de la máquina de estados:
// https://drive.google.com/file/d/14AdsVPKWjIdQPbeXotiFKwIEhfFmiZsU/view?usp=sharing

public class RangeEnemy : Enemy
{

    public float playerSeparationRange = 10.0f;

    protected override StateMachine MainFSM()
    {
        EnemyStateFactory f = new EnemyStateFactory(this);

        var sm = new StateMachine();
        sm.AddState("Wandering", f.CreateWanderFSM(10, 5));
        sm.SetStartState("Wandering");

        sm.AddState("GoReachablePos", f.CreateStateAimPlayer(playerSeparationRange));
        sm.AddState("LookPlayer", f.CreateStateLookPlayer());
        sm.AddState("AttackPlayer", f.CreateStateRangeAttack());
        sm.AddState("Dead", f.CreateStateDeath());

        sm.AddTransition("Wandering", "GoReachablePos",
            transition => DistanceToPlayer() < playerScanningRange);

        sm.AddTransition("GoReachablePos", "Wandering",
            transition => DistanceToPlayer() > playerScanningRange);

        sm.AddTransition("GoReachablePos", "LookPlayer");

        sm.AddTransition("LookPlayer", "AttackPlayer");

        sm.AddTransition("AttackPlayer", "GoReachablePos",
            transition => DistanceToPlayer() > playerAttackRange);

        sm.AddTransitionFromAny("Dead",
            transition => IsDead());



        return sm;
    }


}