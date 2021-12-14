﻿using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace tsorcRevamp {
    [Label("Config")]
    [BackgroundColor(30, 60, 40, 220)]
    public class tsorcRevampConfig : ModConfig {
        public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref string message) => true;
        public override ConfigScope Mode => ConfigScope.ServerSide;
        [Header("Adventure Mode")]
        [Label("Adventure Mode: Main")]
        [BackgroundColor(60, 140, 80, 192)]
        [Tooltip("Prevents breaking and placing most blocks. \nIt also enables some features intended for the custom map. \n\"If the game lets you break it or place it, it's allowed!\"\nLeave this enabled if you're playing with the custom map!")]
        [DefaultValue(true)]
        public bool AdventureMode { get; set; }

        [Label("Adventure Mode: Recipes and Items")]
        [BackgroundColor(60, 140, 80, 192)]
        [Tooltip("Disables or modifies certain recipes and \ndrops that interfere with the custom map. \nRequires a reload.\nLeave this enabled if you're playing with the custom map!")]
        [ReloadRequired]
        [DefaultValue(true)]
        public bool AdventureModeItems { get; set; }


        [Header("Gameplay Changes")]
        [Label("Souls Drop on Death")]
        [BackgroundColor(60, 140, 80, 192)]
        [Tooltip("Drop all your Dark Souls when you die.\nIf \"Delete Dropped Souls on Death\" is enabled, \nyour Souls will drop after old Souls are deleted.\nDefaults to On")]
        [DefaultValue(true)]
        public bool SoulsDropOnDeath { get; set; }

        [Label("Delete Dropped Souls on Death")]
        [BackgroundColor(60, 140, 80, 192)]
        [Tooltip("Any Dark Souls in the world will be deleted when a player dies.\nEven if this option is disabled, your Souls will be deleted \nif over 400 items are active in the world after you die, \nor if you exit the game while your Souls are still on the ground.\nDefaults to On")]
        [DefaultValue(true)]
        public bool DeleteDroppedSoulsOnDeath { get; set; }

        [Label("Legacy Mode")]
        [BackgroundColor(60, 140, 80, 192)]
        [Tooltip("Legacy mode disables new additions from the Revamp team.\nTurn this on if you want to play the original \nStory of Red Cloud experience as it was in tConfig. \nSome changes and improvements will not be disabled. \nRequires a reload. \nDefaults to Off")]
        [DefaultValue(false)]
        [ReloadRequired]
        //todo items must be manually tagged as legacy. make sure we got them all
        //todo before release, set this to constant and comment out the legacy mode block
        public bool LegacyMode { get; set; }

        [Label("Boss Zen")]
        [BackgroundColor(60, 140, 80, 192)]
        [Tooltip("Boss Zen disables enemy spawns while a boss is alive.\nDefaults to On")]
        [DefaultValue(true)]
        public bool BossZenConfig { get; set; }


        [Header("Options")]

        /*
        [Label("Legacy Music")]
        [BackgroundColor(60, 140, 80, 192)]
        [Tooltip("Warning! This music was disabled to protect streamers and youtubers who were having copyright issues with it, despite being fair use. Enable it for the classic experience, but we do not advise streaming or recording while it is active.")]
        [DefaultValue(false)]
        public bool LegacyMusic { get; set; }*/

        [Label("Miakoda Volume")]
        [BackgroundColor(60, 140, 80, 192)]
        [Tooltip("Revamp Miakoda giving you Navi flashbacks?\nThis slider controls Miakoda's volume.\nSet to 0 to disable Miakoda sounds.")]
        [DefaultValue(100)]
        public uint MiakodaVolume { get; set; }

        [Label("Soul Counter X position")]
        [BackgroundColor(60, 140, 80, 192)]
        [SliderColor(224, 165, 56, 128)]
        [Range(0, 3840)]
        [DefaultValue(180)]
        [Tooltip("The X position of the Soul Counter.")]
        public int SoulCounterPosX { get; set; }

        [Label("Soul Counter Y position")]
        [BackgroundColor(60, 140, 80, 192)]
        [SliderColor(224, 165, 56, 128)]
        [Range(0, 2160)]
        [DefaultValue(60)]
        [Tooltip("The Y position of the Soul Counter.")]
        public int SoulCounterPosY { get; set; }


        [Header("[EXPERIMENTAL!]")]

        [Label("Disable Gravity Potion Screen Flip")]
        [BackgroundColor(200, 80, 80, 192)]
        [SliderColor(224, 165, 56, 128)]
        [ReloadRequired]
        [DefaultValue(false)]
        [Tooltip("The screen no longer flips when gravity does." +
            "\nFlipping gravity will invert your aim instead of the whole screen, a less disorienting effect." +
            "\nExperimental because it still has many issues that have not been polished out yet." +
            "\nNotice: Some other mods may draw visuals in the wrong spot while gravity is flipped.")]
        public bool GravityFix { get; set; }

        /*[Label("Disable Inverted Gravity Aim")]
        [BackgroundColor(200, 80, 80, 192)]
        [SliderColor(224, 165, 56, 128)]
        [ReloadRequired]
        [DefaultValue(false)]
        [Tooltip("Flipping gravity no longer inverts your aim." +
            "\nExperimental: Unfinished, not all weapons are supported yet." +
            "\nAlso may be removed as an option entirely, as it might make things way too easy.")]
        public bool GravityNormalAim { get; set; }*/

        /*
        [Label("Auto-Update Adventure Map")]
        [BackgroundColor(60, 140, 80, 192)]
        [Tooltip("Automatically download updates to the adventure map.\nAround 6MB, updates every few weeks.\nAdventure map updates will not affect existing worlds, only newly created ones.")]
        [DefaultValue(true)]
        public bool AutoUpdateMap { get; set; }

        [Label("Auto-Update Music Mod")]
        [BackgroundColor(60, 140, 80, 192)]
        [Tooltip("Automatically download updates to the music mod.\nAround 100MB, updates every few months.")]
        [DefaultValue(true)]
        public bool AutoUpdateMusic { get; set; }*/
    }
}
