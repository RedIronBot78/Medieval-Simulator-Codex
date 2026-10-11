# MEDIEVAL LIFE SIMULATOR
## Codex Project Design Specification — v0.1

**Status:** Planning baseline; implementation not yet authorized by this document alone  
**Date:** 2026-10-10  
**Working title:** Medieval Life Simulator (temporary)  
**Initial playable focus:** Blacksmithing / bronze metalworking  
**Long-term genre:** Single-player, first-person, open-world medieval/fantasy life simulator and survival crafter  
**Owner's priority:** Systems-driven freedom, believable physical crafting, a living society, and extensibility to other professions  

> **Instructions to Codex:** Treat this document as the source of truth for design intent and scope. Read it in full before proposing implementation. Do not treat long-term features as tasks to build in the first milestone. Preserve the distinction between **vision**, **committed MVP scope**, and **future possibilities**. When the document leaves a technical detail unresolved, make the smallest reversible choice, identify the assumption, and avoid making irreversible architecture decisions silently. This is a collaborative, plan-first project with a user who is not a professional software developer; describe decisions in clear language.

---

## 1. Product vision

Build a world that behaves according to interconnected physical, economic, and social rules rather than a collection of isolated crafting recipes and scripted quests.

A player may settle in a village as a blacksmith, produce tools that villagers actually need, negotiate prices, compete with another smith, travel to another settlement, supply an army during a war, change professions, or pursue a substantially different way of life. Eventually, professions, resources, institutions, creatures, magic (if adopted), and the environment should participate in one shared simulation.

**Design principle:** Simulate **causes, capabilities, constraints, and consequences**. Do not script every permitted outcome.

### 1.1 Core pillars

1. **Process matters:** How a tool is made changes how it performs. Material composition alone is insufficient.
2. **The world has internal needs:** Villagers require goods to perform jobs; their demand is not solely generated as player quests.
3. **Agency without predefined morality branches:** Trade, collaboration, persuasion, boycott, sabotage, violence, migration, or specialization should eventually be possible where world systems support them. NPC responses follow their knowledge, incentives, relationships, norms, and available evidence.
4. **Persistent consequences:** Items can wear out, inventories change, relationships evolve, resources are consumed, and decisions matter beyond immediate dialogue.
5. **Reusable rules:** Future carpentry, pottery, mining, farming, construction, and other professions extend shared systems rather than becoming unrelated minigames.
6. **Pragmatic realism:** Model the real mechanisms that players can meaningfully perceive and influence. Use approximations where full scientific simulation would be too expensive or not fun.
7. **Playable first:** Every milestone must deliver a small, complete, testable improvement. Prefer a convincing workshop loop over a huge unfinished open world.

### 1.2 Intended player experience

- First-person interaction with tools, objects, and a workshop.
- The player learns a craft by experimenting and observing results, not merely unlocking recipes or increasing an abstract skill number.
- A poor tool can result from inappropriate composition, bad casting, poor shape, excessive deformation, a bad edge, or other identifiable causes.
- Customers value function, reliability, delivery, price, and reputation differently.
- The player is not required to become a blacksmith in the final game; blacksmithing is the first profession used to prove the world framework.

---

## 2. Scope hierarchy: do not conflate these levels

### 2.1 Long-term vision — NOT initial implementation scope

- Traversable open world, multiple settlements, wilderness, travel, survival systems.
- Persistent population with household needs, occupations, memory, relationships, and local politics.
- Supply chains, resource extraction, transport, markets, money/credit, and regional trade.
- Multiple detailed professions and construction systems.
- Crime, enforcement, persuasion, agreements, property, conflict, and political structures.
- Armies, wartime demand, equipment supply, and changing regional conditions.
- Creatures and fantasy elements with consistent rules rather than arbitrary exceptions.
- Optional natural-language NPC conversations and potentially local AI assistance.
- Large-scale simulation that remains coherent when parts of the world are not rendered.

These are **architectural direction**, not near-term feature commitments.

### 2.2 First substantial vertical slice — the blacksmith's village

A small, self-contained playable village with:

