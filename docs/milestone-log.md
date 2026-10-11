# Milestone log

## 2026-10-10 — Milestone 0, step 1: .NET baseline

- Authorization: owner requested use of RedIronBot78/Medieval-Simulator-Codex and
  authorized the proposed minimal .NET foundation.
- SPEC-CHECK: serves sections 3.2, 3.3, 8 (Milestone 0), and 12. No material or gameplay logic.
- SIZE-CHECK: one solution, two projects, one smoke test, SDK settings, ignore rules,
  README, and this log. Master specification preserved unchanged.
- Connected the local repository's `origin` to the owner's empty GitHub repository.
- Selected installed SDK 10.0.401 with patch-only roll-forward and provisional `net10.0`
  targets. Godot compatibility remains unverified.
- Core is empty and has no package dependencies. Tests use xUnit; unnecessary coverage
  tooling from the template was removed.
- Verification: `dotnet restore MedievalLifeSimulator.sln` succeeded;
  `dotnet test MedievalLifeSimulator.sln --no-restore` built both projects and passed
  1 test, with 0 failures and 0 skipped. These commands required approved execution
  outside the restricted sandbox. This is a setup check, not material-model validation.
- No commit or push performed. A fresh GitHub checkout cannot contain these files until
  they are committed and pushed; fresh-checkout verification remains pending.
- Next recommended step: separately authorized Godot .NET build/empty-scene setup check.
- Milestone 0 remains open; Milestone 1 is not authorized by this step.

## 2026-10-10 — Milestone 0, step 2: Godot setup verification

- Authorization: owner instructed continued setup without routine confirmation, until
  intervention is required. The explicit gate before Milestone 1 remains in force.
- SPEC-CHECK: verifies the Milestone 0 C# toolchain and empty-scene launch gate.
- SIZE-CHECK: added only the game project, project settings, one empty Node3D scene,
  one C# startup message, its generated stable UID, and setup documentation.
- Downloaded official portable Godot 4.7.2 .NET Windows x86_64 into ignored
  `.local-tools/`; no global editor install or PATH changes. Version reports
  `4.7.2.stable.mono.official.ed1daf0bf`.
- Verified `net10.0` with SDK 10.0.401 and runtime 10.0.12. Game build passed with
  zero warnings/errors; editor import passed; headless launch printed the C# startup
  message; graphics-enabled launch used NVIDIA GeForce RTX 4070 / OpenGL 3.3
  Compatibility and exited with code 0 after 120 frames, with no stderr output.
- Initial sandboxed import could not write Godot user settings. Repeated with approved
  access; the successful checks used normal Godot user settings directories.
- Kept Game outside the headless solution and without a core reference; the real
  simulation bridge belongs to Milestone 2. Renderer choice is reversible.
- Clean source-copy verification: copied the 14 source/configuration files selected
  by Git's ignore rules into a new folder with no build output or Godot cache.
  `dotnet test` restored/built both headless projects and passed 1/1 tests; the game
  built with zero warnings/errors; editor import and headless C# startup both passed.
  This used installed tools and the package cache, not a new machine or Git checkout.
- Local setup checks are complete. No commits or pushes performed. The literal
  fresh-checkout exit gate remains pending until a baseline commit is authorized.
- Intervention point: authorize recording/pushing the baseline to the requested
  repository; entering Milestone 1 still requires separate explicit authorization.
- Remaining scope: no material model, interaction systems, assets, exports, or simulation.
