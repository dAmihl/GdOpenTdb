#if TOOLS
using Godot;
using System;

[Tool]
public class GdOpenTdb : EditorPlugin
{
    public override void _EnterTree()
    {
        base._EnterTree();
        var path = (GetScript() as Resource).ResourcePath.GetBaseDir();
        var script = ResourceLoader.Load(path + "/OpenTdbHTTP.cs") as Script;
        var iconTex = ResourceLoader.Load(path + "/icon.png") as Texture;
        AddCustomType("OpenTdb", "HTTPRequest", script, iconTex);
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        RemoveCustomType("OpenTdb");
    }
}
#endif