- One interactive workshop and one player-controlled blacksmith.
- Basic material acquisition, costs, and stock management.
- A small variety of tools with differentiated uses and wear.
- Approximately 8–12 simulated villagers with jobs, wallets, inventories, and equipment needs.
- A simple village economy where villagers request or buy tools because they need to work.
- One NPC competitor whose production, pricing, and decisions can affect the player.
- Basic shop trading, relationships, reputation, negotiation, and price competition.
- Persistent world state and saves.

**This is NOT the first coding milestone.** It is the target after the crafting and simulation foundations have been proven.

### 2.3 First playable MVP — the single-workshop prototype

Build **one contained workshop** with a complete bronze-object production-and-testing loop:

1. Acquire measured quantities of copper and tin (initially via a simple inventory).
2. Select a composition by **mass**, showing a clear breakdown of components.
3. Melt and combine metal in a simplified furnace/crucible simulation.
4. Cast one simple tool head (initial candidate: a chisel or axe head).
5. Cool it, optionally cold-work and anneal appropriate regions, then finish its edge.
6. Test the resulting tool on a standardized target.
7. Inspect performance and an explanation linked to its actual production history.
8. Repeat with different compositions or technique; get measurably different outcomes.

A basic 3D workspace should support this loop, but appearance may initially be placeholder-quality. No villagers, open world, combat, multiplayer, realistic characters, or elaborate economy are required for this MVP.

### 2.4 Very first implementation milestone — material lab (headless)

Before 3D crafting, implement a small, testable core that can:

- Represent material quantities and composition in consistent units.
- Simulate a minimal set of thermal and process state transitions.
- Record production history and derived properties.
- Compare two differently manufactured test specimens.
- Produce deterministic, inspectable reports from automated tests or a console harness.

This isolates the most technically uncertain part of the design and makes it easier to replace weak approximations later.

---

## 3. Technology and development constraints

### 3.1 Default stack

| Layer | Default | Rationale |
|---|---|---|
| Game engine | **Godot 4.7.2 .NET** | 3D, tools, editor, C# support, open source |
| Primary language | **C#** | Maintainable simulation models and .NET testing |
| Simulation core | **Standalone .NET class library** | Test without rendering or launching Godot |
| Editor | VS Code / Cursor / Codex | AI-assisted incremental work |
| Modeling | Blender | 3D assets when they are needed |
| Version control | Git | Small reversible changes and safe experiments |
| Definitions | JSON or equivalent versioned text data | Human-readable, source-controlled material/item definitions |
| Unit testing | xUnit or equivalent .NET test framework | Fast deterministic testing |
| Persistence | Versioned files for MVP; evaluate SQLite later | Avoid premature database complexity |
| Initial target platform | Windows desktop | Match available development environment |

**Important technical caveat:** Use the Godot **.NET** editor, not the standard build, and install a compatible .NET SDK. Godot 4 C# projects have export-platform limitations; web export is not a goal for this project. Use the stable compatible runtime verified at project setup.

**Development machine:** Windows 11; AMD Ryzen 7 7800X3D; NVIDIA RTX 4070 12 GB; 64 GB RAM. Optimize for a modest Windows build first; do not assume powerful hardware will justify uncontrolled per-entity physics.

### 3.2 Architectural invariants

1. The core material and economy simulations **must not depend on Godot scene nodes**.
2. Separate **definitions** (what copper, a hammer type, or a villager occupation is) from **instances** (this particular ingot, hammer, or person and its current state).
3. Maintain explicit and documented units: **kilograms, meters, seconds, kelvin/°C with proper conversions, joules, pascals**, etc., internally. The UI may offer customary units later. Never mix unit conventions implicitly.
4. Prefer seeded random generation and deterministic state transitions where practical. The same inputs and seed should reproduce the same material-lab result.
5. Simulation produces authoritative state changes; rendering, sounds, and animations respond to it. UI must not secretly decide tool properties.
6. Each item receives a stable identity and a clear state/history. Avoid hardcoding item behavior by display name.
7. Save formats must be versioned once persistence is introduced.
8. Expensive simulations run at a suitable frequency or level of detail; off-screen objects need not update at rendering frame rate.
9. Keep foundational interfaces small. No speculative plugin architecture, networking, distributed services, or over-engineered ECS until measurements justify them.
10. Explain simplified models and their limitations. Never label game heuristics as verified scientific predictions.

