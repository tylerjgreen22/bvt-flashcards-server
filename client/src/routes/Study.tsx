import { useEffect, useState } from "react";
import { FlashcardSet } from "../models/flashcardSet";
import { useParams } from "react-router-dom";
import agent from "../api/agent";
import { Flashcard } from "../models/flashcard";
import ArrowBackIcon from "@mui/icons-material/ArrowBack";
import ArrowForwardIcon from "@mui/icons-material/ArrowForward";
import LinearProgress from "@mui/material/LinearProgress";
import IosShareIcon from "@mui/icons-material/IosShare";
import { Avatar } from "@mui/material";
import FlashcardDisplay from "../components/Flashcard/FlashcardDisplay";
import StudyLoadingSkeleton from "../components/Loading/StudyLoadingSkeleton";
import { router } from "./Routes";

// Sutdy page
const Study = () => {
  const { id } = useParams();
  const [flashcardSet, setFlashcardSet] = useState<FlashcardSet>();
  const [selectedFlashcard, setSelectedFlashcard] = useState<Flashcard>();
  const [flashcards, setFlashcards] = useState<Flashcard[]>();
  const [currentCards, setCurrentCards] = useState<Flashcard[]>();
  const [start, setStart] = useState(0);
  const [end, setEnd] = useState(3);
  const [progress, setProgress] = useState(0);
  const [loading, setLoading] = useState(false);

  // Fetch the set to study
  useEffect(() => {
    const fetchData = async () => {
      setLoading(true);
      if (id) {
        const setData = await agent.Set.detail(id);
        setFlashcardSet(setData);
        const flashcardsData = setData.flashcards;
        if (flashcardsData) {
          setCurrentCards(flashcardsData.slice(0, 3));
          setSelectedFlashcard(flashcardsData[0]);
          setFlashcards(flashcardsData);
        }
      }
      setLoading(false);
    };

    fetchData();
  }, [id]);

  // On select of a flashcard, set the flashcard to selected and update progress bar
  const handleSelect = (flashcard: Flashcard) => {
    setSelectedFlashcard(flashcard);
    if (flashcards && selectedFlashcard) {
      const currIndex = flashcards?.indexOf(flashcard);
      const totalLength = flashcards?.length;

      if (
        totalLength != undefined &&
        currIndex != undefined &&
        currIndex !== totalLength
      ) {
        setProgress((currIndex / totalLength) * 100);
      }
    }
  };

  // On change cards, increment/decrement the currently displayed cards
  const handleChangeCards = (direction: string) => {
    if (direction === "start" && end !== flashcards?.length) {
      setStart(start + 1);
      setEnd(end + 1);
      setCurrentCards(flashcards?.slice(start + 1, end + 1));
    } else if (direction === "end" && start !== 0) {
      setStart(start - 1);
      setEnd(end - 1);
      setCurrentCards(flashcards?.slice(start - 1, end - 1));
    }
  };

  // On click next button, set the selected flashcard to the next card and update progress bar
  const handleNext = () => {
    if (flashcards && selectedFlashcard) {
      const currIndex = flashcards?.indexOf(selectedFlashcard);
      const totalLength = flashcards?.length;

      if (
        totalLength != undefined &&
        currIndex != undefined &&
        currIndex !== totalLength
      ) {
        setSelectedFlashcard(flashcards[currIndex + 1]);
        setProgress(((currIndex + 1) / totalLength) * 100);
      }
    }
  };

  // On click back button, set the selected flashcard to the previous card and update progress bar
  const handleBack = () => {
    if (flashcards && selectedFlashcard) {
      const currIndex = flashcards?.indexOf(selectedFlashcard);
      const totalLength = flashcards?.length;

      if (
        totalLength != undefined &&
        currIndex != undefined &&
        currIndex !== 0
      ) {
        setSelectedFlashcard(flashcards[currIndex - 1]);
        setProgress(((currIndex - 1) / totalLength) * 100);
      }
    }
  };

  // On reset, set selected flashcard to null, reset progress bar
  const handleReset = (flashcard: Flashcard) => {
    setSelectedFlashcard(flashcard);
    setProgress(0);
  };

  // If loading, return Study skeleton
  if (loading) {
    return (
      <div className="max-w-[1100px] mx-auto">
        <StudyLoadingSkeleton />
      </div>
    );
  }

  return (
    <div className="max-w-[1100px] mx-auto pb-56 md:pb-0">
      {flashcardSet && (
        <div>
          {/* Title  */}
          <h2 className="text-4xl text-secondary text-center my-4">
            {flashcardSet.title}
          </h2>

          {/* Selected flashcard  */}
          {selectedFlashcard ? (
            selectedFlashcard.pictureUrl ? (
              <FlashcardDisplay
                question={selectedFlashcard.term}
                answer={selectedFlashcard.definition}
                image={selectedFlashcard.pictureUrl}
              />
            ) : (
              <FlashcardDisplay
                question={selectedFlashcard.term}
                answer={selectedFlashcard.definition}
              />
            )
          ) : (
            <p className="h-[500px] lg:w-5/6 mx-auto bg-[#2e3856] text-3xl text-secondary font-bold rounded-md p-4 flex justify-center items-center">
              Complete!
            </p>
          )}

          {selectedFlashcard ? (
            <div className="flex justify-between w-5/6 mx-auto mt-4">
              {/* Back  */}
              <button
                className="bg-[#2e3856] py-2 px-4 rounded-md text-secondary"
                onClick={handleBack}
              >
                Back
              </button>
              {/* Next  */}
              <button
                className="bg-[#2e3856] py-2 px-4 rounded-md text-secondary"
                onClick={handleNext}
              >
                Next
              </button>
            </div>
          ) : (
            <div className="flex justify-center mt-4">
              {/* Reset  */}
              <button
                className="bg-[#2e3856] py-2 px-4 rounded-md text-secondary mx-auto"
                onClick={() => flashcards && handleReset(flashcards[0])}
              >
                Reset
              </button>
            </div>
          )}

          {/* Progress bar  */}
          <LinearProgress
            value={progress}
            variant="determinate"
            className="w-5/6 mx-auto mt-4"
          />

          {/* Created By  */}
          <div className="text-secondary w-5/6 mx-auto mt-4 flex justify-between">
            <div className="flex gap-2">
              {/* Avatar */}
              <Avatar
                sx={{ bgcolor: "#ff5722", width: 50, height: 50 }}
                className="cursor-pointer"
              >
                <p className="text-2xl">
                  {flashcardSet.appUser &&
                    flashcardSet.appUser[0].toUpperCase()}
                </p>
              </Avatar>
              <div>
                {/* Username  */}
                <h3 className="text-sm">Created by </h3>
                <h2 className="text-lg">{flashcardSet.appUser}</h2>
              </div>
            </div>
            {/* Clone */}
            <button
              className="bg-accent px-4 pb-1 rounded-md text-secondary flex items-center"
              onClick={() => router.navigate(`/Create/${flashcardSet.id}`)}
            >
              <IosShareIcon fontSize="medium" /> <p className="mt-1">Clone</p>
            </button>
          </div>

          {/* Tablet+ back arrow */}
          {selectedFlashcard ? (
            <div className="flex flex-col md:flex-row gap-4 mt-12 justify-center items-center px-3">
              <button
                type="button"
                onClick={() => handleChangeCards("end")}
                className="hidden md:block"
              >
                <ArrowBackIcon />
              </button>
              {/* Currently displayed cards  */}
              {currentCards?.map((flashcard: Flashcard) => (
                <div
                  key={flashcard.id}
                  className="h-[200px] max-w-[400px] bg-secondary w-full md:w-1/2 flex justify-center items-center text-center rounded-md py-16"
                  onClick={() => handleSelect(flashcard)}
                >
                  {flashcard.term}
                </div>
              ))}
              <div className="flex gap-8">
                {/* Mobile back arrow  */}
                <button
                  type="button"
                  onClick={() => handleChangeCards("end")}
                  className="md:hidden"
                >
                  <ArrowBackIcon />
                </button>
                {/* Forward arrow  */}
                <button
                  type="button"
                  onClick={() => handleChangeCards("start")}
                >
                  <ArrowForwardIcon />
                </button>
              </div>
            </div>
          ) : null}
        </div>
      )}
    </div>
  );
};

export default Study;
