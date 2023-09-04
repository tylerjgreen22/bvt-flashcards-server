import "./Flashcard.css";
import { CSSTransition } from "react-transition-group";
import { useState } from "react";

type Flashcard = {
  question: string;
  answer: string;
  image?: string;
};

// Flashcard with flip animation for main card on Study page
const FlashcardDisplay = (props: Flashcard) => {
  const [showFront, setShowFront] = useState(true);
  const { question, answer } = props;

  return (
    <div className="h-[300px] lg:h-[500px] p-3 lg:w-5/6 mx-auto ">
      {/* Transition for flip animation  */}
      <CSSTransition in={showFront} timeout={300} classNames="flip">
        <div
          className="card"
          onClick={() => {
            setShowFront((prev) => !prev);
          }}
        >
          {/* Back of card */}
          <div className="card back bg-[#2e3856] text-3xl text-secondary font-bold rounded-md p-4">
            {answer}
          </div>

          {/* Front of card */}
          <div className="card front bg-[#2e3856] text-3xl text-secondary font-bold rounded-md p-4">
            <div className="content-container">
              <h1 className="text-center">{question}</h1>
              {props.image ? (
                <img className="w-1/2 mx-auto mt-3" src={props.image} />
              ) : null}
            </div>
          </div>
        </div>
      </CSSTransition>
    </div>
  );
};

export default FlashcardDisplay;