### 3.3 Conceptual project structure (not mandatory exact filenames)

```text
MedievalLifeSimulator/
  DESIGN.md                 # This document
  README.md                 # Installation, setup, play/test instructions
  docs/
    decisions/              # Short architecture decisions as they arise
    material-model.md       # Data sources, assumptions, equations, limits
    milestone-log.md        # What was completed and tested
  src/
    Simulation.Core/        # Pure C# simulation: materials, items, time, events
    Simulation.Tests/       # Headless automated tests
    Game/                   # Godot .NET project: scenes, rendering, controls, UI
      assets/
      scenes/
      scripts/
      data/
  tools/                    # Optional CLI or simulation experiment harness
```

Keep a solution that can run its core tests using `dotnet test` without launching Godot. Do not generate complex boilerplate folders before they contain useful code.

---

## 4. Materials and manufacturing simulation

### 4.1 Scientific fidelity philosophy

The goal is **calibrated plausibility**, not a real-time metallurgical research solver. Accuracy matters when it changes an understandable player decision. Simplify where measurement data, computational cost, or playability make high fidelity impractical. Document each approximation.

There is **no universal single “quality” stat** and no material hierarchy such as bronze < iron < steel that applies to every use case. Tool performance is contextual.

### 4.2 Material and item representation

Every manufactured item should ultimately track or derive:

- Unique item ID and type/use intent.
- Total mass and constituent masses (e.g., copper, tin, impurities).
- Composition expressed as **mass fractions** that sum to one within tolerance.
- Geometry: overall shape, dimensions, edge profile, effective thickness, and mass distribution at a suitable level of detail.
- Thermal state and process history (heated, melted, poured, cooled, annealed, cold-worked, etc.).
- Material state: local or simplified hardness, ductility/toughness, work hardening, internal defects, and other properties once supported.
- Surface/edge condition, current wear, and damage.
- Manufacturing event history with enough detail to explain outcomes.
- Ownership/location only when the world simulation requires them.

Use derived quantities where possible, rather than many manually tuned, conflicting stats.

### 4.3 Initial bronze system

Start with **copper + tin** only, by mass, plus optional configurable impurity level later. Make alloy composition continuous within a bounded, plausible design space, rather than discrete recipes. Example experimental compositions include **90 wt% Cu / 10 wt% Sn** and **85 wt% Cu / 15 wt% Sn**. Do not assume one is always superior: trade-offs depend on process and application.

Initial process model:

| Process | Player input | Simulation state changed |
|---|---|---|
| Weigh/alloy | Amounts of Cu, Sn | Mass, composition, possible loss/waste |
| Heat/melt | Heat input, fuel/air or furnace control, time | Temperature and molten/solid state |
| Pour/cast | Mold choice, pour conditions | Shape, casting conditions, possible defects |
| Cool | Cooling conditions/time | Temperature, solidification state, relevant properties |
| Cold-work | Hit location, strike force and geometry | Local shape, strain/work hardening, damage risk |
| Anneal | Heat treatment at appropriate conditions | Recover workability / lower accumulated cold-work effects |
| Sharpen | Material removal, angle/profile | Edge geometry, mass, usability |
| Use/test | Load/target and usage cycles | Functional effectiveness, edge wear, deformation or failure |

**Metallurgical guardrails:** Bronze casting and post-casting working are distinct from iron/steel forging. Do **not** give tin bronze steel-style quench-hardening. Do not equate color with exact temperature as a universal physical rule. Look up actual property ranges, temperature-dependent behavior, and limitations when implementing calibration tables; record sources in `docs/material-model.md`. Initial numerical constants may be labeled placeholders and must be revisited before claiming realism.

### 4.4 Conservation and process constraints

