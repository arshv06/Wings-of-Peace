# Game Roadmap: Dystopian Drone Control Game

This roadmap outlines the planned features and milestones for the drone control game.

## Milestone 1: Basic Game Mechanics (Completed)
- [x] Set up the basic UI for the fax system and drone control panel.
- [x] Create animation for the Top Secret page printing out.
- [x] Implement the cryptic message decoding system.
- [x] Make the Top Secret page draggable within the fax panel.

## Milestone 2: Core Gameplay Features (Architecture Implemented)
- [x] Implement multiple missions with varying levels of difficulty via `MissionData` + weighted selection in `MissionManager`.
- [x] Create day-to-day progression with mission result routing through `GameFlowCoordinator` and `DayManager`.
- [x] Add coordinate + missile launch validation through `DroneControlPanel` and `MissionValidationEngine`.
- [x] Implement war progress tracking with bounded metrics in `WarStateManager` and newspaper output in `NewspaperGenerator`.

## Milestone 3: Visual and Audio Enhancements
- [ ] Add sound effects for fax machine printing and missile launches.
- [ ] Implement background music and atmospheric sounds for the game.
- [ ] Refine the visual design of the drone control UI for a more immersive retro-futuristic feel.

## Milestone 4: Final Game Features (Architecture Implemented)
- [x] Implement moral choice framework impacting war and family metrics via `ChoiceData` + `ChoiceManager`.
- [x] Add outcome consequence triggers through endgame thresholds in `WarStateManager`.
- [ ] Finalize all UI elements and make the game fully playable.

## Systems Architecture Notes
- Mission pool supports 20+ missions without code rewrite using ScriptableObject-driven data and day-based weighted selection.
- Mission logic is isolated from UI; UI submits launch inputs while `MissionManager` resolves outcomes.
- Grid targeting is tile-based (`GridCoordinate`) with explicit map bounds and civilian zones (`GridDefinition`).
- Save system writes JSON at end of day and resumes from last completed day (`SaveSystem`, `SaveGameData`).
- Endgame thresholds currently enforced:
  - `regimeStability <= 0` => Regime Collapse
  - `civilianUnrest >= 100` => Civilian Uprising
  - `warScore >= 200` => Military Victory
