using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class AchievementManager
{
    public enum Achievement
    {
        StartTheGame,
        Kill5Enemies,
        Kill10Enemies,
        Kill50Enemies,
        Kill100Enemies,
        Kill5EnemiesWhileBelow10HP,
        PickWeapon,
        Count,
    }

    private static bool[] unlockedAchivements = new bool[(int)Achievement.Count];

    public static void SetAchievementsVector(bool[] achievements)
    {
        int minAmount = Mathf.Min(achievements.Length, unlockedAchivements.Length); // Pick the smallest length to iterate (allows both newer and older achievement version files to work out of the box without issue, because anything out of range is truncated or padded)
        for (int i = 0; i < minAmount; ++i)
            unlockedAchivements[i] = achievements[i];
        for (int i = minAmount; i < unlockedAchivements.Length; ++i) // Pad the remaining data if the input vector is smaller (eg: save data from a prior version)
            unlockedAchivements[i] = false;
    }

    public static bool[] GetAchievementsVector()
    {
        return unlockedAchivements.ToArray(); // Make a (quite heavy) copy to prevent modifying the data externally. Ugly fucking hack that should not exist tbh. But it's ok for now.
    }

    public static void UnlockAchievement(Achievement achievement)
    {
        UnlockAchievement((int)achievement);
    }

    public static void UnlockAchievement(int index)
    {
        if (index < 0 || index >= (int)Achievement.Count)
            return;
        if (unlockedAchivements[index])
            return;
        unlockedAchivements[index] = true;
        DisplayAchievementPopUp(index);
    }

    private static void DisplayAchievementPopUp(int index)
    {
        string locName = $"loc_achievement_name_{index}";
        string locDesc = $"loc_achievement_desc_{index}";
        string name = LanguageManager.GetString(locName);
        string desc = LanguageManager.GetString(locDesc);
        Debug.Log($"Unlocked achievement! ({index}, {name}, {desc})"); // Temporary log for debugging purposes.
        // TODO : Implement actual pop up animation display logic here.
        // Also maybe find a way to get achievement icon handling.
    }
}