- Preserve material mass within explicitly modeled losses, additions, grinding waste, or discarded material.
- Make it impossible to produce more finished material than went into the process without a documented input.
- Temperature changes should come from heat exchange/energy inputs, not arbitrary quality bonuses.
- Annealing and cold-working change relevant material states; they do not create free mass or make every attribute better.
- Geometry should influence usability: a sharp, brittle thin edge and a thick, durable edge can behave differently.
- The same workpiece should produce different outcomes under different player-controlled process histories.
- Distinguish *measured/grounded* behavior from *invented/gameplay* approximations in code and documentation.

### 4.5 Simulation fidelity ladder

Build incrementally:

**Level A — Phase 1:** Scalar/lumped workpiece: mass, composition, temperature, process history, several derived mechanical attributes; no mesh deformation necessary.

**Level B — Workshop MVP:** A small number of functional regions (e.g., edge/body/socket) with different shape and work-history states; player actions affect selected regions.

**Level C — Future R&D:** More detailed spatial grid/voxel/mesh deformation and richer thermomechanics if prototype testing demonstrates the benefit.

**Explicitly defer:** atomistic simulation, full computational fluid dynamics, unrestricted runtime finite-element analysis, microscale grain simulation, and per-strike high-resolution remeshing. These can be investigated separately but must not block the MVP.

### 4.6 Quality feedback and validation

A finished item's inspection report should explain **why** it performs as it does, e.g.:

- Actual composition and total mass.
- Process timeline and approximate thermal exposure.
- Shape and edge geometry.
- Estimated hardness / workability / toughness, with uncertainty where appropriate.
- Defect and wear indicators.
- Performance under standardized test conditions.
- Observed consequences, e.g., quicker edge wear, greater chipping risk, reduced cutting efficiency.

Do not expose a magical letter grade without context. A single player-friendly score may be displayed for a *specific task* (e.g., woodcutting efficiency), but it must be derived from a documented assessment of physical properties.

---

## 5. Hands-on crafting gameplay

**Goal:** Make real craft decisions meaningful through direct manipulation, not a timing-bar substitute.

### 5.1 First-person controls (prototype)

- Mouse: aim/select a region or working surface.
- Input for hammer placement, angle, and force; begin with a controlled strike model, not full hand-tracking physics.
- Context-sensitive interaction to grasp/move the workpiece, tongs, molds, and tools.
- Workpiece visual feedback after strikes, heating, sharpening, and tests.
- Clear states preventing impossible operations (e.g., handling hot work without suitable tools, hammering fully molten metal).
- Simple inspect/debug UI explaining the workpiece's physical state.

Exact bindings are a UX experiment and can change. Prioritize understandable response and repeatability before complex animation.

### 5.2 First testing station

Choose one standardized, reproducible test appropriate to the initial item:

- A **chisel** cutting a test material, or
- An **axe head** with an explicitly defined handle and a standardized chopping target.

Performance should depend on applicable geometry/material states, not a preselected item-tier multiplier. Handle fit or attachment becomes relevant if the axe is chosen.

### 5.3 Gameplay progression without arbitrary skill locks

Learning primarily changes **player execution** and understanding. Later NPC/player skill can affect physical precision or action consistency, but should not be the primary determinant of item quality. Avoid recipes that require a level to unlock physically possible actions.

---

## 6. World and NPC design — build after the workshop loop

### 6.1 Living villagers

NPCs eventually have:

- Occupation, household, money/resources, inventory, equipment and its condition.
- Needs/goals (food, shelter, tools, security, income, family, relationships).
- Knowledge, attitudes, trust/reputation, memories of meaningful events.
- A plausible set of actions evaluated based on feasibility, likely value, norms, and risk.

NPC actions need not be fully animated or evaluated every render frame. Use event-driven or scheduled simulation where possible.

### 6.2 Simulated demand

Example: a woodcutter needs an axe to work. The axe wears, can break, and can be repaired or replaced. If the woodcutter lacks funds, they may postpone replacement, borrow, barter, or seek cheaper alternatives. If wood is scarce, carpenters and households may face higher costs. Supply and demand should arise from occupations, activities, and available stock.

