# Medieval Life Simulator

A single-player medieval life simulator, beginning with a bronze metalworking workshop.

The source of truth is [MEDIEVAL_LIFE_SIMULATOR_DESIGN.md](MEDIEVAL_LIFE_SIMULATOR_DESIGN.md).
Keep this filename; references to `DESIGN.md` in the specification mean that file.

## Current scope

Milestone 0: a standalone .NET build/test foundation and an empty Godot C# scene. The simulation library
is intentionally empty. One smoke test confirms it builds, can be loaded by the test
runner, and has no direct Godot assembly references. This does not validate any material
behavior or prove future architectural isolation by itself.

The scene only prints a startup message; it has no simulation bridge or gameplay.
Material simulation has not started. Each new milestone requires
the owner's explicit authorization.

## Requirements

- Windows 11 is the initial development platform.
- Git.
- .NET SDK 10.0.401, or a later patch in the 10.0.4xx feature band, as selected by `global.json`.
- Internet access to NuGet for the first package restore.
- Optional: VS Code with Microsoft's C# extension.

All projects target `net10.0`. Godot 4.7.2 .NET was verified to build and run the empty
C# scene with SDK 10.0.401 and runtime 10.0.12 on this Windows machine. This verifies
editor/local execution only; standalone exports and other platforms remain untested.

## Build and test

From the repository root:

```powershell
dotnet --version
dotnet restore MedievalLifeSimulator.sln
dotnet test MedievalLifeSimulator.sln --no-restore
```

`dotnet test` builds both projects and should report one passing test. No Godot editor
or window is required. Subsequent runs can use `dotnet test` alone.

For a fresh checkout:

```powershell
git clone https://github.com/RedIronBot78/Medieval-Simulator-Codex.git
cd Medieval-Simulator-Codex
dotnet restore MedievalLifeSimulator.sln
dotnet test MedievalLifeSimulator.sln --no-restore
```

## Layout

- `src/Simulation.Core/`: pure C# library; no game engine dependencies.
- `src/Simulation.Tests/`: xUnit tests referencing the core.
- `src/Game/`: separate Godot project with an empty scene and C# startup message.
- `docs/milestone-log.md`: completed steps, checks, and remaining gates.

## Godot setup and launch

Download [Godot 4.7.2 .NET for Windows x86_64](https://godotengine.org/download/archive/4.7.2-stable/).
Extract it with its accompanying files. The local setup uses the ignored directory
`.local-tools/godot-4.7.2/Godot_v4.7.2-stable_mono_win64/`. This editor is not included
in Git; a new checkout must obtain it separately. No global PATH changes are required.

From the repository root, using that extraction location:

```powershell
$godot = '.\.local-tools\godot-4.7.2\Godot_v4.7.2-stable_mono_win64\Godot_v4.7.2-stable_mono_win64_console.exe'
dotnet build src/Game/Game.csproj
& $godot --headless --path src/Game --editor --import
& $godot --headless --path src/Game --quit-after 3
& $godot --path src/Game
```

The headless launch should print `Milestone 0: empty scene started with C#.` and exit.
The normal launch opens an intentionally empty window; close it normally. To open
the editor, run `& $godot --path src/Game --editor`.

The game project stays outside the headless solution so core tests remain independent
of the Godot SDK. It deliberately does not reference the core until the interaction
milestone. Compatibility rendering is a reversible setup choice, not a final graphics
decision. Keep generated `.cs.uid` source identifiers in Git; ignore `.godot/` caches.

VS Code's C# extension is optional and was not installed by this setup. If desired,
install Microsoft's C# extension and select Visual Studio Code under Godot's
Editor Settings > Dotnet > Editor > External Editor.

## Next milestone (authorization required)

Blender and export templates can wait.
Milestone 1, after separate authorization, will begin with constituent masses and
composition validation for an abstract specimen, before thermal or manufacturing rules.
