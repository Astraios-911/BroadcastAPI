using StardewModdingAPI;
using StardewValley;
using StardewValley.Triggers;
using System;

namespace BroadcastAPI
{
    /// <summary>
    /// Centralized condition parsing/evaluation for channel fields.
    /// </summary>
    public static class ConditionResolver
    {
        /// <summary>
        /// Evaluates channel conditions used to invert HideFromMenu.
        /// </summary>
        public static bool ShouldInvertHideFromMenu(List<string>? conditionEntries)
        {
            if (conditionEntries == null || conditionEntries.Count == 0)
                return false;

            foreach (string raw in conditionEntries)
            {
                if (string.IsNullOrWhiteSpace(raw))
                    continue;

                bool allTrue = true;
                foreach (string condition in raw.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
                {
                    if (!EvaluateGameStateQuery(condition))
                    {
                        allTrue = false;
                        break;
                    }
                }

                if (allTrue)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Resolves the first matching value using conditional syntax.
        /// </summary>
        public static string? ResolveFirstConditionalValue(List<string>? entries, string fieldName)
        {
            if (entries == null || entries.Count == 0)
                return null;

            foreach (string raw in entries)
            {
                if (TryResolveConditionalValue(raw, fieldName, out string? value) && !string.IsNullOrWhiteSpace(value))
                    return value;
            }

            return null;
        }

        /// <summary>
        /// Resolves every entry using conditional syntax and returns all resulting values.
        /// </summary>
        public static List<string>? ResolveAllConditionalValues(List<string>? entries, string fieldName)
        {
            if (entries == null || entries.Count == 0)
                return null;

            var resolved = new List<string>();
            foreach (string raw in entries)
            {
                if (TryResolveConditionalValue(raw, fieldName, out string? value) && !string.IsNullOrWhiteSpace(value))
                    resolved.Add(value);
            }

            return resolved.Count > 0 ? resolved : null;
        }

        private static bool TryResolveConditionalValue(string raw, string fieldName, out string? resolvedValue)
        {
            resolvedValue = null;

            if (string.IsNullOrWhiteSpace(raw))
                return false;

            string entry = raw.Trim();
            if (!entry.StartsWith("If ", StringComparison.OrdinalIgnoreCase))
            {
                resolvedValue = entry;
                return true;
            }

            string conditional = entry.Substring("If ".Length).Trim();
            string[] parts = conditional.Split(" ## ", StringSplitOptions.None);
            if (parts.Length < 2)
            {
                ModEntry.ModMonitor?.Log($"[ConditionResolver] Invalid conditional {fieldName} entry (expected ' ## ' separators): {raw}", LogLevel.Warn);
                return false;
            }

            string conditions = parts[0].Trim();
            string trueValue = parts[1].Trim();
            string? falseValue = parts.Length >= 3 ? parts[2].Trim() : null;

            if (string.IsNullOrEmpty(trueValue))
            {
                ModEntry.ModMonitor?.Log($"[ConditionResolver] Invalid conditional {fieldName} entry (empty true value): {raw}", LogLevel.Warn);
                return false;
            }

            if (parts.Length > 3)
            {
                ModEntry.ModMonitor?.Log($"[ConditionResolver] Invalid conditional {fieldName} entry (too many ' ## ' segments): {raw}", LogLevel.Warn);
                return false;
            }

            if (falseValue != null && falseValue.Length == 0)
            {
                ModEntry.ModMonitor?.Log($"[ConditionResolver] Invalid conditional {fieldName} entry (empty false value): {raw}", LogLevel.Warn);
                return false;
            }

            bool result = string.IsNullOrEmpty(conditions) || EvaluateGameStateQuery(conditions);
            resolvedValue = result ? trueValue : falseValue;
            return !string.IsNullOrWhiteSpace(resolvedValue);
        }

        private static bool EvaluateGameStateQuery(string query)
        {
            return GameStateQuery.CheckConditions(query, Game1.currentLocation, Game1.player, null, null, null);
        }
    }
}