Avoid: villagers materializing infinite money, buying unlimited tools, or ordering random items without a reason.

### 6.3 Competition scenario — future reference test

An NPC blacksmith opens a nearby shop. The player can eventually attempt to:

- Lower prices or compete on quality/delivery.
- Negotiate, cooperate, specialize, form agreements, or purchase the business.
- Persuade villagers to boycott the competitor.
- Sabotage, threaten, or kill the competitor, with consequences arising from knowledge, witnesses, relationships, local law, and retaliation.
- Relocate or change products and markets.

**Crucial test:** These responses should arise from reusable trade, dialogue, social, ownership, and crime systems—not separate hardcoded "blacksmith rival" branches.

### 6.4 NPC decision system

Start with transparent utility/goal-based decisions over a constrained menu of legal game actions. The world's authoritative systems decide what actions are feasible and what state changes occur.

An optional LLM dialogue layer can be researched later for rich conversations. It must **not** be authoritative for physics, inventory, currency, or consequences and should not be required for basic NPC operation. The game should function completely offline without an external AI service.

### 6.5 Simulation at distance

Use different levels of simulation detail:

1. **Nearby/visible:** 3D physics, interactions, locomotion, animation.
2. **Same settlement/offscreen:** occupation schedules, inventory and economy changes, compact state updates.
3. **Distant regions (future):** coarse trade, production, scarcity, politics, events.

Cross-region state must remain coherent when areas are loaded/unloaded. Start with a tiny world and only add aggregation once a real performance need appears.

---

## 7. MVP interaction loop and example user story

### 7.1 Core loop

```text
Select materials -> weigh -> heat/melt -> cast -> cool
       -> optionally work/anneal -> sharpen -> test
       -> inspect results -> experiment again
```

### 7.2 Acceptance story: two bronze tools

1. Player produces a test tool with one Cu/Sn composition.
2. Player repeats with a different composition, keeping other variables as controlled as possible.
3. Game preserves mass and shows different derived properties where the model supports differences.
4. Player modifies workmanship on a new tool (work history or sharpening geometry).
5. Standardized testing produces explainable performance differences.
6. Player can understand the effects from an inspection panel and manufacturing history.
7. Saving and reopening the game preserves both items and their attributes once persistence is included.

A cosmetic color change alone does **not** satisfy this story. A predefined random "quality" roll alone does **not** satisfy it.

---

## 8. Development plan and exit gates

Each phase is entered only after the previous phase can be demonstrated and its tests pass. No fixed calendar promises.

### Milestone 0 — Repository and design baseline

**Deliver:** Repo skeleton, Godot/.NET toolchain decisions, `.gitignore`, build/test instructions, this design file, and a brief first-implementation plan.

**Exit:** A fresh checkout can run a minimal .NET test; any Godot project created launches a basic empty scene. No substantial gameplay required.

### Milestone 1 — Headless material lab

**Deliver:** Pure C# material entities, composition validation, process events, deterministic thermal/process state transitions, basic derived properties, comparison/report command or tests.

**Exit tests:**

- Composition fractions add to 1 within tolerance.
- Material input mass = output mass + modeled loss (within defined tolerance).
- Repeated simulation with same seed and inputs gives the same outputs.
- Out-of-range composition, impossible temperatures/operations, invalid numeric states, and negative masses are rejected or handled explicitly.
- Two controlled alloy/process variants produce differences for explainable modeled reasons.
- Headless test suite passes without Godot.

### Milestone 2 — Minimal Godot interaction lab

**Deliver:** Launchable first-person workshop test scene; inspect/debug HUD; basic interaction with a workpiece and one process station; bridge to Simulation.Core.

**Exit:** Changing an action in the 3D scene changes simulation state and displayed outcome; Godot presentation contains no competing material calculation logic.

### Milestone 3 — Complete single-tool making loop

**Deliver:** Melt/cast/cool, region-based cold-working/annealing where relevant, finishing/sharpening, one functional test, comparison reports, temporary placeholder visuals.

