using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using LiteNetLib;
using UnityEngine;
using UnityEngine.UI;
using HarmonyLib;
using Hints;
using Hint = Hints.Hint;
using MEC;
using Exiled.Events.EventArgs.Player;

namespace CustHintShow
{
    public class CHS : Plugin<Config>
    {
        public override string Author => "114514(QQ:3145186196)";
        public override string Name => "CustHintShow(CHS)";
        public override Version Version => new Version(2,1);
        private Harmony _harmony;
        public override void OnEnabled()
        {
            Log.Info("CustHintShow 插件已启用！");

            // 初始化 Harmony
            _harmony = new Harmony("com.114514.custhintshow");
            _harmony.PatchAll();
            Exiled.Events.Handlers.Server.RoundStarted += OnStart;
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Log.Info("CustHintShow 插件已禁用！");

            // 卸载 Harmony 补丁
            _harmony.UnpatchAll();
            Exiled.Events.Handlers.Server.RoundStarted -= OnStart;
            base.OnDisabled();
        }
        private void OnStart()
        {
            foreach (var player in Player.List)
            {
                var hint = new CustHintShow.Hints.Hint
                {
                    X = 960,
                    Y = 600,
                    Size = 20,
                    Text = "欢迎游玩服务器"
                };
                var showplayer = CustHintShow.Display.Get(player);
                showplayer.Add(hint);
                Timing.CallDelayed(5f, () =>
                {
                    showplayer.Delete(hint);
                });
            }
        }
    }
    [HarmonyPatch(typeof(Player), nameof(Player.ShowHint))]
    public static class HintPatch
    {
        public static void Postfix(Player __instance, string message, float duration)
        {
            // 在这里可以修改或扩展提示的显示逻辑
            Log.Info($"玩家 {__instance.Nickname} 显示了提示: {message}");
        }
    }
}
