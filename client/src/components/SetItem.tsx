import { FlashcardSet } from "../models/flashcardSet";
import DeleteIcon from "@mui/icons-material/Delete";
import EditIcon from "@mui/icons-material/Edit";
import { router } from "../routes/Routes";
import { useLocation } from "react-router-dom";

interface SetItemProps {
  flashCardSet: FlashcardSet;
  handleDelete?: (id: string) => void;
}

const SetItem = (props: SetItemProps) => {
  const location = useLocation();
  const { title, id, cardCount, appUser } = props.flashCardSet;

  const { handleDelete } = props;

  const handleEdit = (id: string) => {
    router.navigate(`/Edit/${id}`);
  };

  const handleStudy = (id: string) => {
    router.navigate(`/Study/${id}`);
  };

  if (location.pathname.includes("Results") && id) {
    return (
      <div className="w-full bg-secondary rounded-md p-2 mt-6 hover:border-b-8 hover:border-accent hover:pb-2">
        <div className="group">
          <div className="flex items-center justify-between">
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
            <p className="text-lg">
              Created By: <strong>{appUser}</strong>
            </p>
          </div>
        </div>
      </div>
    );
  } else if (id) {
    return (
      <div className="w-full bg-secondary rounded-md p-2 mt-6 hover:border-b-8 hover:border-accent hover:pb-2">
        <div className="group">
          <div className="flex items-center justify-between">
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
