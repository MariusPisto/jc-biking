const phrases = [
  "On m'a dit que c'est + rapide avec des textes...",
  "1 km en vélo, ça use, ça use...",
  "Une marche à pied, un vélo, une RedBull !",
  "Si c'est long, vous avez sûrement une mauvaise connexion 🥸",
];

const phrasesContainer = document.getElementById("loading-phrases");
let lastPhraseIndex = -1;
let intervalId;

function showNextPhrase() {
  let randomIndex;
  if (phrases.length > 1) {
    do {
      randomIndex = Math.floor(Math.random() * phrases.length);
    } while (randomIndex === lastPhraseIndex);
  } else {
    randomIndex = 0;
  }

  phrasesContainer.textContent = phrases[randomIndex];
  lastPhraseIndex = randomIndex;
}

function startPhrases() {
  if (intervalId) {
    clearInterval(intervalId);
  }
  showNextPhrase();
  intervalId = setInterval(showNextPhrase, 2000);
}

function stopPhrases() {
  clearInterval(intervalId);
  phrasesContainer.textContent = "";
}

const loader = document.getElementById("loader");

const observer = new MutationObserver((mutations) => {
  mutations.forEach((mutation) => {
    if (mutation.type === "attributes" && mutation.attributeName === "class") {
      if (loader.classList.contains("hidden")) {
        stopPhrases();
      } else {
        startPhrases();
      }
    }
  });
});

observer.observe(loader, {
  attributes: true,
});
