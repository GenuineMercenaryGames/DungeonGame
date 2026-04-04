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

        sm.AddState("Combat", f.CreateMeleeCombatFSM());
        sm.AddState("Flee", f.CreateStateFlee(10));
        sm.AddState("Dead", f.CreateStateDeath());


        sm.AddTransitionFromAny("Dead",
            transition => IsDead());



        return sm;
    }
}