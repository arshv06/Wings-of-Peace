# WebGL + Netlify Deployment Guide (Beginner Friendly)

This project can be deployed as a **frontend-only game** using Unity WebGL.

## What this means
- Your game runs fully in the browser.
- No backend is required.
- Save data is stored locally in the browser storage via `PlayerPrefs` (WebGL-backed by browser IndexedDB/local profile storage).

## 1) Work in a separate Git branch
Use a feature branch so your main branch stays safe.

```bash
git checkout -b feature/webgl-netlify-browser-save
```

You can always switch back later:

```bash
git checkout main
```

## 2) Build Unity for WebGL
In Unity:
1. Open **File > Build Settings**.
2. Select **WebGL** and click **Switch Platform**.
3. Ensure `Assets/Scenes/Main.unity` is in Scenes In Build.
4. Click **Build** and choose output folder: `Build/WebGL`.

After build, you should have `index.html` and `Build/TemplateData` outputs.

## 3) Push branch to GitHub
```bash
git add .
git commit -m "Add WebGL/browser-save compatibility and deployment docs"
git push -u origin feature/webgl-netlify-browser-save
```

## 4) Deploy to Netlify from GitHub
1. Log in to Netlify.
2. **Add new site > Import from Git**.
3. Choose your repo.
4. Set **Branch to deploy** to `feature/webgl-netlify-browser-save` (or main if merged).
5. Build command: *(leave empty for static upload)*
6. Publish directory: `Build/WebGL`
7. Deploy site.

## 5) Save data behavior
- Save data is local to the browser/device/profile.
- Clearing browser site data removes saves.
- Different browsers/devices do not share saves by default.

## Recommended workflow for you
1. Build/test in branch.
2. Deploy that branch on Netlify.
3. When happy, open PR and merge into `main`.
4. Point Netlify to `main` for production.

## Notes about Steam
This web build is great for browser distribution.
For Steam, you typically ship native desktop builds (Windows first), while keeping WebGL as demo/marketing build.
