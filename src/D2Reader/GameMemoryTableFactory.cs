namespace Zutatensuppe.D2Reader
{
    using System;
    using System.Collections.Generic;

    public interface IGameMemoryTableFactory
    {
        GameMemoryTable CreateForVersion(string gameVersion, Dictionary<string, IntPtr> moduleBaseAddresses);
    }

    class GameVersion
    {
        public const string Version_1_13_C = "1, 0, 13, 60";
        public const string Version_1_13_D = "1, 0, 13, 64";

        public const string Version_1_14_B = "1.14.1.68";
        public const string Version_1_14_C = "1.14.2.70";
        public const string Version_1_14_D = "1.14.3.71";
    }

    public class GameVersionUnsupportedException : Exception
    {
        public String GameVersion;
        public GameVersionUnsupportedException(String gameVersion) :
            base(string.Format("Failed to create memory table for game version {0}", gameVersion))
        {
            GameVersion = gameVersion;
        }
    }

    public class ModuleNotLoadedException : Exception
    {
        public String ModuleName;
        public ModuleNotLoadedException(String moduleName) :
            base(string.Format("Failed to create memory table, module not loaded {0}", moduleName))
        {
            ModuleName = moduleName;
        }
    }

    public class GameMemoryTableFactory : IGameMemoryTableFactory
    {
        public GameMemoryTable CreateForVersion(string gameVersion, Dictionary<string, IntPtr> moduleBaseAddresses)
        {
            // Refer to the wiki at https://github.com/Zutatensuppe/DiabloInterface/wiki/Finding-memory-table-addresses
            // for information about how to find addresses for a different version of the game.
            switch (gameVersion)
            {
                case GameVersion.Version_1_14_D:
                    return CreateMemoryTableForVersion114D();
                case GameVersion.Version_1_14_C:
                    return CreateMemoryTableForVersion114C();
                case GameVersion.Version_1_14_B:
                    return CreateMemoryTableForVersion114B();
                case GameVersion.Version_1_13_D:
                    return CreateMemoryTableForVersion113D(moduleBaseAddresses);
                case GameVersion.Version_1_13_C:
                    return CreateMemoryTableForVersion113C(moduleBaseAddresses);
                default:
                    throw new GameVersionUnsupportedException(gameVersion);
            }
        }

        private GameMemoryTable CreateMemoryTableForVersion114D()
        {
            return new GameMemoryTable()
            {
                Offset =
                {
                    Quests = QuestOffsets(),
                },
                Address =
                {
                    GlobalData = new IntPtr(0x00344304),
                    World = new IntPtr(0x00483D38),
                    PlayersX = new IntPtr(0x483D70),
                    GameId = new IntPtr(0x00482D0C),
                    LowQualityItems = new IntPtr(0x56CC58),
                    ItemDescriptions = new IntPtr(0x56CA58),
                    MagicModifierTable = new IntPtr(0x56CA7C),
                    RareModifierTable = new IntPtr(0x56CAA0),
                    PlayerUnit = new IntPtr(0x003A5E74),
                    Area = new IntPtr(0x003A3140),
                    StringIndexerTable = new IntPtr(0x4829B4),
                    StringAddressTable = new IntPtr(0x4829B8),
                    PatchStringIndexerTable = new IntPtr(0x4829D0),
                    PatchStringAddressTable = new IntPtr(0x4829BC),
                    ExpansionStringIndexerTable = new IntPtr(0x4829D4),
                    ExpansionStringAddressTable = new IntPtr(0x4829C0),
                }
            };
        }

        private GameMemoryTable CreateMemoryTableForVersion114C()
        {
            return new GameMemoryTable()
            {
                Offset =
                {
                    Quests = QuestOffsets(),
                },
                Address =
                {
                    GlobalData = new IntPtr(0x33FD78),
                    World = new IntPtr(0x0047ACC0),
                    PlayersX = new IntPtr(0x47ACF8),
                    GameId = new IntPtr(0x00479C94),
                    LowQualityItems = new IntPtr(0x563BE0),
                    ItemDescriptions = new IntPtr(0x5639E0),
                    MagicModifierTable = new IntPtr(0x563A04),
                    RareModifierTable = new IntPtr(0x563A28),
                    PlayerUnit = new IntPtr(0x0039CEFC),
                    Area = new IntPtr(0x0039A1C8),
                    StringIndexerTable = new IntPtr(0x479A3C),
                    StringAddressTable = new IntPtr(0x479A40),
                    PatchStringIndexerTable = new IntPtr(0x479A58),
                    PatchStringAddressTable = new IntPtr(0x479A44),
                    ExpansionStringIndexerTable = new IntPtr(0x479A5C),
                    ExpansionStringAddressTable = new IntPtr(0x479A48),
                }
            };
        }

        private GameMemoryTable CreateMemoryTableForVersion114B()
        {
            return new GameMemoryTable()
            {
                Offset =
                {
                    Quests = QuestOffsets(),
                },
                Address =
                {
                    GlobalData = new IntPtr(0x00340D78),
                    World = new IntPtr(0x0047BD78),
                    PlayersX = new IntPtr(0x47BDB0),
                    GameId = new IntPtr(0x0047AD4C),
                    LowQualityItems = new IntPtr(0x564C98),
                    ItemDescriptions = new IntPtr(0x564A98),
                    MagicModifierTable = new IntPtr(0x564ABC),
                    RareModifierTable = new IntPtr(0x564AE0),
                    PlayerUnit = new IntPtr(0x0039DEFC),
                    Area = new IntPtr(0x0039B1C8),
                    StringIndexerTable = new IntPtr(0x47AAF4),
                    StringAddressTable = new IntPtr(0x47AAF8),
                    PatchStringIndexerTable = new IntPtr(0x47AB10),
                    PatchStringAddressTable = new IntPtr(0x47AAFC),
                    ExpansionStringIndexerTable = new IntPtr(0x47AB14),
                    ExpansionStringAddressTable = new IntPtr(0x47AB00),
                }
            };
        }

        private GameMemoryTable CreateMemoryTableForVersion113D(Dictionary<string, IntPtr> moduleBaseAddresses)
        {
            int d2CommonAddress = GetModuleAddress("D2Common.dll", moduleBaseAddresses);
            int d2LaunchAddress = GetModuleAddress("D2Launch.dll", moduleBaseAddresses);
            int d2LangAddress = GetModuleAddress("D2Lang.dll", moduleBaseAddresses);
            int d2NetAddress = GetModuleAddress("D2Net.dll", moduleBaseAddresses);
            int d2GameAddress = GetModuleAddress("D2Game.dll", moduleBaseAddresses);
            int d2ClientAddress = GetModuleAddress("D2Client.dll", moduleBaseAddresses);

            return new GameMemoryTable()
            {
                Offset =
                {
                    Quests = QuestOffsets(),
                },
                Address =
                {
                    GlobalData = new IntPtr(d2CommonAddress + 0x000A33F0),
                    World = new IntPtr(d2GameAddress + 0x111C10),
                    PlayersX = new IntPtr(d2GameAddress + 0x111C44),
                    GameId = new IntPtr(d2NetAddress + 0xB420), //  and the pointer to that address is: new IntPtr(d2NetAddress + 0x70A8);
                    LowQualityItems = new IntPtr(d2CommonAddress + 0xA4EB0),
                    ItemDescriptions = new IntPtr(d2CommonAddress + 0xA4CB0),
                    MagicModifierTable = new IntPtr(d2CommonAddress + 0xA4CD4),
                    RareModifierTable = new IntPtr(d2CommonAddress + 0xA4CF8),
                    PlayerUnit = new IntPtr(d2ClientAddress + 0x00101024),
                    Area = new IntPtr(d2ClientAddress + 0x0008F66C),
                    StringIndexerTable = new IntPtr(d2LangAddress + 0x10A84),
                    StringAddressTable = new IntPtr(d2LangAddress + 0x10A88),
                    PatchStringIndexerTable = new IntPtr(d2LangAddress + 0x10AA0),
                    PatchStringAddressTable = new IntPtr(d2LangAddress + 0x10A8C),
                    ExpansionStringIndexerTable = new IntPtr(d2LangAddress + 0x10AA4),
                    ExpansionStringAddressTable = new IntPtr(d2LangAddress + 0x10A90),
                }
            };
        }

        private GameMemoryTable CreateMemoryTableForVersion113C(Dictionary<string, IntPtr> moduleBaseAddresses)
        {
            int d2CommonAddress = GetModuleAddress("D2Common.dll", moduleBaseAddresses);
            int d2LaunchAddress = GetModuleAddress("D2Launch.dll", moduleBaseAddresses);
            int d2LangAddress = GetModuleAddress("D2Lang.dll", moduleBaseAddresses);
            int d2NetAddress = GetModuleAddress("D2Net.dll", moduleBaseAddresses);
            int d2GameAddress = GetModuleAddress("D2Game.dll", moduleBaseAddresses);
            int d2ClientAddress = GetModuleAddress("D2Client.dll", moduleBaseAddresses);

            return new GameMemoryTable()
            {
                Offset =
                {
                    Quests = QuestOffsets(),
                },
                Address =
                {
                    GlobalData = new IntPtr(d2CommonAddress + 0x00099E1C),
                    World = new IntPtr(d2GameAddress + 0x111C24),
                    PlayersX = new IntPtr(d2GameAddress + 0x111C1C),
                    GameId = new IntPtr(d2NetAddress + 0xB428),
                    LowQualityItems = new IntPtr(d2CommonAddress + 0x9FD98),
                    ItemDescriptions = new IntPtr(d2CommonAddress + 0x9FB94),
                    MagicModifierTable = new IntPtr(d2CommonAddress + 0x9FBB8),
                    RareModifierTable = new IntPtr(d2CommonAddress + 0x9FBDC),
                    PlayerUnit = new IntPtr(d2ClientAddress + 0x0010A60C),
                    Area = new IntPtr(d2ClientAddress + 0x0011C310),
                    StringIndexerTable = new IntPtr(d2LangAddress + 0x10A64),
                    StringAddressTable = new IntPtr(d2LangAddress + 0x10a68),
                    PatchStringIndexerTable = new IntPtr(d2LangAddress + 0x10A80),
                    PatchStringAddressTable = new IntPtr(d2LangAddress + 0x10A6C),
                    ExpansionStringIndexerTable = new IntPtr(d2LangAddress + 0x10A84),
                    ExpansionStringAddressTable = new IntPtr(d2LangAddress + 0x10A70),
                }
            };
        }

        private int GetModuleAddress(string moduleName, Dictionary<string, IntPtr> moduleBaseAddresses)
        {
            if (!moduleBaseAddresses.TryGetValue(moduleName, out IntPtr address))
                throw new ModuleNotLoadedException(moduleName);
            return address.ToInt32() - 0x400000; // - base address
        }

        private int[] QuestOffsets()
        {
            // Offsets are the same for all versions so far.
            return new[] { 0x264, 0x450, 0x20, 0x00 };
        }
    }
}
