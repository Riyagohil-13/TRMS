import gsap from "gsap";

export function getClipPathsForDirection(direction = "top-bottom") {
  switch (direction) {
    case "bottom-top": return { hide: "inset(100% 0 0 0)", reveal: "inset(0 0 0 0)" };
    case "left-right": return { hide: "inset(0 100% 0 0)", reveal: "inset(0 0 0 0)" };
    case "right-left": return { hide: "inset(0 0 0 100%)", reveal: "inset(0 0 0 0)" };
    default: return { hide: "inset(0 0 100% 0)", reveal: "inset(0 0 0 0)" };
  }
}

export const presets = {
  cfg1: { 
    rows: 6, 
    cols: 8, 
    layers: 2, 
    stepDuration: 0.25, // Original: 0.35
    stepInterval: 0.03, // Original: 0.05
    moverPauseBeforeExit: 0.08, // Original: 0.12
    rotationRange: 2, 
    sineAmplitude: 0, 
    sineFrequency: 0.5, 
    moverBlendMode: false, 
    revealEase: "sine.inOut" 
  },
  cfg2: { 
    rows: 9, 
    cols: 12, 
    layers: 3, 
    stepDuration: 0.3, // Original: 0.4
    stepInterval: 0.04, // Original: 0.06
    moverPauseBeforeExit: 0.09, // Original: 0.12
    rotationRange: 12, 
    sineAmplitude: 32, 
    sineFrequency: 0.9, 
    moverBlendMode: "screen", 
    revealEase: "power2.inOut" 
  },
  cfg3: { 
    rows: 12, 
    cols: 16, 
    layers: 4, 
    stepDuration: 0.35, // Original: 0.48
    stepInterval: 0.05, // Original: 0.07
    moverPauseBeforeExit: 0.1, // Original: 0.15
    rotationRange: 24, 
    sineAmplitude: 72, 
    sineFrequency: 1.2, 
    moverBlendMode: "screen", 
    revealEase: "power3.inOut" 
  },
};

export function animateTransitionGrid(panelImg, imgURL, cfg, onDone) {
  const {
    rows = 6,
    cols = 8,
    layers = 2,
    stepDuration = 0.35,
    stepInterval = 0.05,
    moverPauseBeforeExit = 0.12,
    rotationRange = 8,
    sineAmplitude = 32,
    sineFrequency = 1,
    moverBlendMode = false,
  } = cfg;

  const parent = panelImg.parentNode;
  parent.querySelectorAll(".mover").forEach(m => m.remove());

  let total = rows * cols * layers;
  let done = 0;

  for (let layer = 0; layer < layers; layer++) {
    for (let row = 0; row < rows; row++) {
      for (let col = 0; col < cols; col++) {
        const mover = document.createElement("div");
        mover.className = "mover";
        mover.style.position = "absolute";
        mover.style.inset = "0";
        mover.style.backgroundImage = `url(${imgURL})`;
        mover.style.backgroundSize = `${cols * 100 / (cols - 1)}% ${rows * 100 / (rows - 1)}%`;
        mover.style.backgroundPosition = `${(col / (cols - 1)) * 100}% ${(row / (rows - 1)) * 100}%`;

        // Clip path per cell
        const top = (row * (100 / rows)).toFixed(4);
        const bottom = (100 - (row + 1) * (100 / rows)).toFixed(4);
        const left = (col * (100 / cols)).toFixed(4);
        const right = (100 - (col + 1) * (100 / cols)).toFixed(4);
        mover.style.clipPath = `inset(${top}% ${right}% ${bottom}% ${left}%)`;

        // Layer depth effect
        mover.style.zIndex = layer;
        mover.style.opacity = 0.9 - layer * 0.2;

        if (moverBlendMode) mover.style.mixBlendMode = moverBlendMode;

        parent.appendChild(mover);

        const stagger = row * stepInterval + col * (stepInterval / 2) + layer * 0.05 + moverPauseBeforeExit;

        gsap.fromTo(
          mover,
          { opacity: mover.style.opacity, x: 0, y: 0, rotate: gsap.utils.random(-rotationRange, rotationRange) },
          {
            opacity: 0,
            x: 80 + col * 10 + layer * 5,
            y: sineAmplitude * Math.sin(sineFrequency * row) + layer * 5,
            rotate: gsap.utils.random(-rotationRange, rotationRange),
            duration: stepDuration * 2.8,
            delay: stagger,
            ease: "power1.out",
            onComplete: () => {
              done++;
              if (done === total && typeof onDone === "function") {
                panelImg.style.backgroundImage = `url(${imgURL})`;
                onDone();
              }
            },
          }
        );
      }
    }
  }
}
