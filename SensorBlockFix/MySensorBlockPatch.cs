using System;
using System.Reflection;
using NLog;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Torch.Managers.PatchManager;
using VRage.Game;
using VRage.Sync;
using VRageMath;

namespace SensorBlockFix
{
    [PatchShim]
    public static class MySensorBlockPatch
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private static readonly FieldInfo FieldMin =
            typeof(MySensorBlock).GetField("m_fieldMin", BindingFlags.NonPublic | BindingFlags.Instance);

        private static readonly FieldInfo FieldMax =
            typeof(MySensorBlock).GetField("m_fieldMax", BindingFlags.NonPublic | BindingFlags.Instance);

        // ReSharper disable once InconsistentNaming
        private static void PatchInit(MySensorBlock __instance)
        {
            var maxRange = __instance.MaxRange;

            bool Validator(Vector3 it) => it.X >= -maxRange && it.X <= maxRange &&
                                          it.Y >= -maxRange && it.Y <= maxRange &&
                                          it.Z >= -maxRange && it.Z <= maxRange;

            ((Sync<Vector3, SyncDirection.BothWays>) FieldMin.GetValue(__instance)).Validate ??= Validator;
            ((Sync<Vector3, SyncDirection.BothWays>) FieldMax.GetValue(__instance)).Validate ??= Validator;
        }

        public static void Patch(PatchContext patchContext)
        {
            var init = typeof(MySensorBlock).GetMethod(
                "Init",
                new[] {typeof(MyObjectBuilder_CubeBlock), typeof(MyCubeGrid)}
            );
            patchContext.GetPattern(init).Suffixes.Add(((Action<MySensorBlock>) PatchInit).Method);
            Log.Info("Patching Successful!");
        }
    }
}