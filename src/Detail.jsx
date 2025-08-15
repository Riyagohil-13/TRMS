import React, { useEffect, useLayoutEffect, useRef, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import gsap from "gsap";
import { sections } from "./config";
import { animateTransitionGrid, getClipPathsForDirection, presets } from "./animations";
import "./index.css";

// Read optional overrides from DOM data-attributes
function getOverrides(sectionId, index) {
  const selector = `.grid__item[data-section-id="${sectionId}"]:nth-child(${Number(index) + 1})`;
  const el = document.querySelector(selector);
  if (!el) return {};
  const attrs = el.dataset;
  return {
    clipPathDirection: attrs.clipPathDirection,
    steps: attrs.steps && Number(attrs.steps),
    stepDuration: attrs.stepDuration && Number(attrs.stepDuration),
    stepInterval: attrs.stepInterval && Number(attrs.stepInterval),
    rotationRange: attrs.rotationRange && Number(attrs.rotationRange),
    moverBlendMode: attrs.moverBlendMode,
    sineAmplitude: attrs.sineAmplitude && Number(attrs.sineAmplitude),
    sineFrequency: attrs.sineFrequency && Number(attrs.sineFrequency),
  };
}

export default function Detail() {
  const { sectionId, index } = useParams();
  const navigate = useNavigate();

  const panelRef = useRef(null);
  const panelImgRef = useRef(null);

  const [img, setImg] = useState(null);
  const [cfg, setCfg] = useState(presets.cfg1);
  const [isAnimating, setIsAnimating] = useState(true);

  // Load image & configuration on mount
  useEffect(() => {
    const section = sections.find((s) => s.id === sectionId);
    if (!section) return navigate("/", { replace: true });

    const i = Number(index);
    const image = section.images[i];
    if (!image) return navigate("/", { replace: true });

    setImg(image);

    const overrides = getOverrides(sectionId, index);
    const preset = { ...presets[section.effect] || presets.cfg1, ...overrides };
    setCfg(preset);
  }, [sectionId, index, navigate]);

  // Animate tiles
  useLayoutEffect(() => {
    if (!img || !panelRef.current || !panelImgRef.current) return;

    gsap.set(panelRef.current, { opacity: 1, pointerEvents: "auto" });

    // Animate repeating tiles
    animateTransitionGrid(panelImgRef.current, img.src, cfg, () => {
      const clip = getClipPathsForDirection(cfg.clipPathDirection || "top-bottom");
      gsap.fromTo(
        panelImgRef.current,
        { clipPath: clip.hide },
        {
          clipPath: clip.reveal,
          duration: cfg.stepDuration * 2.2,
          ease: cfg.revealEase || "sine.inOut",
          onComplete: () => setIsAnimating(false),
        }
      );
    });

    // Add title overlays on each tile
    const movers = panelRef.current.querySelectorAll(".mover");
    movers.forEach((m) => {
      const titleEl = document.createElement("div");
      titleEl.className = "mover__title";
      titleEl.textContent = img.title;
      titleEl.style.position = "absolute";
      titleEl.style.bottom = "5px";
      titleEl.style.left = "5px";
      titleEl.style.color = "#fff";
      titleEl.style.fontSize = "0.7rem";
      titleEl.style.pointerEvents = "none";
      m.appendChild(titleEl);
    });

    // Cleanup tiles on unmount
    return () => {
      if (panelRef.current) {
        panelRef.current.querySelectorAll(".mover").forEach((m) => m.remove());
      }
    };
  }, [img, cfg]);

  // Close panel
  const onClose = () => {
    if (isAnimating) return;
    gsap.to(panelRef.current, { opacity: 0, duration: 0.35, onComplete: () => navigate(-1) });
  };

  if (!img) return null;

  return (
    <figure className="panel panel--right" ref={panelRef} role="img" aria-labelledby="caption">
      <div className="panel__img" ref={panelImgRef} />
      <figcaption className="panel__content" id="caption">
        <h3>{img.title}</h3>
        <p>Model: {img.model}</p>
        <button className="panel__close" onClick={onClose}>
          Close
        </button>
      </figcaption>
    </figure>
  );
}
