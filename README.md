# CornerAwake ⚡

[![Windows](https://img.shields.io/badge/Platform-Windows%2010%20%7C%2011-blue.svg)](https://github.com/RedmanNetwork/CornerAwake)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Size](https://img.shields.io/badge/Taille-~11%20Ko-brightgreen.svg)](https://github.com/RedmanNetwork/CornerAwake)
[![Release](https://img.shields.io/badge/Download-CornerAwake.exe-success.svg)](https://raw.githubusercontent.com/RedmanNetwork/CornerAwake/main/CornerAwake.exe)

> **Empêchez votre PC et votre écran de s'éteindre simplement en garant votre souris dans le coin supérieur gauche de l'écran.**

Fini la mise en veille intempestive pendant que vous révisez, prenez des notes sur papier, suivez une présentation ou regardez une démo.

---

## 🎯 Comment ça marche ?

```
 ┌────────────────────────────────────────────────────────┐
 │ ⚡ [COIN ACTIF]                                        │
 │   Garez votre curseur ici                              │
 │   --> Écran & PC maintenus ALLUMÉS                     │
 │                                                        │
 │                                                        │
 │                                                        │
 │                   Zone normale                         │
 │                   --> Veille Windows habituelle        │
 │                                                        │
 └────────────────────────────────────────────────────────┘
```

1. **Activez l'anti-veille** : Déplacez votre curseur tout en haut à gauche de l'écran.  
   L'icône près de l'horloge devient **verte avec un éclair ⚡** : l'écran et le système restent allumés indéfiniment.
2. **Reprenez la veille normale** : Déplacez simplement la souris hors du coin.  
   L'icône redevient **grise 💤** : vos paramètres d'économie d'énergie habituels reprennent immédiatement.

---

## 🚀 Installation ultra simple (pour vos potes)

### 👉 Option 1 : Téléchargement direct (Recommandé - 2 secondes)
1. Téléchargez directement le fichier [**CornerAwake.exe**](https://raw.githubusercontent.com/RedmanNetwork/CornerAwake/main/CornerAwake.exe).
2. Double-cliquez dessus. C'est tout ! L'application se loge discrètement dans la zone des notifications (près de l'horloge Windows).

*(Optionnel)* : Pour qu'il démarre tout seul à chaque allumage de votre PC, faites un **clic droit sur l'icône** près de l'horloge et cochez **« Démarrer avec Windows »**.

---

### 👉 Option 2 : Installation automatique complète avec raccourci Bureau
1. Téléchargez le projet via le bouton vert **`<> Code` > `Download ZIP`** sur GitHub et extrayez-le.
2. Double-cliquez sur `install.bat`.
3. Le script s'occupe de tout :
   - Installe l'exécutable proprement dans vos programmes locaux (`%LOCALAPPDATA%\CornerAwake`)
   - Crée un raccourci direct sur votre **Bureau**
   - Vous propose de lancer l'application immédiatement

---

## ⚙️ Options disponibles (Clic droit sur l'icône)

Faites un clic droit sur l'icône CornerAwake (près de l'horloge) pour accéder aux réglages :

| Option | Description |
| :--- | :--- |
| **Statut** | Affiche en temps réel : `⚡ Anti-veille ACTIVE` ou `💤 Veille normale` |
| **Taille de la zone** | Ajustez la sensibilité du coin : `20x20 px`, `35x35 px` *(conseillé)*, `50x50 px` |
| **Démarrer avec Windows** | Cochez pour lancer CornerAwake automatiquement au démarrage de votre session |
| **Quitter** | Ferme l'application et réactive instantanément la veille normale |

---

## 💡 Pourquoi CornerAwake est supérieur aux alternatives ?

- 🪶 **Poids plume** : Seulement **~11 Ko**, consomme **0% de processeur** et moins de **10 Mo de RAM**.
- 🔒 **Propre et natif** : Utilise l'API officielle Windows (`SetThreadExecutionState`). Il ne simule **aucun clic parasite**, aucune frappe de touche fantôme (`F15`), et ne perturbe jamais vos applications ouvertes ou vos jeux.
- ⚡ **Aucune modification système permanente** : Dès que vous sortez du coin, votre PC redevient 100% normal.
- 📦 **Zéro dépendance** : Fonctionne nativement sur Windows 10 et Windows 11 sans devoir installer Python, Node.js ou un runtime lourd.

---

## 🛠️ Pour les développeurs : Compiler les sources

Si vous préférez compiler vous-même le code source :
1. Clonez ce dépôt :
   ```bash
   git clone https://github.com/RedmanNetwork/CornerAwake.git
   ```
2. Double-cliquez sur `build.bat` (ou lancez-le en invite de commande).
3. Le compilateur C# natif de Windows (`csc.exe`) génère automatiquement un nouveau `CornerAwake.exe` en 1 seconde chrono, sans nécessiter Visual Studio !

---

## 🗑️ Désinstallation

Si vous avez utilisé `install.bat` et souhaitez retirer l'application :
- Double-cliquez simplement sur `uninstall.bat`.  
Il ferme le processus, supprime le raccourci Bureau, nettoie le registre et supprime les fichiers en toute propreté.

---

## 📄 Licence

Ce projet est sous licence [MIT](LICENSE). Vous êtes libre de l'utiliser, le modifier et le partager !
