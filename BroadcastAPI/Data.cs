using Microsoft.Xna.Framework;

namespace BroadcastAPI
{
    /// <summary>
    /// Represents a custom TV channel.
    /// </summary>
    public class CustomChannelData
    {
        public string Name { get; set; } = "";
        public string Displayname { get; set; } = "";
        public List<string> Dialogues { get; set; } = new();
        public string? Texture { get; set; }
        public Rectangle SpriteRegion { get; set; } = new Rectangle(0, 0, 42, 28);
        public float AnimationInterval { get; set; } = 150f;
        public int AnimationLength { get; set; } = 2;
        public float Cooldown { get; set; } = 0f;
        public bool Flicker { get; set; } = false;
        public bool Flipped { get; set; } = false;
        public float AlphaFade { get; set; } = 0f;
        public Color Color { get; set; } = Color.White;
        public float Scale { get; set; } = 1f;
        public float ScaleChange { get; set; } = 0f;
        public float Rotation { get; set; } = 0f;
        public float RotationChange { get; set; } = 0f;
        public Vector2 Position { get; set; } = Vector2.Zero;
        public List<string>? Overlays { get; set; } = null;
        public float LayerDepth { get; set; } = 1f;
        public List<string>? Actions { get; set; }
        public List<string>? NextChannel { get; set; } = null;
        public bool HideFromMenu { get; set; } = false;
        public QuestionsData? BQuestions { get; set; } = null;
        public QuestionsData? EQuestions { get; set; } = null;
    }

    /// <summary>
    /// Represents an edit/override for a TV channel.
    /// </summary>
    public class EditChannelData
    {
        public string? Displayname { get; set; } = null;
        public List<string>? Dialogues { get; set; } = null;
        public string? Texture { get; set; } = null;
        public Rectangle? SpriteRegion { get; set; } = null;
        public float? AnimationInterval { get; set; } = null;
        public int? AnimationLength { get; set; } = null;
        public float? Cooldown { get; set; } = null;
        public bool? Flicker { get; set; } = null;
        public bool? Flipped { get; set; } = null;
        public float? AlphaFade { get; set; } = null;
        public Color? Color { get; set; } = null;
        public float? Scale { get; set; } = null;
        public float? ScaleChange { get; set; } = null;
        public float? Rotation { get; set; } = null;
        public float? RotationChange { get; set; } = null;
        public Vector2? Position { get; set; } = null;
        public List<string>? Overlays { get; set; } = null;
        public float? LayerDepth { get; set; } = null;
        public List<string>? Actions { get; set; } = null;
        public List<string>? NextChannel { get; set; } = null;
        public bool? HideFromMenu { get; set; } = null;
        public QuestionsData? BQuestions { get; set; } = null;
        public QuestionsData? EQuestions { get; set; } = null;
    }

    /// <summary>
    /// Represents a custom overlay for TV channels.
    /// </summary>
    public class OverlayData
    {
        public string Name { get; set; } = "";
        public string? Texture { get; set; }
        public Rectangle SpriteRegion { get; set; } = new Rectangle(0, 0, 16, 16);
        public float AnimationInterval { get; set; } = 150f;
        public int AnimationLength { get; set; } = 1;
        public float Cooldown { get; set; } = 0f;
        public bool Flicker { get; set; } = false;
        public bool Flipped { get; set; } = false;
        public float AlphaFade { get; set; } = 0f;
        public Color Color { get; set; } = Color.White;
        public float Scale { get; set; } = 1f;
        public float ScaleChange { get; set; } = 0f;
        public float Rotation { get; set; } = 0f;
        public float RotationChange { get; set; } = 0f;
        public Vector2 Position { get; set; } = Vector2.Zero;
        public float LayerDepth { get; set; } = 2f;
    }

    /// <summary>
    /// Represents questions to ask before or after a channel plays.
    /// </summary>
    public class QuestionsData
    {
        public string Question { get; set; } = "";
        public List<AnswerData> Answers { get; set; } = new();
        public class AnswerData
        {
            public string Text { get; set; } = "";
            public List<string>? Actions { get; set; } = null;
        }
    }
}
