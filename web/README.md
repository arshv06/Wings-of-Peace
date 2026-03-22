# Wings of Peace Web Frontend Recreation

This folder is a faithful HTML/CSS/JS recreation pass of the Unity frontend using the same project assets.

## Included
- `index.html`
- `styles.css`
- `app.js`

## Recreated interactions
- Fax communication LED, print-ready state, print action
- Draggable Top Secret page
- Main map crosshair movement from X/Y knobs
- Location detection + zoom eligibility LED
- Missile selection with LED feedback and crosshair size/sprite change
- Zoom/minimap flow + combined coordinate readout
- Countdown timer
- Mission submit validation + event log
- Browser persistence via `localStorage`

## Run locally
From repository root:

```bash
python -m http.server 8080
```

Then open:

`http://localhost:8080/web/`

## Notes
- This is a browser recreation, not a Unity export.
- It uses the original asset files under `Assets/Images` and `Assets/Dashboard`.
- Pixel-perfect parity may require a follow-up pass for exact anchoring and typography.
