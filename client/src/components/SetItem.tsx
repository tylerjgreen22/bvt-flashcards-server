import { FlashcardSet } from "../models/flashcardSet";
import DeleteIcon from "@mui/icons-material/Delete";
import EditIcon from "@mui/icons-material/Edit";
import { router } from "../routes/Routes";
import { useLocation } from "react-router-dom";

interface SetItemProps {
  flashCardSet: FlashcardSet;
  handleDelete?: (id: string) => void;
}

// Set item rendered for search results and library
const SetItem = (props: SetItemProps) => {
  const location = useLocation();
  const { title, id, cardCount, appUser } = props.flashCardSet;

  // Delete set using function passed as a prop
  const { handleDelete } = props;

  // Navigate to edit page
  const handleEdit = (id: string) => {
    router.navigate(`/Edit/${id}`);
  };

  // Navigate to study page
  const handleStudy = (id: string) => {
    router.navigate(`/Study/${id}`);
  };

  // Search results item (no delete and edit, includes user who created the set)
  if (location.pathname.includes("Results") && id) {
    return (
      <div className="w-full bg-secondary rounded-md p-2 mt-6 hover:border-b-8 hover:border-accent hover:pb-2">
        {/* Set item  */}
        <div className="flex items-center justify-between">
          {/* Card count and set title  */}
          <div>
            <p>
              {cardCount} Card{cardCount != 1 ? "s" : null}
            </p>
            <h3
              className="font-bold text-lg cursor-pointer"
              onClick={() => handleStudy(id)}
            >
              {title}
            </h3>
          </div>

          {/* Created by  */}
          <p className="text-lg">
            Created By: <strong>{appUser}</strong>
          </p>
        </div>
      </div>
    );
  } else if (id) {
    // Library set item, includes delete and edit and omits created by
    return (
      <div className="w-full bg-secondary rounded-md p-2 mt-6 hover:border-b-8 hover:border-accent hover:pb-2">
        {/* Set item  */}
        <div className="group">
          <div className="flex items-center justify-between">
            {/* Card count and set title  */}
            <div>
              <p>
                {cardCount} Card{cardCount != 1 ? "s" : null}
              </p>
              <h3
                className="font-bold text-lg cursor-pointer"
                onClick={() => handleStudy(id)}
              >
                {title}
              </h3>
            </div>

            {/* Edit and delete  */}
            <div className="flex gap-2 items-center opacity-0 group-hover:opacity-100">
              <div className="cursor-pointer" onClick={() => handleEdit(id)}>
                <EditIcon />
              </div>
              <div className="cursor-pointer" onClick={() => handleDelete!(id)}>
                <DeleteIcon />
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  }
};

export default SetItem;
