// Guids.cs
// MUST match guids.h

using System;

namespace BariVsAddon
{
    static class GuidList
    {
        public const string GuidBariVsPackagePkgString = "99e01380-a443-4a37-af07-527015b79f78";
        public const string GuidBariVsPackageCmdSetString = "18c9d376-aa94-48b5-9340-0bd5cae2f67c";

        public static readonly Guid GuidBariVsPackageCmdSet = new Guid(GuidBariVsPackageCmdSetString);
    };
}