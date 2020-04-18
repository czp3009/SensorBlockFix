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
            var fieldMin = (Sync<Vector3, SyncDirection.BothWays>) FieldMin.GetValue(__instance);
            // ReSharper disable once ConvertIfStatementToNullCoalescingAssignment
            if (fieldMin.Validate == null)
            {
                fieldMin.Validate = it => it.X >= -maxRange && it.Y >= -maxRange && it.Z >= -maxRange;
            }

            var fieldMax = (Sync<Vector3, SyncDirection.BothWays>) FieldMax.GetValue(__instance);
            // ReSharper disable once ConvertIfStatementToNullCoalescingAssignment
            if (fieldMax.Validate == null)
            {
                fieldMax.Validate = it => it.X <= maxRange && it.Y <= maxRange && it.Z <= maxRange;
            }
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