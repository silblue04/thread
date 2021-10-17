public enum SceneType
{
    NONE = DefsDefault.VALUE_NONE,

    InGame = DefsDefault.VALUE_ZERO,

    MAX
}

public static class DefsScene
{
    public static string GetSceneName(SceneType contentsThemaType)
    {
        switch (contentsThemaType)
        {
            case SceneType.InGame: return "InGame";
        }
        return string.Empty;
    }
}
