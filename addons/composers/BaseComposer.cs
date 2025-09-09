using Godot;
using System;

public abstract class BaseComposer
{
    public static List<string> ComposerNames = new List<string>();
    protected abstract string ComposerName { get; }

    protected BaseComposer()
    {
        if (ComposerNames.Contains(ComposerName)) 
            throw new Exception($"Failed to Instantiate Composer class {ComposerName}. Reason: Already istantiated!");
        ComposerNames.Add(ComposerName);
    }
}