const state = {
  crypticMessage: 'D3c0d3 th3 m3554g3: Launch at N:3 E:2',
  correctDecodedMessage: 'Decode the message: Launch at N:3 E:2',
  x: 0,
  y: 0,
  miniX: 0,
  miniY: 0,
  selectedMissile: 0,
  timerSec: 7 * 60,
  isFaxReady: false,
  locationIndex: -1,
  zoomActive: false,
  missionHistory: []
};

const locations = [
  { name: 'Apartments', info: 'High civilian density.', x: 7, y: 6, minimap: '../Assets/Images/Martyr Square North.png' },
  { name: 'Martyr Square North', info: 'Crowded urban zone.', x: 5, y: 5, minimap: '../Assets/Images/Martyr Square North.png' },
  { name: 'Martyr Square South', info: 'Transit and residential mix.', x: 4, y: 3, minimap: '../Assets/Images/Martyr Square South.png' }
];

const missiles = {
  1: {
    img: '../Assets/Images/Missile1.png',
    desc: 'Missile 1: precision payload, smaller spread.',
    crosshair: '../Assets/Images/crosshairmissile1.png',
    crosshairSize: 25
  },
  2: {
    img: '../Assets/Images/Missile2.png',
    desc: 'Missile 2: wider blast radius.',
    crosshair: '../Assets/Images/crosshairmissile2.png',
    crosshairSize: 50
  }
};

const el = {
  led: document.getElementById('led'),
  faxDisplay: document.getElementById('faxDisplay'),
  printButton: document.getElementById('printButton'),
  topSecretText: document.getElementById('topSecretText'),
  faxPage: document.getElementById('faxPage'),
  map: document.getElementById('map'),
  crosshair: document.getElementById('crosshair'),
  xKnob: document.getElementById('xKnob'),
  yKnob: document.getElementById('yKnob'),
  xDisplay: document.getElementById('xDisplay'),
  yDisplay: document.getElementById('yDisplay'),
  locationName: document.getElementById('locationName'),
  locationInfo: document.getElementById('locationInfo'),
  locationLed: document.getElementById('locationLed'),
  missile1Button: document.getElementById('missile1Button'),
  missile2Button: document.getElementById('missile2Button'),
  missile1Led: document.getElementById('missile1Led'),
  missile2Led: document.getElementById('missile2Led'),
  missileScreen: document.getElementById('missileScreen'),
  missileDescription: document.getElementById('missileDescription'),
  zoomButton: document.getElementById('zoomButton'),
  minimapImage: document.getElementById('minimapImage'),
  miniCrosshair: document.getElementById('miniCrosshair'),
  miniXKnob: document.getElementById('miniXKnob'),
  miniYKnob: document.getElementById('miniYKnob'),
  combinedCoordinates: document.getElementById('combinedCoordinates'),
  timerText: document.getElementById('timerText'),
  decodedMessageInput: document.getElementById('decodedMessageInput'),
  submitLaunch: document.getElementById('submitLaunch'),
  eventLog: document.getElementById('eventLog')
};

function saveState() { localStorage.setItem('wings_web_save_v1', JSON.stringify(state)); }
function loadState() {
  const raw = localStorage.getItem('wings_web_save_v1');
  if (!raw) return;
  try { Object.assign(state, JSON.parse(raw)); } catch (_) {}
}
function log(msg) {
  const timestamp = new Date().toLocaleTimeString();
  el.eventLog.textContent = `[${timestamp}] ${msg}\n` + el.eventLog.textContent;
}

function setCrosshair(mapEl, crosshairEl, x, y, maxX, maxY) {
  const px = (x / maxX) * mapEl.clientWidth;
  const py = (y / maxY) * mapEl.clientHeight;
  crosshairEl.style.left = `${px}px`;
  crosshairEl.style.top = `${py}px`;
}

function updateMainMap() {
  state.x = Number(el.xKnob.value);
  state.y = Number(el.yKnob.value);
  el.xDisplay.textContent = state.x;
  el.yDisplay.textContent = state.y;
  setCrosshair(el.map, el.crosshair, state.x, state.y, 9, 9);

  const threshold = 1;
  const idx = locations.findIndex((l) => Math.abs(l.x - state.x) <= threshold && Math.abs(l.y - state.y) <= threshold);
  state.locationIndex = idx;

  if (idx >= 0) {
    const loc = locations[idx];
    el.locationName.textContent = loc.name;
    el.locationInfo.textContent = loc.info;
    el.locationLed.classList.remove('off');
    el.locationLed.classList.add('on');
    el.zoomButton.disabled = state.selectedMissile === 0;
  } else {
    el.locationName.textContent = '';
    el.locationInfo.textContent = '';
    el.locationLed.classList.remove('on');
    el.locationLed.classList.add('off');
    el.zoomButton.disabled = true;
  }
  saveState();
}

function updateMiniMap() {
  state.miniX = Number(el.miniXKnob.value);
  state.miniY = Number(el.miniYKnob.value);
  setCrosshair(el.minimapImage.parentElement, el.miniCrosshair, state.miniX, state.miniY, 30, 30);
  el.combinedCoordinates.textContent = `N ${state.x}.${state.miniX}, E ${state.y}.${state.miniY}`;
  saveState();
}

