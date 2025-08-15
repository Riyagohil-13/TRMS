import React, { useEffect } from "react";
import { Link } from "react-router-dom";
import { sections } from "./config";
import "./index.css";

export default function Home() {
  useEffect(() => {
    document.body.classList.remove("loading");
  }, []);

  return (
    <main>
      <header className="frame">
        <h1 className="frame__title">Repeating Image Transition</h1>
        {/* Additional text for the header, aligned as per image */}
        <div className="frame__top-right-links">
          <a href="#" className="frame__link">more info, code, all demos</a>
          <a href="#" className="frame__link">page-transition, repetition, grid</a>
          <span className="frame__text-right">divi ai: on demand content <br /> creation, code writing & image <br /> generation.</span>
        </div>
      </header>
      {sections.map((section) => (
        <section key={section.id} className="grid-section">
          <div className="heading">
            <h2 className="heading__title">{section.id}</h2>
            {/* Conditional rendering for section descriptions based on section.id */}
            {section.id === "Shane Weber" && (
              <p className="section-description">effect 01: straight linear paths, smooth easing, clean timing, minimal rotation. </p>
            )}
            {section.id === "Manika Jorge" && (
              <p className="section-description">effect 02: Adjusts mover count, rotation, timing, and animation feel.</p>
            )}
            {section.id === "Angela Wong" && (
              <p className="section-description">effect 03: Big arcs, smooth start, powerful snap, slow reveal.</p>
            )}
            {/* Add more conditions for other sections if needed */}
          </div>

          <div className="grid">
            {section.images.map((img, idx) => (
              <figure className="grid__item" key={idx}>
                <Link
                  to={`/detail/${encodeURIComponent(section.id)}/${idx}`}
                  className="grid__link"
                >
                  <div
                    className="grid__item-image"
                    style={{ backgroundImage: `url(${img.src})` }}
                  />
                  <figcaption className="grid__item-caption">
                 
                    <h3><strong>{img.title}</strong></h3>
                    <p>{img.model}</p>
                  </figcaption>
                </Link>
              </figure>
            ))}
          </div>
        </section>
      ))}
    </main>
  );
}
