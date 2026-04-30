using System;
using System.Collections.Generic;
using UnityEngine;

public static class LanguageManager
{
    private static Language _language;
    private static Dictionary<Language, Dictionary<string, string>> _strings = new() // NOTE : This data could potentially be loaded from files instead of hardcoded here.
    {
        {
            Language.English, new()
            {
                { "loc_language_name", "English" },

                { "loc_play", "Play" },
                { "loc_settings", "Settings" },
                { "loc_achievements", "Achievements" },
                { "loc_credits", "Credits" },
                { "loc_quit", "Quit" },
                { "loc_return", "Return" },

                { "loc_primary_weapon", "Primary Weapon" },
                { "loc_secondary_weapon", "Secondary Weapon" },

                { "loc_level_selection", "Level Selection" },
                { "loc_level_the_forest", "The Forest" },

                { "loc_achievements_placeholder", "You haven't earned any achievements yet..." },
            }
        },
        {
            Language.Spanish, new()
            {
                { "loc_language_name", "Español" },

                { "loc_play", "Jugar" },
                { "loc_settings", "Ajustes" },
                { "loc_achievements", "Logros" },
                { "loc_credits", "Créditos" },
                { "loc_quit", "Salir" },
                { "loc_return", "Volver" },

                { "loc_primary_weapon", "Arma Primaria" },
                { "loc_secondary_weapon", "Arma Secundaria" },

                { "loc_level_selection", "Selection de Nivel" },
                { "loc_level_the_forest", "El Bosque" },

                { "loc_achievements_placeholder", "Todavía no has conseguido ningún logro..." },
            }
        },
        {
            Language.French, new()
            {
                { "loc_language_name", "Français" },

                { "loc_play", "Jouer" },
                { "loc_settings", "Paramètres" },
                { "loc_achievements", "Réalisations" },
                { "loc_credits", "Générique" },
                { "loc_quit", "Quitter" },
                { "loc_return", "Retour" },

                { "loc_primary_weapon", "Arme principale" },
                { "loc_secondary_weapon", "Arme secondaire" },

                { "loc_level_selection", "Sélection du niveau" },
                { "loc_level_the_forest", "La Forêt" },

                { "loc_achievements_placeholder", "Vous n'avez encore débloqué aucun succès..." },
            }
        },
        {
            Language.German, new()
            {
                { "loc_language_name", "Deutsche" },

                { "loc_play", "Spielen" },
                { "loc_settings", "Einstellungen" },
                { "loc_achievements", "Erfolge" },
                { "loc_credits", "Credits" },
                { "loc_quit", "Aufhören" },
                { "loc_return", "Zurückkehren" },

                { "loc_primary_weapon", "Primärwaffe" },
                { "loc_secondary_weapon", "Sekundärwaffe" },

                { "loc_level_selection", "Levelauswahl" },
                { "loc_level_the_forest", "Der Wald" },

                { "loc_achievements_placeholder", "Du hast noch keine Erfolge erzielt..." },
            }
        }
    };

    public static string GetString(Language lang, string loc)
    {
        if (loc != null && _strings != null && _strings.ContainsKey(lang) && _strings[lang].ContainsKey(loc))
            return _strings[lang][loc];
        return $"LOC[\"{lang}\"][\"{loc}\"] NOT FOUND"; // placeholder str to identify where things went to shit
    }

    public static string GetString(string loc)
    {
        return GetString(_language, loc);
    }

    public static Language GetLanguage()
    {
        return _language;
    }

    public static void SetLanguage(int id)
    {
        if (id < 0 || id >= (int)Language.COUNT) id = 0; // English by default if the selected id does not exist.
        SetLanguage((Language)id);
    }

    public static void SetLanguage(Language lang)
    {
        _language = lang;
    }

}
