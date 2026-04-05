using System;
using System.Linq;
using UnityEngine;

public class UtilitySystem : MonoBehaviour
{

    Enemy enemy;

    public EnemyStates GetBestState()
    {

        EnemyStates bestState = EnemyStates.Wandering;

        float bestScore = float.MinValue;

        foreach (EnemyStates state in Enum.GetValues(typeof(EnemyStates)))
        {
            if (!enemy.fsm.GetAllStateNames().Contains(state.ToString())) // Esto es caro de pelotas, pero lo necesito hasta que no defina los estados principales de los enemigos.
                continue;
            float currentScore = enemy.GetScore(state);

            if (currentScore > bestScore)
            {
                bestScore = currentScore;
                bestState = state;
            }
        }

        return bestState;
    }

    // Esto es por si quiero probar con estados aleatorios, es probabilidad acumulada.
    public EnemyStates GetRandomizedState()
    {
        EnemyStates[] states = (EnemyStates[])Enum.GetValues(typeof(EnemyStates));
        float totalScore = 0f;

        foreach (EnemyStates state in states)
        {
            if (!enemy.fsm.GetAllStateNames().Contains(state.ToString())) // Esto es caro de pelotas, pero lo necesito hasta que no defina los estados principales de los enemigos.
                continue;
            totalScore += Mathf.Max(0f, enemy.GetScore(state));
        }

        if (totalScore <= 0f)
            return EnemyStates.Wandering;

        float randomValue = UnityEngine.Random.value * totalScore;
        float accumulatedScore = 0f;

        foreach (EnemyStates state in states)
        {
            if (!enemy.fsm.GetAllStateNames().Contains(state.ToString())) // Esto es caro de pelotas, pero lo necesito hasta que no defina los estados principales de los enemigos.
                continue;
            accumulatedScore += Mathf.Max(0f, enemy.GetScore(state));

            if (randomValue <= accumulatedScore)
                return state;
        }

        return states[states.Length - 1];
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
