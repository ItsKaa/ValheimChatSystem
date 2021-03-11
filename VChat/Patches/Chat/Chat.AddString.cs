﻿using HarmonyLib;
using VChat.Data;

namespace VChat.Patches
{
    [HarmonyPatch(typeof(Chat), nameof(Chat.AddString), typeof(string), typeof(string), typeof(Talker.Type))]
    public static class ChatPatchAddStringFormatting
    {
        private static bool Prefix(ref Chat __instance, ref string user, ref string text, ref Talker.Type type)
        {
            // Pings should not add a message to the chat.
            if (type == Talker.Type.Ping)
            {
                return false;
            }

            __instance.AddString(VChatPlugin.GetFormattedMessage(new CombinedMessageType(type), user, text));
            return false;
        }
    }

    [HarmonyPatch(typeof(Chat), nameof(Chat.AddString), typeof(string))]
    public static class ChatPatchAddStringToBuffer
    {
        private static void Prefix(ref Chat __instance, ref string text)
        {
            // Display chat window when a new message is received.
            if (VChatPlugin.Settings.ShowChatWindowOnMessageReceived)
            {
                __instance.m_hideTimer = 0;
                VChatPlugin.ChatHideTimer = 0;
            }
        } 
    }
}
