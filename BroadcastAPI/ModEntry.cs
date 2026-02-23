using HarmonyLib;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Objects;
using static StardewValley.GameLocation;

namespace BroadcastAPI
{
    public class ModEntry : Mod
    {
        private Api? _api;
        public static ModEntry? Instance { get; private set; }
        public static IMonitor ModMonitor { get; private set; }
        public static IFurnitureFrameworkAPI? FurnitureFrameworkAPI;
        public static AssetHandler<CustomChannelData> CustomChannels { get; private set; }
        public static AssetHandler<OverlayData> Overlays { get; private set; }
        public static AssetHandler<EditChannelData> EditChannels { get; private set; }

        public override void Entry(IModHelper helper)
        {
            Instance = this;
            _api = new Api();
            ModMonitor = this.Monitor;

            // Apply Harmony patches
            var harmony = new Harmony(this.ModManifest.UniqueID);
            harmony.Patch(
                AccessTools.Method(typeof(GameLocation), nameof(GameLocation.createQuestionDialogue),
                    new[] { typeof(string), typeof(Response[]), typeof(afterQuestionBehavior), typeof(NPC) }),
                prefix: new HarmonyMethod(typeof(TvPatches), nameof(TvPatches.CreateQuestionDialoguePrefix))
            );
            harmony.Patch(
                AccessTools.Method(typeof(TV), nameof(TV.checkForAction)),
                postfix: new HarmonyMethod(typeof(TvPatches), nameof(TvPatches.CheckForActionPostfix))
            );
            harmony.Patch(
                AccessTools.Method(typeof(TV), nameof(TV.turnOffTV)),
                postfix: new HarmonyMethod(typeof(TvPatches), nameof(TvPatches.TurnOffTVPostfix))
            );

            // Load mod-provided APIs only after all mods are initialized.
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;

            // Initialize asset handlers as instances
            CustomChannels = new AssetHandler<CustomChannelData>(Monitor, ModManifest.UniqueID, "CustomChannels");
            Overlays = new AssetHandler<OverlayData>(Monitor, ModManifest.UniqueID, "OverlaySprite");
            EditChannels = new AssetHandler<EditChannelData>(Monitor, ModManifest.UniqueID, "EditChannels");

            // Register events
            helper.Events.Content.AssetRequested += OnAssetRequested;
            helper.Events.Content.AssetsInvalidated += CustomChannels.OnAssetsInvalidated;
            helper.Events.Content.AssetsInvalidated += Overlays.OnAssetsInvalidated;
            helper.Events.Content.AssetsInvalidated += EditChannels.OnAssetsInvalidated;
        }

        public override object GetApi() => _api!;

        private void OnAssetRequested(object? sender, AssetRequestedEventArgs e)
        {
            // Load empty dictionaries for each asset type
            var baseName = this.ModManifest.UniqueID;
            if (e.NameWithoutLocale.IsEquivalentTo($"{baseName}/CustomChannels"))
                e.LoadFrom(() => new Dictionary<string, CustomChannelData>(), AssetLoadPriority.Exclusive);

            if (e.NameWithoutLocale.IsEquivalentTo($"{baseName}/OverlaySprite"))
                e.LoadFrom(() => new Dictionary<string, OverlayData>(), AssetLoadPriority.Exclusive);

            if (e.NameWithoutLocale.IsEquivalentTo($"{baseName}/EditChannels"))
                e.LoadFrom(() => new Dictionary<string, EditChannelData>(), AssetLoadPriority.Exclusive);
        }

        private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
        {
            if (this.Helper.ModRegistry.IsLoaded("leroymilo.FurnitureFramework"))
            {
                FurnitureFrameworkAPI = this.Helper.ModRegistry.GetApi<IFurnitureFrameworkAPI>("leroymilo.FurnitureFramework");
                if (FurnitureFrameworkAPI != null)
                    this.Monitor.Log("Furniture Framework API loaded successfully!", LogLevel.Info);
            }
        }
    }

    /// <summary>
    /// Generic asset handler to reduce code duplication.
    /// </summary>
    public class AssetHandler<T> where T : class
    {
        private Dictionary<string, T>? _cache;
        private readonly IMonitor _monitor;
        private readonly string _assetName;
        private readonly string _assetType;

        public AssetHandler(IMonitor monitor, string modUniqueId, string assetType)
        {
            _monitor = monitor;
            _assetType = assetType;
            _assetName = $"{modUniqueId}/{assetType}";
            _monitor?.Log($"[{assetType}] Initialized", LogLevel.Trace);
        }

        public Dictionary<string, T> Data
        {
            get
            {
                if (_cache == null)
                {
                    _monitor?.Log($"[{_assetType}] Loading asset: {_assetName}", LogLevel.Trace);
                    try
                    {
                        _cache = Game1.content.Load<Dictionary<string, T>>(_assetName!);
                        _cache ??= new Dictionary<string, T>();

                        _monitor?.Log($"[{_assetType}] Loaded {_cache.Count} entries", LogLevel.Info);

                        if (_cache.Count > 0)
                            foreach (var key in _cache.Keys)
                                _monitor?.Log($"[{_assetType}] Key: {key}", LogLevel.Trace);
                    }
                    catch (Exception ex)
                    {
                        _monitor?.Log($"[{_assetType}] Error loading: {ex}", LogLevel.Error);
                        _cache = new Dictionary<string, T>();
                    }
                }
                return _cache;
            }
        }

        public void OnAssetsInvalidated(object? sender, AssetsInvalidatedEventArgs e)
        {
            foreach (var name in e.NamesWithoutLocale)
            {
                if (name.IsEquivalentTo(_assetName))
                {
                    _monitor?.Log($"[{_assetType}] Asset invalidated, reloading", LogLevel.Info);
                    _cache = null;
                }
            }
        }
    }
}