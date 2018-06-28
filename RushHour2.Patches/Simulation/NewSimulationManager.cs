﻿using RushHour2.Core.Reporting;
using RushHour2.Core.Settings;
using RushHour2.Patches.HarmonyLocal;
using System;
using System.Reflection;

namespace RushHour2.Patches.Simulation
{
    public class SimulationManager_Update : Patchable
    {
        public override MethodBase BaseMethod => typeof(SimulationManager).GetMethod("Update", BindingFlags.Instance | BindingFlags.NonPublic, null, Type.EmptyTypes, new ParameterModifier[] { });
        public override MethodInfo Postfix => typeof(SimulationManager_Update).GetMethod(nameof(UpdatePostfix), BindingFlags.Static | BindingFlags.Public);

        public static void UpdatePostfix(ref SimulationManager __instance)
        {
            var idealTimePerFrame = new TimeSpan(TimeSpan.FromHours(24).Ticks / SimulationManager.DAYTIME_FRAMES);

            if (__instance.m_timePerFrame.TotalMilliseconds != idealTimePerFrame.TotalMilliseconds)
            {
                LoggingWrapper.Log(LoggingWrapper.LogArea.Hidden, LoggingWrapper.LogType.Message, $"Changing time per frame from {__instance.m_timePerFrame.TotalMilliseconds}ms to {idealTimePerFrame.TotalMilliseconds}ms per tick");

                __instance.m_timePerFrame = idealTimePerFrame;
            }

            __instance.m_currentGameTime = new DateTime(((long)(__instance.m_referenceFrameIndex + __instance.m_dayTimeOffsetFrames + __instance.m_referenceTimer) * __instance.m_timePerFrame.Ticks) /*+ __instance.m_timeOffsetTicks*/);
        }
    }
}