**Exit:** The entire core loop is playable end-to-end; player-controlled composition **and** at least one player-controlled workmanship input independently affect test results; impossible actions provide intelligible feedback.

### Milestone 4 — Persistent shop

**Deliver:** Item instances, inventory/cost basics, versioned save/load, meaningful material consumption and production waste, item inspection history.

**Exit:** Restarting does not regenerate or silently alter produced items; inventory is conserved.

### Milestone 5 — Village economy

**Deliver:** A small village, several occupations, 8–12 villagers, money/resources, tool demand, purchases, tool wear, and simple production schedules.

**Exit:** Villagers need tools for actual simulated work; purchases, lack of money, damaged tools, and tool availability change actions without player-triggered quests.

### Milestone 6 — Rival blacksmith and social decisions

**Deliver:** NPC shop, competitive pricing, relationships, core negotiation/boycott options, rudimentary persistent reputation, basic consequences for illegal actions as capabilities allow.

**Exit:** Multiple outcomes arise from common world systems; competitor has limited stock, resources, and incentives rather than cheating arbitrarily.

### Milestone 7 — Regional expansion (separate future plan)

**Explore:** Travel, settlements, merchant movement, scarcity, warfare demand, other professions. Requires a new scoped design and performance evaluation before starting.

---

## 9. Automated tests and instrumentation

### 9.1 Test categories

- **Unit and invariants:** units, mass balance, valid state transitions, alloy fractions, temperature math, region state.
- **Property/edge-case tests:** ranges, large/small inputs, impossible operations, zero-mass, rounding, no NaN/Infinity.
- **Deterministic experiment fixtures:** identical seeds and actions reproduce results; controlled variables isolate cause/effect.
- **Integration:** Godot command changes authoritative simulation object; report accurately reflects it.
- **Persistence:** save/load round-trips preserve IDs, quantities, item history and results.
- **Simulation integrity later:** no free wealth/items; NPC decisions respect inventory, affordability and availability.

### 9.2 Development diagnostics

- Debug overlay for selected item ID, mass, composition, temperature, region state, defects, and process history.
- Human-readable experiment report (inputs, actions, outputs, reason for outcome).
- Basic performance measurements before pursuing optimizations.
- Prefer stable reproducible test scenarios over visually judging whether an alloy feels right.

### 9.3 Performance target (goal, not benchmark evidence)

Aim for a responsive desktop game at 1080p and eventually ~60 FPS on the development PC. Keep material simulation decoupled from rendering to make this feasible. Do not prematurely implement costly world streaming or multithreading; profile actual bottlenecks.

---

## 10. Deliberate non-goals for the first playable MVP

Do **not** implement unless the owner explicitly changes the milestone:

- Open-world terrain generation, multiple villages or distant travel.
- NPC conversations, LLM NPCs, emergent political simulation.
- Combat, crime system, war, faction reputations, property law.
- Multiplayer, networking, online services, account systems.
- Survival meters, hunger, thirst, full day/night and seasons.
- Iron/steel metallurgy; swords/armor catalogs; magic materials.
- Arbitrary RPG crafting levels, rarity tiers or recipe-unlock progression.
- Photorealistic characters or production art pipeline.
- High-resolution fluid mechanics, FEM, atom-scale metallurgy.
- Marketplace/monetization, Steam publishing, console exports.

These are deferred, not rejected long-term.

---

## 11. Decisions, assumptions, and open questions

### 11.1 Working decisions

- **Single-player first; offline-capable.**
- **First-person 3D** and mouse/keyboard first.
- **Godot .NET + C#** as default, pending a setup spike; no new engine evaluation unless a technical blocker is demonstrated.
- **Bronze** is the first material domain because alloy experiments clearly show why composition matters. It is a metalworking slice; later iron/steel forging is a distinct mechanical problem.
- **One tool head** and one reproducible usage test before expanding item variety.
- **No required AI/LLM dependency** in the initial game.
- **Real-world metallurgy** is the calibration reference for mundane materials; fantasy elements should follow similarly consistent, testable rules later.

