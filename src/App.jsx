import React from 'react';
import { images } from './config';
import './index.css';

export default function App() {
  return (
    <main className="grid">
      {images.map((img, i) => (
        <figure
          key={i}
          className="grid__item"
          role="img"
          aria-labelledby={`caption${i}`}
          onClick={() => window.location.href = `/detail/${i}`} // New page
        >
          <div
            className="grid__item-image"
            style={{ backgroundImage: `url(${img.src})` }}
          />
          <figcaption className="grid__item-caption" id={`caption${i}`}>
            <h3>{img.title}</h3>
            <p>Model: {img.model}</p>
          </figcaption>
        </figure>
      ))}
    </main>
  );
}
