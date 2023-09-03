import { useEffect, useState } from "react";
import agent from "../api/agent";
import * as Yup from "yup";
import { FlashcardSet } from "../models/flashcardSet";
import { Flashcard } from "../models/flashcard";
import { router } from "./Routes";
import { ErrorMessage, Field, FieldArray, Form, Formik } from "formik";
import AddIcon from "@mui/icons-material/Add";
import ControlPointIcon from "@mui/icons-material/ControlPoint";
import DeleteIcon from "@mui/icons-material/Delete";
import Picture from "../models/picture";
import { useParams } from "react-router-dom";
import { useUserContext } from "../context/UserContext";
import FormLoadingSkeleton from "../components/Loading/FormLoadingSkeleton";
import CircularProgress from "@mui/material/CircularProgress";

const Edit = () => {
  const { id } = useParams();
  const [exisitingSet, setExisitingSet] = useState<FlashcardSet>();
  const { user } = useUserContext();
  const [selectedFlashcard, setSelectedFlashcard] = useState<number | null>(
    null
  );
  const [addDesc, setAddDesc] = useState(false);
  const [loading, setLoading] = useState(false);
  const [loadingSubmit, setLoadingSubmit] = useState(false);

  // Fetch user data for pictures and flashcard data for editing
  useEffect(() => {
    const fetchData = async () => {
      setLoading(true);
      if (id) {
        const data = await agent.Set.detail(id);
        setExisitingSet(data);
      }
      setLoading(false);
    };

    fetchData();
  }, [id]);

  // Initial form values
  let initialValues: {
    title: string;
    description: string;
    flashcards: Flashcard[] | undefined;
  } = {
    title: "",
    description: "",
    flashcards: [],
  };

  // Once the set comes in, populate form initial values with set information
  if (exisitingSet) {
    initialValues = {
      title: exisitingSet.title,
      description: exisitingSet.description,
      flashcards: exisitingSet.flashcards as Flashcard[],
    };
  }

  // Validation schema, checks to make sure title, and flashcard terms and definitions are populated
  const validationSchema = Yup.object({
    title: Yup.string().required("Title is required"),
    flashcards: Yup.array().of(
      Yup.object().shape({
        term: Yup.string().required("Term is required"),
        definition: Yup.string().required("Definition is required"),
        pictureUrl: Yup.string(),
      })
    ),
  });

  // On form submit, create a set object from the form values and submit to the API, then navigate to the Library page
  const handleSubmit = async (values: FlashcardSet) => {
    setLoadingSubmit(true);
    try {
      const set: FlashcardSet = {
        id: exisitingSet?.id,
        title: values.title,
        description: values.description || "",
        flashcards: values.flashcards?.map((flashcard: Flashcard) => ({
          id: flashcard.id ? flashcard.id : null,
          term: flashcard.term,
          definition: flashcard.definition,
          pictureUrl: flashcard.pictureUrl || "",
        })) as Flashcard[],
      };

      await agent.Set.update(set);
      setLoadingSubmit(false);

      router.navigate("/Library");
    } catch (error) {
      setLoadingSubmit(false);
    }
  };

  if (loading) {
    return (
      <div className="max-w-[1100px] mx-auto mt-6 p-2">
        <FormLoadingSkeleton />
      </div>
    );
  } else {
    return (
      <div className="max-w-[1100px] mx-auto mt-6 p-2">
        {/* Page title  */}
        <h2 className=" text-white text-5xl mb-8">Edit Deck</h2>

        <Formik
          enableReinitialize
          initialValues={initialValues}
          validationSchema={validationSchema}
          onSubmit={handleSubmit}
        >
          {({ values, setFieldValue }) => (
            <Form>
              {/* Title and description */}
              <div className="flex flex-col gap-4 mb-6">
                {/* Title */}
                <div>
                  <Field
                    type="text"
                    id="title"
                    placeholder="Enter a title for your Deck"
                    name="title"
                    className="block w-full py-4 pl-4 text-md text-secondary placeholder-secondary bg-violet-900 rounded-md shadow-[inset_0_0px_4px_rgba(0,0,0,0.6)]"
                  />
                  <button
                    type="button"
                    className="editColor pr-3 flex items-center gap-3 rounded-full"
                    onClick={() => setAddDesc(!addDesc)}
                  >
                    <div className="bg-violet-900 w-fit rounded-full p-1 flex justify-center items-center">
                      <AddIcon className="text-white" />
                    </div>
                    <p>Edit Set Description</p>
                  </button>
                  <ErrorMessage
                    name="title"
                    component="div"
                    className="text-red-500 bg-secondary w-fit mt-2 p-2 px-4 rounded-md"
                  />
                </div>

                {/* Description */}
                {addDesc && (
                  <Field
                    type="text"
                    id="description"
                    name="description"
                    placeholder="Enter a description for your set"
                    className="block w-full py-4 pl-4 text-md text-secondary placeholder-secondary bg-violet-900 rounded-md shadow-[inset_0_0px_4px_rgba(0,0,0,0.6)]"
                  />
                )}
              </div>

              {/* Flashcards */}
              <FieldArray
                name="flashcards"
                render={({
                  push,
                  remove,
                }: {
                  push: (value: Flashcard) => void;
                  remove: (index: number) => void;
                }) => (
                  <div>
                    {values.flashcards?.map((flashcard: Flashcard, index) => (
                      <div className="mb-6" key={flashcard.id}>
                        <div className="flex justify-between bg-violet-700 text-white rounded-t-md p-1">
                          {/* Flashcard title */}
                          <h3>Flashcard {index + 1}</h3>

                          {/* Delete flashcard */}
                          <button type="button" onClick={() => remove(index)}>
                            <DeleteIcon />
                          </button>
                        </div>

                        <div className="editCard flex justify-between items-center rounded-b-md p-4">
                          {/* Term */}
                          <div className="w-1/3">
                            <Field
                              type="text"
                              name={`flashcards[${index}].term`}
                              className="editField p-3 w-full"
                            />
                            <label
                              className="block text-secondary"
                              htmlFor={`flashcards[${index}].term`}
                            >
                              Term
                            </label>
                            <ErrorMessage
                              name={`flashcards[${index}].term`}
                              component="div"
                              className="text-red-500 bg-secondary w-fit mt-2 p-2 px-4 rounded-md"
                            />
                          </div>

                          {/* Definition  */}
                          <div className="w-1/3">
                            <Field
                              type="text"
                              name={`flashcards[${index}].definition`}
                              className="editField p-3 w-full"
                            />
                            <label
                              className="block text-secondary"
                              htmlFor={`flashcards[${index}].definition`}
                            >
                              Definition
                            </label>
                            <ErrorMessage
                              name={`flashcards[${index}].definition`}
                              component="div"
                              className="text-red-500 bg-secondary w-fit mt-2 p-2 px-4 rounded-md"
                            />
                          </div>

                          {/* Picture */}
                          <div>
                            <button
                              type="button"
                              className="cursor-pointer"
                              onClick={() => {
                                selectedFlashcard === index
                                  ? setSelectedFlashcard(null)
                                  : setSelectedFlashcard(index);
                              }}
                            >
                              <svg
                                width="64px"
                                height="64px"
                                viewBox="0 0 24 24"
                                fill="none"
                                xmlns="http://www.w3.org/2000/svg"
                              >
                                <g id="SVGRepo_bgCarrier" stroke-width="0"></g>
                                <g
                                  id="SVGRepo_tracerCarrier"
                                  stroke-linecap="round"
                                  stroke-linejoin="round"
                                ></g>
                                <g id="SVGRepo_iconCarrier">
                                  <path
                                    opacity="0.5"
                                    d="M21.9998 12.6978C21.9983 14.1674 21.9871 15.4165 21.9036 16.4414C21.8067 17.6308 21.6081 18.6246 21.1636 19.45C20.9676 19.814 20.7267 20.1401 20.4334 20.4334C19.601 21.2657 18.5405 21.6428 17.1966 21.8235C15.8835 22 14.2007 22 12.0534 22H11.9466C9.79929 22 8.11646 22 6.80345 21.8235C5.45951 21.6428 4.39902 21.2657 3.56664 20.4334C2.82871 19.6954 2.44763 18.777 2.24498 17.6376C2.04591 16.5184 2.00949 15.1259 2.00192 13.3967C2 12.9569 2 12.4917 2 12.0009V11.9466C1.99999 9.79929 1.99998 8.11646 2.17651 6.80345C2.3572 5.45951 2.73426 4.39902 3.56664 3.56664C4.39902 2.73426 5.45951 2.3572 6.80345 2.17651C7.97111 2.01952 9.47346 2.00215 11.302 2.00024C11.6873 1.99983 12 2.31236 12 2.69767C12 3.08299 11.6872 3.3952 11.3019 3.39561C9.44749 3.39757 8.06751 3.41446 6.98937 3.55941C5.80016 3.7193 5.08321 4.02339 4.5533 4.5533C4.02339 5.08321 3.7193 5.80016 3.55941 6.98937C3.39683 8.19866 3.39535 9.7877 3.39535 12C3.39535 12.2702 3.39535 12.5314 3.39567 12.7844L4.32696 11.9696C5.17465 11.2278 6.45225 11.2704 7.24872 12.0668L11.2392 16.0573C11.8785 16.6966 12.8848 16.7837 13.6245 16.2639L13.9019 16.0689C14.9663 15.3209 16.4064 15.4076 17.3734 16.2779L20.0064 18.6476C20.2714 18.091 20.4288 17.3597 20.5128 16.3281C20.592 15.3561 20.6029 14.1755 20.6044 12.6979C20.6048 12.3126 20.917 12 21.3023 12C21.6876 12 22.0002 12.3125 21.9998 12.6978Z"
                                    fill="#000000"
                                  ></path>
                                  <path
                                    fill-rule="evenodd"
                                    clip-rule="evenodd"
                                    d="M17.5 11C15.3787 11 14.318 11 13.659 10.341C13 9.68198 13 8.62132 13 6.5C13 4.37868 13 3.31802 13.659 2.65901C14.318 2 15.3787 2 17.5 2C19.6213 2 20.682 2 21.341 2.65901C22 3.31802 22 4.37868 22 6.5C22 8.62132 22 9.68198 21.341 10.341C20.682 11 19.6213 11 17.5 11ZM19.7121 4.28794C20.096 4.67187 20.096 5.29434 19.7121 5.67826L19.6542 5.7361C19.5984 5.7919 19.5205 5.81718 19.4428 5.80324C19.3939 5.79447 19.3225 5.77822 19.2372 5.74864C19.0668 5.68949 18.843 5.5778 18.6326 5.36742C18.4222 5.15704 18.3105 4.93324 18.2514 4.76276C18.2218 4.67751 18.2055 4.60607 18.1968 4.55721C18.1828 4.47953 18.2081 4.40158 18.2639 4.34578L18.3217 4.28794C18.7057 3.90402 19.3281 3.90402 19.7121 4.28794ZM17.35 8.0403C17.2057 8.18459 17.1336 8.25673 17.054 8.31878C16.9602 8.39197 16.8587 8.45472 16.7512 8.50591C16.6602 8.54932 16.5634 8.58158 16.3698 8.64611L15.349 8.98639C15.2537 9.01814 15.1487 8.99335 15.0777 8.92234C15.0067 8.85134 14.9819 8.74631 15.0136 8.65104L15.3539 7.63021C15.4184 7.43662 15.4507 7.33983 15.4941 7.24876C15.5453 7.14133 15.608 7.0398 15.6812 6.94596C15.7433 6.86642 15.8154 6.79427 15.9597 6.65L17.7585 4.85116C17.802 4.80767 17.8769 4.82757 17.8971 4.88568C17.9707 5.09801 18.109 5.37421 18.3674 5.63258C18.6258 5.89095 18.902 6.02926 19.1143 6.10292C19.1724 6.12308 19.1923 6.19799 19.1488 6.24148L17.35 8.0403Z"
                                    fill="#000000"
                                  ></path>
                                </g>
                              </svg>
                            </button>
                          </div>
                        </div>

                        {selectedFlashcard === index && (
                          <div className="bg-violet-900 p-4 rounded-b-md w-[98%] mx-auto">
                            {/* Picture URL */}
                            <div className="w-2/3">
                              <Field
                                type="text"
                                name={`flashcards[${index}].pictureUrl`}
                                className="editField p-3 w-full"
                              />
                              <label
                                className="block text-secondary"
                                htmlFor={`flashcards[${index}].pictureUrl`}
                              >
                                Picture URL
                              </label>
                            </div>

                            {/* Picture Gallery  */}
                            <div className="mt-8">
                              <p className="text-secondary mb-4">
                                Choose one of your uploaded pictures or use an
                                image URL from the internet
                              </p>
                              <div className="flex gap-4">
                                {user?.pictures.map((picture: Picture) => (
                                  <img
                                    key={picture.id}
                                    src={picture.url}
                                    className="w-1/6"
                                    onClick={() => {
                                      setFieldValue(
                                        `flashcards[${index}].pictureUrl`,
                                        picture.url
                                      );
                                    }}
                                  />
                                ))}
                              </div>
                            </div>
                          </div>
                        )}
                      </div>
                    ))}

                    {/* Add flashcard */}
                    <div className="editCard rounded-md">
                      <button
                        type="button"
                        onClick={() =>
                          push({
                            term: "",
                            definition: "",
                            pictureUrl: "",
                          })
                        }
                        className="flex mx-auto py-12"
                      >
                        <div className="mr-1">
                          <ControlPointIcon />
                        </div>
                        <p className="text-lg">Add Card</p>
                      </button>
                    </div>
                  </div>
                )}
              />

              {/* Submit button */}
              <div className="flex justify-end mt-4">
                <button
                  className="bg-violet-700 px-4 py-2 rounded-xl text-secondary"
                  type="submit"
                >
                  {loadingSubmit ? <CircularProgress size={30} /> : "Save"}
                </button>
              </div>
            </Form>
          )}
        </Formik>
      </div>
    );
  }
};

export default Edit;
