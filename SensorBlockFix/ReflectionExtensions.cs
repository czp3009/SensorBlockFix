using System;
using System.Reflection;
using Torch.Managers.PatchManager;

namespace SensorBlockFix
{
    internal static class ReflectionExtensions
    {
        internal static FieldInfo GetPrivateFieldInfo(this Type type, string fieldName) =>
            type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance) ??
            throw new MissingFieldException(type.Name, fieldName);

        internal static MethodInfo GetMethod(this Type type, string methodName, params Type[] types) =>
            type.GetMethod(methodName, types) ?? throw new MissingMethodException(type.Name, methodName);

        internal static bool Add<T>(this MethodRewritePattern.MethodRewriteSet set, Action<T> method) =>
            set.Add(method.Method);
    }
}