function selectMissile(id) {
  state.selectedMissile = id;
  const m = missiles[id];
  el.missileScreen.src = m.img;
  el.missileDescription.textContent = m.desc;
  el.miniCrosshair.src = m.crosshair;
  el.miniCrosshair.style.width = `${m.crosshairSize}px`;
  el.miniCrosshair.style.height = `${m.crosshairSize}px`;

  el.missile1Led.classList.toggle('on', id === 1);
  el.missile1Led.classList.toggle('off', id !== 1);
  el.missile2Led.classList.toggle('on', id === 2);
  el.missile2Led.classList.toggle('off', id !== 2);

  if (state.locationIndex >= 0) el.zoomButton.disabled = false;
  saveState();
}

function handleZoom() {
  if (state.selectedMissile === 0 || state.locationIndex < 0) return;
  state.zoomActive = true;
  const loc = locations[state.locationIndex];
  el.minimapImage.src = loc.minimap;
  el.miniCrosshair.classList.remove('hidden');
  updateMiniMap();
  log(`Zoomed into ${loc.name}`);
}

function startFaxFlow() {
  el.topSecretText.textContent = state.crypticMessage;
  el.printButton.disabled = true;
  el.faxDisplay.textContent = 'Waiting for Communication...';
  let bright = false;
  const blink = setInterval(() => {
    bright = !bright;
    el.led.style.opacity = bright ? '1' : '.2';
  }, 500);

  setTimeout(() => {
    clearInterval(blink);
    el.led.style.opacity = '.2';
    state.isFaxReady = true;
    el.faxDisplay.textContent = 'Print Ready';
    el.printButton.disabled = false;
  }, 3000);
}

function printFax() {
  if (!state.isFaxReady) return;
  el.printButton.disabled = true;
  el.faxDisplay.textContent = 'Printing...';
  el.faxPage.animate([
    { transform: 'translateY(-20px)' },
    { transform: 'translateY(0px)' }
  ], { duration: 1000, easing: 'ease-out' });
  setTimeout(() => {
    el.faxDisplay.textContent = 'Print Complete';
  }, 1200);
}

function setupDrag() {
  const page = el.faxPage;
  let dragging = false;
  let offX = 0;
  let offY = 0;

  page.addEventListener('mousedown', (e) => {
    dragging = true;
    page.classList.add('dragging');
    const r = page.getBoundingClientRect();
    offX = e.clientX - r.left;
    offY = e.clientY - r.top;
  });

  window.addEventListener('mousemove', (e) => {
    if (!dragging) return;
    const panelRect = page.parentElement.getBoundingClientRect();
    const x = e.clientX - panelRect.left - offX;
    const y = e.clientY - panelRect.top - offY;
    page.style.position = 'absolute';
    page.style.left = `${Math.max(0, x)}px`;
    page.style.top = `${Math.max(60, y)}px`;
  });

  window.addEventListener('mouseup', () => {
    dragging = false;
    page.classList.remove('dragging');
  });
}

function startTimer() {
  setInterval(() => {
    if (state.timerSec <= 0) return;
    state.timerSec -= 1;
    const m = String(Math.floor(state.timerSec / 60)).padStart(2, '0');
    const s = String(state.timerSec % 60).padStart(2, '0');
    el.timerText.textContent = `${m}:${s}`;
    if (state.timerSec % 10 === 0) saveState();
  }, 1000);
}

function submitLaunch() {
  const decoded = el.decodedMessageInput.value.trim();
  const decodedCorrect = decoded.toLowerCase() === state.correctDecodedMessage.toLowerCase();
  const coordsCorrect = state.x === 3 && state.y === 2;
  const missileValid = state.selectedMissile !== 0;
  const success = decodedCorrect && coordsCorrect && missileValid;

  const result = {
    decodedCorrect,
    coordsCorrect,
    missileValid,
    success,
    at: `${state.x},${state.y}`
  };
  state.missionHistory.unshift(result);
  saveState();

  if (success) {
    log('Mission success: launch parameters accepted.');
  } else {
    log(`Mission failed: decoded=${decodedCorrect}, coords=${coordsCorrect}, missile=${missileValid}`);
  }
}

function wireEvents() {
  el.xKnob.addEventListener('input', updateMainMap);
  el.yKnob.addEventListener('input', updateMainMap);
  el.miniXKnob.addEventListener('input', updateMiniMap);
  el.miniYKnob.addEventListener('input', updateMiniMap);
  el.missile1Button.addEventListener('click', () => selectMissile(1));
  el.missile2Button.addEventListener('click', () => selectMissile(2));
  el.zoomButton.addEventListener('click', handleZoom);
  el.printButton.addEventListener('click', printFax);
  el.submitLaunch.addEventListener('click', submitLaunch);
}

function hydrateFromSave() {
  el.xKnob.value = state.x;
  el.yKnob.value = state.y;
  el.miniXKnob.value = state.miniX;
  el.miniYKnob.value = state.miniY;
  el.topSecretText.textContent = state.crypticMessage;
  if (state.selectedMissile) selectMissile(state.selectedMissile);
  updateMainMap();
  updateMiniMap();
  const m = String(Math.floor(state.timerSec / 60)).padStart(2, '0');
  const s = String(state.timerSec % 60).padStart(2, '0');
  el.timerText.textContent = `${m}:${s}`;
}

function init() {
  loadState();
  wireEvents();
  setupDrag();
  hydrateFromSave();
  startFaxFlow();
  startTimer();
  log('System initialized.');
}

init();
