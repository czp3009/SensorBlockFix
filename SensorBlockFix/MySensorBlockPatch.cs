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

        private static FieldInfo _fieldMin;

        private static FieldInfo _fieldMax;

        // ReSharper disable once InconsistentNaming
        private static void InitPatch(MySensorBlock __instance)
        {
            var maxRange = __instance.MaxRange;
            var fieldMinMin = new Vector3(-maxRange);
            var fieldMinMax = new Vector3(-0.1f);
            var fieldMaxMin = new Vector3(0.1f);
            var fieldMaxMax = new Vector3(maxRange);

            ((Sync<Vector3, SyncDirection.BothWays>) _fieldMin.GetValue(__instance)).Validate ??= vector3 =>
                vector3.IsInsideInclusive(ref fieldMinMin, ref fieldMinMax);
            ((Sync<Vector3, SyncDirection.BothWays>) _fieldMax.GetValue(__instance)).Validate ??= vector3 =>
                vector3.IsInsideInclusive(ref fieldMaxMin, ref fieldMaxMax);
        }

        public static void Patch(PatchContext patchContext)
        {
            try
            {
                var init = typeof(MySensorBlock).GetMethod(
                    "Init",
                    typeof(MyObjectBuilder_CubeBlock), typeof(MyCubeGrid)
                );
                _fieldMin = typeof(MySensorBlock).GetPrivateFieldInfo("m_fieldMin");
                _fieldMax = typeof(MySensorBlock).GetPrivateFieldInfo("m_fieldMax");
                patchContext.GetPattern(init).Suffixes.Add<MySensorBlock>(InitPatch);
                Log.Info("Patching Successful!");
            }
            catch (Exception e)
            {
                Log.Error(e, "Patching failed");
            }
        }
    }
}