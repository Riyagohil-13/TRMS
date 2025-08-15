// src/utils.js
export function preloadImages(selector = ".grid__item-image, .panel__img") {
  const els = document.querySelectorAll(selector);
  const urls = [];
  els.forEach((el) => {
    const bg = getComputedStyle(el).backgroundImage;
    const match = bg && bg.match(/url\(["']?(.*?)["']?\)/);
    if (match && match[1]) urls.push(match[1]);
  });

  // also if none found (first load), optionally preload assets from config externally
  const unique = Array.from(new Set(urls));
  return Promise.all(
    unique.map((u) => {
      return new Promise((res) => {
        const img = new Image();
        img.src = u;
        img.onload = img.onerror = res;
      });
    })
  );
}