### 11.2 Questions to resolve when they become relevant

1. Is the first tool a chisel (simpler testing) or an axe (more immediately evocative, but needs handle and impact modeling)? **Default for Milestone 1: use an abstract test specimen; select before Milestone 3.**
2. How fine is hammer control: target+force+angle or full physical hammer motion? **Default: target+force+angle.**
3. How much real-world metallurgy is worth modeling at the first playable stage? **Default: mass-conserving, evidence-grounded simplified material model with documented assumptions.**
4. Is fantasy magic part of the initial material system? **Default: no, but keep room for future rules.**
5. How much automation should skilled smithing allow? **Default: none needed to prove the loop; revisit after testing manual interactions.**
6. Are tool mass and price displayed in metric or US customary units? **Default: internal SI; offer UI conversion when a player-facing UI is implemented.**

No question here blocks Milestone 0 or 1.

---

## 12. Codex working agreement — essential

The project owner prefers discussion and planning before unsolicited coding, and small auditable changes. Follow this routine:

**Before changing files:**

1. Read `DESIGN.md`, `README.md`, and relevant existing code; identify the current milestone.
2. State the requested result and which design requirements it serves (**SPEC-CHECK**).
3. Identify the minimum files/features required and avoid unnecessary refactors (**SIZE-CHECK**).
4. Propose a short, reviewable plan. If the owner asks for planning only, **do not edit files**.
5. If the request is to implement an agreed step, make only that step, not an entire milestone's worth of speculative work.

**While implementing:**

- Prefer small readable C# types and explicit contracts.
- Add or update tests alongside new simulation logic.
- Preserve the separation between pure simulation and Godot presentation.
- Avoid large dependencies, premature abstractions, generated complexity, or unrelated style changes.
- Do not fabricate scientific citations, property constants, or validation results. Label uncalibrated values as placeholders.
- Maintain a compact change log and note any deviation from this design.
- Do not perform destructive operations, commit, push, install global tooling, or publish without explicit approval when relevant.

**At the end of an implemented step, report:**

- What changed (files/features, plain English).
- How to run or test it, including exact commands where available.
- Test results and any unverified aspects.
- Assumptions or design decisions made.
- A **single recommended next small step**.

**If a requested feature conflicts with this specification:** Explain the conflict and propose the smallest coherent amendment. Do not silently override design intent.

---

## 13. First Codex task — copy/paste as an initial message

> Read `MEDIEVAL_LIFE_SIMULATOR_DESIGN.md` in full and inspect the repository, if one exists. We are still in the planning/setup stage. **Do not write implementation code yet.** Confirm the MVP boundary, identify any technical setup blockers for Godot 4.7.2 .NET + C#, and propose the smallest Milestone 0 repository/setup plan with file layout, exact tools needed, how we'll verify it works, and any assumptions. Apply SPEC-CHECK and SIZE-CHECK. Keep the explanation accessible to a non-programmer. Do not advance into the material simulator until I explicitly authorize that step.

If this document is saved as `DESIGN.md` in the repository rather than the full filename, update the reference above accordingly.

---

## 14. Research references for implementation

Use official documentation for tooling. These links establish engine capabilities, not the physical validity of any material model:

- Godot stable release archive: https://godotengine.org/download/archive/
- Godot Windows .NET download: https://godotengine.org/download/windows/
- Godot C# basics and setup: https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_basics.html
- Godot documentation: https://docs.godotengine.org/en/stable/

**Metallurgy sources:** To be collected and logged with their applicable composition ranges, experimental conditions, units, and confidence before assigning physical calibration values. Do not infer precise material properties from generic game design articles or unsupported intuition.

---

## 15. Criteria for the project's ultimate success

The project will eventually be successful if players can conceive and execute strategies not prewritten as quests, while the world responds coherently because its physical, economic, and social systems are internally consistent.

The **initial prototype** succeeds at a smaller, measurable question:

> Can two players—or the same player using different alloys and techniques—make visibly and functionally different tools because the game models the actual manufacturing process rather than selecting an outcome from a fixed recipe?

Everything else builds from that proof.
