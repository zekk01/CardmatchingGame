# 🎴 Card Matching Game 🎴

## Overview
This project is a card-matching game developed in Unity. The game allows players to flip and match pairs of cards within a grid layout. The game features various layouts, sound effects, a scoring system, and optimized performance through the use of object pooling for card management.

## Features
- **Various Card Layouts:** Supports multiple grid layouts (e.g., 2x2, 2x3, 3x4, 4x5, 5x6).
- **Scoring System:** Players earn points by matching pairs of cards. The score resets at the start of each game.
- **Sound Effects:** Includes sound effects for card flipping, matches, mismatches, and game over events.
- **Object Pooling:** Utilizes a card pooling mechanism to optimize performance by reusing card objects.
- **User Interface:** Displays the current score and allows layout selection via an Inspector dropdown.

## Setup and Installation
Clone the Repository:


## Open in Unity:

Open Unity Hub and add the cloned project.
Open the project in Unity Editor.
Configure Scenes:

Ensure the main game scene is added to the Build Settings. (Scenes Folder -> Sample Scene)
Run the Game:

Press the Play button in Unity Editor to start the game.

## How to Play
Select Layout:

Use the Inspector to select the desired card layout from the CardLayout dropdown.
Start the Game:

Click the Play button in Unity Editor to start the game with the selected layout.
Match Cards:

Click on cards to flip them. Match pairs of cards to earn points.
The game automatically evaluates matches and updates the score.
Game Over:

The game ends when all pairs are matched. The score is reset for the next game.

## Code Structure
GameController.cs: Manages the game logic, card initialization, and scoring system.
Card.cs: Handles individual card behavior, including flipping and matching.
SaveLoadSystem.cs: Manages saving and loading game progress (e.g., score).
Audio Management: Plays sound effects for various game events.

## Detailed Class Descriptions
**GameController**
Instance: Singleton instance of the GameController.
InitializeCardsByLayout(): Initializes the game with the selected card layout.
HandleCardFlip(Card card): Handles the logic for flipping a card.
EvaluateCardMatch(): Coroutine that evaluates if two flipped cards are a match.
HandleMatch(): Handles the logic when two cards match.
HandleMismatch(): Handles the logic when two cards do not match.
InitializeCards(int rows, int columns): Initializes the card grid based on rows and columns.
ClearExistingCards(): Clears existing cards and adds them to the card pool.
GetCardFromPool(): Retrieves a card from the pool or instantiates a new one.
PlaySound(AudioClip clip): Plays the specified sound clip.
UpdateScoreDisplay(): Updates the score display on the UI.
ResetScore(): Resets the score to zero.
SaveProgress(): Saves the current score using SaveLoadSystem.

**Card**
Flip(): Flips the card to show its front or back.
MarkAsMatched(): Marks the card as matched.
SetFrontSprite(Sprite frontSprite): Sets the sprite for the front of the card.
GetFrontSprite(): Returns the sprite of the card's front.
CanFlip(): Checks if the card can be flipped.


**SaveLoadSystem**
SaveProgress(int score): Saves the current score to PlayerPrefs.
LoadProgress(): Loads the saved score from PlayerPrefs.

## Note: 
"Ensure smooth gameplay with animations for card flipping and matching. The system
should allow continuous card flipping without requiring users to wait for card"
I decided not to implement this task as I believe that the game experience is ruined by multiple clicking in a row, as it removes the memory factor from the game loop. the player can easily press randomly until the achieved combination works (especialy for smaller grids)
as for the implementation of the task, a simple multithreading with the main thread running all Unity API calls and the other threads to preform the non Unity API tasks such as checking car matches. (a thread to evaluate card matches while allowing continuous card flipping on the main thread.)


## Contact
For any questions or feedback, please contact [zainkassem7@gmail.com].



