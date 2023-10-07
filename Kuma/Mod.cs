using BF.File.Emulator.Interfaces;
using PAK.Stream.Emulator.Interfaces;
using Kuma.Template;
using Reloaded.Hooks.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using Reloaded.Mod.Interfaces.Internal;

namespace Kuma
{
    /// <summary>
    /// Your mod logic goes here.
    /// </summary>
    public class Mod : ModBase // <= Do not Remove.
    {
        /// <summary>
        /// Provides access to the mod loader API.
        /// </summary>
        private readonly IModLoader _modLoader;

        /// <summary>
        /// Provides access to the Reloaded.Hooks API.
        /// </summary>
        /// <remarks>This is null if you remove dependency on Reloaded.SharedLib.Hooks in your mod.</remarks>
        private readonly IReloadedHooks? _hooks;

        /// <summary>
        /// Provides access to the Reloaded logger.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Entry point into the mod, instance that created this class.
        /// </summary>
        private readonly IMod _owner;

        /// <summary>
        /// The configuration of the currently executing mod.
        /// </summary>
        private readonly IModConfig _modConfig;

        public Mod(ModContext context)
        {
            _modLoader = context.ModLoader;
            _hooks = context.Hooks;
            _logger = context.Logger;
            _owner = context.Owner;
            _modConfig = context.ModConfig;

            var bfEmulatorController = _modLoader.GetController<IBfEmulator>();
            if (bfEmulatorController == null || !bfEmulatorController.TryGetTarget(out _bfEmulator!))
            {
                _logger.WriteLine($"Unable to get controller for BF Emulator, stuff won't work :(", System.Drawing.Color.Red);
                return;
            }

            var PakEmulatorController = _modLoader.GetController<IPakEmulator>();
            if (PakEmulatorController == null || !PakEmulatorController.TryGetTarget(out _pakEmulator!))
            {
                _logger.WriteLine($"Unable to get controller for PAK Emulator, stuff won't work :(", System.Drawing.Color.Red);
                return;
            }

            var modDir = _modLoader.GetDirectoryForModId(_modConfig.ModId);

            // Check for the mod
            var mods = _modLoader.GetActiveMods();
            //Detailed Descriptions
            if (mods.Any(x => x.Generic.ModId == "p4g64.init.detaileddescriptions.rudiger__gb"))
            {
                _pakEmulator.AddDirectory(Path.Combine(modDir, "PAK", "Detailed"));
            }
            else
            {
                _pakEmulator.AddDirectory(Path.Combine(modDir, "PAK", "No Detailed"));
            }
            //Polished Personamations
            if (mods.Any(x => x.Generic.ModId == "p4gpc.polishedpersonamations"))
            {
                _pakEmulator.AddDirectory(Path.Combine(modDir, "PAK", "Polished"));
            }
            else
            {
                _pakEmulator.AddDirectory(Path.Combine(modDir, "PAK", "No Polished"));
            }
            //Dungeon Text Fixes
            if (mods.Any(x => x.Generic.ModId == "p4g.script.dungeon_text_fixes_JP.rudiger__gb"))
            {
                _bfEmulator.AddDirectory(Path.Combine(modDir, "BF", "Dungeon"));
            }
            else
            {
                _bfEmulator.AddDirectory(Path.Combine(modDir, "BF", "No Dungeon"));
            }
            //Better Vending Machines 64
            if (mods.Any(x => x.Generic.ModId == "p4g64.v.vending"))
            {
                _bfEmulator.AddDirectory(Path.Combine(modDir, "BF", "Vending"));
            }
            else
            {
                _bfEmulator.AddDirectory(Path.Combine(modDir, "BF", "No Vending"));
            }

            // In case it's loaded after
            _modLoader.ModLoaded += ModLoaded;

            // For more information about this template, please see
            // https://reloaded-project.github.io/Reloaded-II/ModTemplate/

            // If you want to implement e.g. unload support in your mod,
            // and some other neat features, override the methods in ModBase.

            // TODO: Implement some mod logic
        }

        private void ModLoaded(IModV1 mod, IModConfigV1 modConfig)
        {
            //Detailed Descriptions
            if (modConfig.ModId == "p4g64.init.detaileddescriptions.rudiger__gb")
            {
                var modDir = _modLoader.GetDirectoryForModId(_modConfig.ModId);
                _pakEmulator.AddDirectory(Path.Combine(modDir, "PAK", "Detailed"));
            }
            //Polished Personamations
            if (modConfig.ModId == "p4gpc.polishedpersonamations")
            {
                var modDir = _modLoader.GetDirectoryForModId(_modConfig.ModId);
                _pakEmulator.AddDirectory(Path.Combine(modDir, "PAK", "Polished"));
            }
            //Dungeon Text Fixes
            if (modConfig.ModId == "p4g.script.dungeon_text_fixes_JP.rudiger__gb")
            {
                var modDir = _modLoader.GetDirectoryForModId(_modConfig.ModId);
                _bfEmulator.AddDirectory(Path.Combine(modDir, "BF", "Dungeon"));
            }
            //Better Vending Machines 64
            if (modConfig.ModId == "p4g64.v.vending")
            {
                var modDir = _modLoader.GetDirectoryForModId(_modConfig.ModId);
                _bfEmulator.AddDirectory(Path.Combine(modDir, "BF", "Vending"));
            }
        }

        #region For Exports, Serialization etc.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Mod() { }
#pragma warning restore CS8618
        #endregion
    }
}