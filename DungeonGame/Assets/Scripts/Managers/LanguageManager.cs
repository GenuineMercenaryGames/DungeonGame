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

                { "loc_not_available_in_demo", "Not Available in Demo" },

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
                { "loc_starting_equipment", "Starting Equipment" },

                { "loc_level_selection", "Level Selection" },
                { "loc_level_begin", "Begin Level" },
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

                { "loc_achievement_name_0", "Game On!" },
                { "loc_achievement_desc_0", "Start your first contract." },
                { "loc_achievement_name_1", "Getting started" },
                { "loc_achievement_desc_1", "Kill 5 enemies." },
                { "loc_achievement_name_2", "Double Digits" },
                { "loc_achievement_desc_2", "Kill 10 enemies." },
                { "loc_achievement_name_3", "Junior Sweeper" },
                { "loc_achievement_desc_3", "Kill 50 enemies." },
                { "loc_achievement_name_4", "True Sweeper" },
                { "loc_achievement_desc_4", "Kill 100 enemies." },
                { "loc_achievement_name_5", "Near Death Experience" },
                { "loc_achievement_desc_5", "Kill 5 enemies while your health is at 10HP or less." },
                { "loc_achievement_name_6", "Hardcore" },
                { "loc_achievement_desc_6", "Kill 10 enemies while your health is at 10HP or less." },
                { "loc_achievement_name_7", "Candy Store for Adults!" },
                { "loc_achievement_desc_7", "Pick up a weapon." },

                { "loc_ctrl_movement_title", "Movement" },
                { "loc_ctrl_movement_details", "<b>W A S D</b>  -  Movement\n<b>SHIFT</b>   -  Sprint\n<b>SPACE</b>   -  Dash" },
                { "loc_ctrl_combat_title", "Combat" },
                { "loc_ctrl_combat_details", "<b>LMB</b>  -  Fire Weapon" },
                { "loc_ctrl_camera_title", "Camera" },
                { "loc_ctrl_camera_details", "<b>Mouse Wheel</b>  -  Zoom Camera\n<b>Q</b>  -  Smooth Rotation" },
                { "loc_ctrl_system_title", "System" },
                { "loc_ctrl_system_details", "<b>M</b>  -  Toggle Minimap\n<b>ESC</b>  -  Pause Operation" },

                { "loc_cosmetics", "Attire" },

                { "loc_weapon_name_recharger_pistol", "Recharger Pistol" },
                { "loc_weapon_desc_recharger_pistol", "Cheap, reliable, and endlessly reusable. A low-power sidearm with self-recharging ammo cells." },
                { "loc_weapon_name_dispersion_pistol", "Dispersion Pistol" },
                { "loc_weapon_desc_dispersion_pistol", "Sweeper.co's standard magnum pistol. Hard-hitting plasma rounds with slow recharge and deliberate fire." },
                { "loc_weapon_name_machinegun", "CPP26" },
                { "loc_weapon_desc_machinegun", "The Sweeper favorite. High-capacity submachine gun built for fast, messy cleanup jobs." },
                { "loc_weapon_name_shotgun", "Shotgun" },
                { "loc_weapon_desc_shotgun", "Devastating at close range. Officially illegal in civilized space, unofficially standard Sweeper equipment." },
                { "loc_weapon_name_shrapnelgun", "Shrapnelgun" },
                { "loc_weapon_desc_sharpnelgun", "Fires ricocheting plasma shards that rebound off walls until their energy burns out. Handle with care." },
                { "loc_weapon_name_laser_beam", "Mining Plasma Beam" },
                { "loc_weapon_desc_laser_beam", "Industrial-grade mining extraction beam repurposed for combat. Massive damage at extreme energy cost." },
                { "loc_weapon_name_minigun", "The Sweeper" },
                { "loc_weapon_desc_minigun", "Some problems need precision. Others need six rotating barrels and enough ammo to erase a hallway full of alien scum." },
            }
        },
        {
            Language.Spanish, new()
            {
                { "loc_language_name", "Español" },

                { "loc_not_available_in_demo", "No Disponible en la Demo" },

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
                { "loc_starting_equipment", "Equipamiento Inicial" },

                { "loc_level_selection", "Selection de Nivel" },
                { "loc_level_begin", "Iniciar Nivel" },
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
                { "loc_loadout", "Armamento" },

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

                { "loc_ctrl_movement_title", "Movimiento" },
                { "loc_ctrl_movement_details", "<b>W A S D</b>  -  Movimiento\n<b>SHIFT</b>   -  Esprintar\n<b>SPACE</b>   -  Rodar" },
                { "loc_ctrl_combat_title", "Combate" },
                { "loc_ctrl_combat_details", "<b>Clic Izq.</b>  -  Disparar Arma" },
                { "loc_ctrl_camera_title", "Cámara" },
                { "loc_ctrl_camera_details", "<b>Rueda Ratón</b>  -  Zoom Cámara\n<b>Q</b>  -  Rotación Suave" },
                { "loc_ctrl_system_title", "Sistema" },
                { "loc_ctrl_system_details", "<b>M</b>  -  Mostrar Minimapa\n<b>ESC</b>  -  Pausar" },

                { "loc_cosmetics", "Atuendo" },

                { "loc_weapon_name_recharger_pistol", "Pistola recargable" },
                { "loc_weapon_desc_recharger_pistol", "Barata, fiable e infinitamente reutilizable. Un arma de mano de baja potencia con celdas de munición autorrecargables." },
                { "loc_weapon_name_dispersion_pistol", "Pistola de dispersión" },
                { "loc_weapon_desc_dispersion_pistol", "Pistola magnum estándar de Sweeper.co. Munición de plasma de gran impacto con recarga lenta y fuego preciso." },
                { "loc_weapon_name_machinegun", "CPP26" },
                { "loc_weapon_desc_machinegun", "La favorita de los Sweeper. Subfusil de alta capacidad diseñado para trabajos de limpieza rápidos y sucios." },
                { "loc_weapon_name_shotgun", "Escopeta" },
                { "loc_weapon_desc_shotgun", "Devastadora a corta distancia. Oficialmente ilegal en el espacio civilizado, extraoficialmente equipo estándar de los Sweepers." },
                { "loc_weapon_name_shrapnelgun", "Escopeta de metralla" },
                { "loc_weapon_desc_sharpnelgun", "Dispara fragmentos de plasma que rebotan en las paredes hasta que se agota su energía. Manéjela con cuidado." },
                { "loc_weapon_name_laser_beam", "Rayo de plasma de minería" },
                { "loc_weapon_desc_laser_beam", "Rayo de extracción minera de grado industrial adaptado para el combate. Daño masivo a un coste energético extremo." },
                { "loc_weapon_name_minigun", "La Barredora" },
                { "loc_weapon_desc_minigun", "Algunos problemas requieren precisión. Otros requieren seis cañones giratorios y suficiente munición para arrasar un pasillo lleno de escoria alienígena." },
            }
        },
        {
            Language.French, new()
            {
                { "loc_language_name", "Français" },

                { "loc_not_available_in_demo", "Non Disponible dans la Démo" },

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
                { "loc_starting_equipment", "Équipement de démarrage" },

                { "loc_level_selection", "Sélection du niveau" },
                { "loc_level_begin", "Commencer Niveau" },
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

                { "loc_ctrl_movement_title", "Mouvement" },
                { "loc_ctrl_movement_details", "<b>W A S D</b>  -  Mouvement\n<b>SHIFT</b>   -  Sprint\n<b>SPACE</b>   -  Esquive" },
                { "loc_ctrl_combat_title", "Combat" },
                { "loc_ctrl_combat_details", "<b>Clic Gauche</b>  -  Tirer" },
                { "loc_ctrl_camera_title", "Caméra" },
                { "loc_ctrl_camera_details", "<b>Molette</b>  -  Zoom Caméra\n<b>Q</b>  -  Rotation Fluide" },
                { "loc_ctrl_system_title", "Système" },
                { "loc_ctrl_system_details", "<b>M</b>  -  Afficher la Carte\n<b>ESC</b>  -  Pause" },

                { "loc_cosmetics", "Tenue" },

                { "loc_weapon_name_recharger_pistol", "Pistolet à recharger" },
                { "loc_weapon_desc_recharger_pistol", "Bon marché, fiable et réutilisable à l'infini. Une arme de poing de faible puissance avec des cellules de munitions auto-rechargeables." },
                { "loc_weapon_name_dispersion_pistol", "Pistolet à dispersion" },
                { "loc_weapon_desc_dispersion_pistol", "Le pistolet magnum standard de Sweeper.co. Munitions plasma à fort impact, recharge lente et tir contrôlé." },
                { "loc_weapon_name_machinegun", "CPP26" },
                { "loc_weapon_desc_machinegun", "Le favori de Sweeper. Une mitraillette haute capacité conçue pour les travaux de nettoyage rapides et salissants." },
                { "loc_weapon_name_shotgun", "Fusil à pompe" },
                { "loc_weapon_desc_shotgun", "Dévastateur à courte portée. Officiellement illégal dans l'espace civilisé, équipement standard officieux des nettoyeurs." },
                { "loc_weapon_name_shrapnelgun", "Fusil à shrapnelgun" },
                { "loc_weapon_desc_sharpnelgun", "Tire des éclats de plasma ricochants qui rebondissent sur les murs jusqu'à épuisement de leur énergie. À manipuler avec précaution." },
                { "loc_weapon_name_laser_beam", "Rayon plasma minier" },
                { "loc_weapon_desc_laser_beam", "Rayon d'extraction minière de qualité industrielle reconverti pour le combat. Dégâts massifs pour un coût énergétique extrême." },
                { "loc_weapon_name_minigun", "Le Balayeur" },
                { "loc_weapon_desc_minigun", "Certains problèmes exigent de la précision. D'autres nécessitent six canons rotatifs et suffisamment de munitions pour éradiquer un couloir rempli de vermine extraterrestre." },
            }
        },
        {
            Language.German, new()
            {
                { "loc_language_name", "Deutsch" },

                { "loc_not_available_in_demo", "Nicht in der Demo Verfügbar" },

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
                { "loc_starting_equipment", "Startausrüstung" },

                { "loc_level_selection", "Levelauswahl" },
                { "loc_level_begin", "Starten Level" },
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

                { "loc_ctrl_movement_title", "Bewegung" },
                { "loc_ctrl_movement_details", "<b>W A S D</b>  -  Bewegen\n<b>SHIFT</b>   -  Sprinten\n<b>SPACE</b>   -  Ausweichen" },
                { "loc_ctrl_combat_title", "Kampf" },
                { "loc_ctrl_combat_details", "<b>LMT</b>  -  Waffe abfeuern" },
                { "loc_ctrl_camera_title", "Kamera" },
                { "loc_ctrl_camera_details", "<b>Mausrad</b>  -  Kamerazoom\n<b>Q</b>  -  Sanfte Drehung" },
                { "loc_ctrl_system_title", "System" },
                { "loc_ctrl_system_details", "<b>M</b>  -  Minikarte umschalten\n<b>ESC</b>  -  Pause" },

                { "loc_cosmetics", "Kleidung" },

                { "loc_weapon_name_recharger_pistol", "Aufladepistole" },
                { "loc_weapon_desc_recharger_pistol", "Günstig, zuverlässig und unendlich oft wiederverwendbar. Eine schwache Seitenwaffe mit selbstaufladenden Munitionszellen." },
                { "loc_weapon_name_dispersion_pistol", "Streupistole" },
                { "loc_weapon_desc_dispersion_pistol", "Sweepers Standard-Magnumpistole. Durchschlagskräftige Plasmageschosse mit langsamer Aufladung und gezielter Feuerrate." },
                { "loc_weapon_name_machinegun", "CPP26" },
                { "loc_weapon_desc_machinegun", "Der Favorit von Sweeper. Eine Maschinenpistole mit hoher Magazinkapazität, entwickelt für schnelle und unordentliche Aufräumarbeiten." },
                { "loc_weapon_name_shotgun", "Schrotflinte" },
                { "loc_weapon_desc_shotgun", "Verheerend auf kurze Distanz. Offiziell im zivilisierten Raum verboten, inoffiziell Standardausrüstung der Sweeper." },
                { "loc_weapon_name_shrapnelgun", "Schrapnellgewehr" },
                { "loc_weapon_desc_sharpnelgun", "Verschießt abprallende Plasmasplitter, die von Wänden zurückprallen, bis ihre Energie verbraucht ist. Vorsicht beim Umgang." },
                { "loc_weapon_name_laser_beam", "Bergbau-Plasmastrahl" },
                { "loc_weapon_desc_laser_beam", "Industrieller Bergbau-Extraktionsstrahl, umfunktioniert für den Kampf. Massiver Schaden bei extrem hohem Energieaufwand." },
                { "loc_weapon_name_minigun", "Der Kehrer" },
                { "loc_weapon_desc_minigun", "Manche Probleme erfordern Präzision. Andere brauchen sechs rotierende Läufe und genug Munition, um einen ganzen Flur voller außerirdischer Abschaum auszulöschen." },
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
