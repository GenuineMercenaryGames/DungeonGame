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

                { "loc_start", "Start" },
                { "loc_continue", "Continue" },

                { "loc_yes", "Yes" },
                { "loc_no", "No" },

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

                { "loc_language", "Language" },
                { "loc_graphics", "Graphics" },
                { "loc_audio", "Audio" },
                { "loc_language_settings", "Language Settings" },
                { "loc_graphics_settings", "Graphics Settings" },
                { "loc_audio_settings", "Audio Settings" },

                { "loc_very_low", "Very Low" },
                { "loc_low", "Low" },
                { "loc_medium", "Medium" },
                { "loc_high", "High" },
                { "loc_very_high", "Very High" },
                { "loc_ultra", "Ultra" },

                { "loc_tutorial", "Tutorial" },
                { "loc_loadout", "Loadout" },

                { "loc_control_scheme", "Control Scheme" },
                { "loc_control", "Control" },

                { "loc_paused", "-Paused" },
                { "loc_resume", "Resume" },
                { "loc_restart", "Restart" },
                { "loc_quit_to_menu", "Quit to Menu" },
                { "loc_quit_to_desktop", "Quit to Desktop" },

                { "loc_victory", "Victory" },
                { "loc_defeat", "Defeat" },
                { "loc_victory2", "You have successfully fulfilled your contract" },
                { "loc_defeat2", "You have failed to fulfill the contract" },

                { "loc_popup_quit", "Are you sure you want to quit the program?" },
                { "loc_popup_tutorial", "If this is your first time playing The Sweeper, we highly recommend you undergo some basic training and check out the tutorial!" },
                { "loc_achievements_placeholder", "You haven't earned any achievements yet..." },
            }
        },
        {
            Language.Spanish, new()
            {
                { "loc_language_name", "Español" },

                { "loc_start", "Comenzar" },
                { "loc_continue", "Continuar" },

                { "loc_yes", "Si" },
                { "loc_no", "No" },

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

                { "loc_language", "Idioma" },
                { "loc_graphics", "Gráficos" },
                { "loc_audio", "Audio" },
                { "loc_language_settings", "Ajustes de Idioma" },
                { "loc_graphics_settings", "Ajustes de Gráficos" },
                { "loc_audio_settings", "Ajustes de Audio" },

                { "loc_very_low", "Muy Bajo" },
                { "loc_low", "Bajo" },
                { "loc_medium", "Medio" },
                { "loc_high", "Alto" },
                { "loc_very_high", "Muy Alto" },
                { "loc_ultra", "Ultra" },

                { "loc_tutorial", "Tutorial" },
                { "loc_loadout", "Equipamiento" },

                { "loc_control_scheme", "Esquema de controles" },
                { "loc_control", "Control" },

                { "loc_paused", "-Pausado" },
                { "loc_resume", "Reanudar" },
                { "loc_restart", "Reiniciar" },
                { "loc_quit_to_menu", "Salir al Menú" },
                { "loc_quit_to_desktop", "Salir al Escritorio" },

                { "loc_victory", "Victoria" },
                { "loc_defeat", "Derrota" },
                { "loc_victory2", "Has cumplido tu contrato con éxito" },
                { "loc_defeat2", "No has logrado completar el contrato" },

                { "loc_popup_quit", "¿Estás seguro de que quieres salir del programa?" },
                { "loc_popup_tutorial", "¡Si esta es tu primera vez jugando a The Sweeper, recomendamos que tomes un entrenamiento básico y le eches un vistazo al tutorial!" },
                { "loc_achievements_placeholder", "Todavía no has conseguido ningún logro..." },
            }
        },
        {
            Language.French, new()
            {
                { "loc_language_name", "Français" },

                { "loc_start", "Commencer" },
                { "loc_continue", "Continuer" },

                { "loc_yes", "Oui" },
                { "loc_no", "Non" },

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

                { "loc_language", "Langue" },
                { "loc_graphics", "Graphique" },
                { "loc_audio", "Audio" },
                { "loc_language_settings", "Paramètres de langue" },
                { "loc_graphics_settings", "Paramètres Graphiques" },
                { "loc_audio_settings", "Paramètres Audio" },

                { "loc_very_low", "Très Faible" },
                { "loc_low", "Faible" },
                { "loc_medium", "Moyen" },
                { "loc_high", "Élevé" },
                { "loc_very_high", "Très Élevé" },
                { "loc_ultra", "Ultra" },

                { "loc_tutorial", "Tutoriel" },
                { "loc_loadout", "Équipement" },

                { "loc_control_scheme", "Schéma de contrôle" },
                { "loc_control", "Contrôle" },

                { "loc_paused", "-Pause" },
                { "loc_resume", "Reprendre" },
                { "loc_restart", "Redémarrer" },
                { "loc_quit_to_menu", "Retour au Menu" },
                { "loc_quit_to_desktop", "Retour au Bureau" },

                { "loc_victory", "Victoire" },
                { "loc_defeat", "Défaite" },
                { "loc_victory2", "Vous avez rempli votre contrat avec succès" },
                { "loc_defeat2", "Vous n'avez pas rempli votre contrat" },

                { "loc_popup_quit", "Êtes-vous sûr de vouloir quitter le programme?" },
                { "loc_popup_tutorial", "Si c'est la première fois que vous jouez à The Sweeper, nous vous recommandons fortement de suivre une formation de base et de consulter le tutoriel!" },
                { "loc_achievements_placeholder", "Vous n'avez encore débloqué aucun succès..." },
            }
        },
        {
            Language.German, new()
            {
                { "loc_language_name", "Deutsche" },

                { "loc_start", "Starten" },
                { "loc_continue", "Weitermachen" },

                { "loc_yes", "Ja" },
                { "loc_no", "Nein" },

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

                { "loc_language", "Sprache" },
                { "loc_graphics", "Grafik" },
                { "loc_audio", "Audio" },
                { "loc_language_settings", "Spracheinstellungen" },
                { "loc_graphics_settings", "Grafikeinstellungen" },
                { "loc_audio_settings", "Audioeinstellungen" },

                { "loc_very_low", "Sehr niedrig" },
                { "loc_low", "Niedrig" },
                { "loc_medium", "Mittel" },
                { "loc_high", "Hoch" },
                { "loc_very_high", "Sehr hoch" },
                { "loc_ultra", "Ultra" },

                { "loc_tutorial", "Tutorial" },
                { "loc_loadout", "Ausrüstung" },

                { "loc_control_scheme", "Steuerungsschema" },
                { "loc_control", "Steuerung" },

                { "loc_paused", "-Pausiert" },
                { "loc_resume", "Fortsetzen" },
                { "loc_restart", "Neustart" },
                { "loc_quit_to_menu", "Zum Menü zurückkehren" },
                { "loc_quit_to_desktop", "Zum Desktop zurückkehren" },

                { "loc_victory", "Sieg" },
                { "loc_defeat", "Niederlage" },
                { "loc_victory2", "Sie haben Ihren Vertrag erfolgreich erfüllt" },
                { "loc_defeat2", "Sie konnten den Vertrag nicht erfüllen" },

                { "loc_popup_quit", "Sind Sie sicher, dass Sie das Programm abbrechen möchten?" },
                { "loc_popup_tutorial", "Wenn Sie The Sweeper zum ersten Mal spielen, empfehlen wir Ihnen dringend, ein grundlegendes Training zu absolvieren und sich das Tutorial anzusehen!" },
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
