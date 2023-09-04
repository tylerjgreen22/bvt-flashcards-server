import { Flashcard } from "./flashcard";

// Set model
export interface FlashcardSet {
  id?: string;
  title: string;
  description: string;
  appUser?: string;
  cardCount?: number;
  flashcards?: Flashcard[] | undefined;
}
