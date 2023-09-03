import { Flashcard } from "./flashcard";

export interface FlashcardSet {
  id?: string;
  title: string;
  description: string;
  appUser?: string;
  cardCount?: number;
  flashcards?: Flashcard[] | undefined;
}
