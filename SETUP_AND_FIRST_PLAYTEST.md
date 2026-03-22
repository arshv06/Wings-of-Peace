# Setup and First Playtest (Unity + WebGL)

Yes — you can clone/switch to the branch, open in Unity, build WebGL, and deploy from there.

This guide gets you to a **working loop** quickly:
1) Start a mission
2) Submit a launch
3) Resolve result
4) Advance day
5) Save to browser storage in WebGL

---

## 1) Get the correct branch locally

```bash
git clone <your-repo-url>
cd Wings-of-Peace
git checkout feature/webgl-netlify-browser-save
```

If that branch is not available yet, use the branch that contains `Assets/Scripts/Core`.

---

## 2) Open in Unity
- Unity version in project: `2022.3.5f1`.
- Open project and let scripts compile.

---

## 3) Minimum scene wiring for “things working”

Create an empty GameObject: `GameSystems`.
Add these components to it:
- `SaveSystem`
- `WarStateManager`
- `DayManager`
- `MissionManager`
- `ChoiceManager`
- `NewspaperGenerator`
- `GameFlowCoordinator`
- `QuickStartGameController`

### Create ScriptableObjects
In Project window:
- Create at least 2–3 `MissionData` assets
- Create 4 `MissileData` assets (Precision, Cluster, Heavy, EMP)
- Create 1 `NewspaperTemplateData`

Optional but recommended:
- Create 1 `GridDefinition`
- Create a few `ChoiceData` assets

### Assign inspector references
- In `MissionManager`: assign `DayManager`, `WarStateManager`, mission pool, missile catalog.
- In `ChoiceManager`: assign `MissionManager`, `WarStateManager`.
- In `DayManager`: assign `SaveSystem`.
- In `GameFlowCoordinator`: assign all manager references.
- In `QuickStartGameController`: assign `MissionManager`, `DayManager`, and a `DroneControlPanel`.

---

## 4) Quick functional test in Editor
1. Press Play.
2. Confirm console logs a mission start.
3. In `QuickStartGameController` inspector, enter:
   - test X / Y
   - missile type
   - decoded message
4. Click component context menu: **Quick Submit Launch**.
5. Confirm logs show mission resolved + success/failure.

If launch is rejected, the usual causes are:
- missile type not present in `MissionManager.missileCatalog`
- no active mission (manager state not `AwaitingLaunch`)
- missing inspector references

---

## 5) Build for WebGL
Use `WEBGL_NETLIFY_DEPLOY.md` for deployment details.

Quick version:
1. File → Build Settings → WebGL → Switch Platform
2. Ensure `Assets/Scenes/Main.unity` is in Scenes In Build
3. Build to: `Build/WebGL`
4. Unity generates `Build/WebGL/index.html`

---

## 6) Deploy branch to Netlify
- Connect repo in Netlify
- Choose your feature branch for preview
- Publish directory: `Build/WebGL`

---

## Can we “finish the game” from here?
Yes — but best done in phases:
1. **Vertical slice first**: 3 missions, 1 branching choice, 1 end condition
2. **Content pass**: 20+ missions through ScriptableObjects
3. **UX pass**: polish, balancing, sound, onboarding text
4. **Release pass**: WebGL performance + deployment hardening

If you want, next I can implement Phase 1 directly in this repo as a playable vertical slice.
