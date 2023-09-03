/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable react-refresh/only-export-components */
import { useState, useEffect } from "react";
import agent from "../api/agent";
import { FlashcardSet } from "../models/flashcardSet";
import { v4 as uuid } from "uuid";
import { Formik, Field, ErrorMessage, FieldArray, Form } from "formik";
import { useParams } from "react-router-dom";
import { Flashcard } from "../models/flashcard";
import { router } from "./Routes";
import * as Yup from "yup";
import { User } from "../models/user";

interface FormikProps {
  push: (value: Flashcard) => void;
  remove: (index: number) => void;
}

const Study = () => {
  const { id } = useParams();
  const [set, setSet] = useState<FlashcardSet>();

  useEffect(() => {
    const fetchData = async () => {
      if (id) {
        const setData = await agent.Set.detail(id);
        setSet(setData);
      }
    };

    fetchData();
  }, [id]);

  return (
    <>
      {set &&
        set.flashcards &&
        set?.flashcards.map((flashcard) => (
          <>
            <p>{flashcard.term}</p>
            <img src={flashcard.pictureUrl} />
          </>
        ))}
    </>
  );
};

const Create = () => {
  const [user, setUser] = useState<User | null>(null);
  const [selectedFlashcard, setSelectedFlashcard] = useState<number>();

  useEffect(() => {
    const fetchData = async () => {
      const data = await agent.Account.current();
      setUser(data);
    };

    fetchData();
  }, []);

  const setId = uuid();

  const initialValues = {
    id: setId,
    title: "",
    description: "",
    flashcards: [],
  };

  const validationSchema = Yup.object({
    title: Yup.string().required("Title is required"),
    flashcards: Yup.array().of(
      Yup.object().shape({
        term: Yup.string().required("Term is required"),
        definition: Yup.string().required("Definition is required"),
        pictureUrl: Yup.string(),
        setId: Yup.string().required("Set Id is required"),
      })
    ),
  });

  const handleSubmit = async (values: FlashcardSet) => {
    const set = {
      id: values.id,
      title: values.title,
      description: values.description || "",
      flashcards: values.flashcards?.map((flashcard: Flashcard) => ({
        id: flashcard.id,
        term: flashcard.term,
        definition: flashcard.definition,
        pictureUrl: flashcard.pictureUrl || "",
        setId,
      })),
    };

    await agent.Set.create(set);

    router.navigate("/Library");
  };

  return (
    <div>
      <h1>Create</h1>
      <Formik
        initialValues={initialValues}
        validationSchema={validationSchema}
        onSubmit={handleSubmit}
      >
        {({ values, setFieldValue }) => (
          <Form>
            <div>
              <label className="block" htmlFor="title">
                Title
              </label>
              <Field type="text" id="title" name="title" />
              <ErrorMessage name="title" component="div" />
            </div>

            <div>
              <label className="block" htmlFor="description">
                Description
              </label>
              <Field type="text" id="description" name="description" />
            </div>

            <div>
              <h2>Flashcards</h2>
              <FieldArray
                name="flashcards"
                render={({ push, remove }: FormikProps) => (
                  <div>
                    {values.flashcards?.map((flashcard: Flashcard, index) => (
                      <div key={flashcard.id} className="">
                        <h3>Flashcard {index + 1}</h3>
                        <div>
                          <label
                            className="block"
                            htmlFor={`flashcards[${index}].term`}
                          >
                            Term
                          </label>
                          <Field
                            type="text"
                            name={`flashcards[${index}].term`}
                          />
                          <ErrorMessage
                            name={`flashcards[${index}].term`}
                            component="div"
                          />
                        </div>

                        <div>
                          <label
                            className="block"
                            htmlFor={`flashcards[${index}].definition`}
                          >
                            Definition
                          </label>
                          <Field
                            type="text"
                            name={`flashcards[${index}].definition`}
                          />
                          <ErrorMessage
                            name={`flashcards[${index}].definition`}
                            component="div"
                          />
                        </div>

                        <div>
                          <p
                            className="cursor-pointer"
                            onClick={() => {
                              setSelectedFlashcard(index);
                            }}
                          >
                            Add Picture
                          </p>
                          {selectedFlashcard === index && (
                            <>
                              <label
                                className="block"
                                htmlFor={`flashcards[${index}].pictureUrl`}
                              >
                                Picture URL
                              </label>
                              <Field
                                type="text"
                                name={`flashcards[${index}].pictureUrl`}
                              />
                              <ErrorMessage
                                name={`flashcards[${index}].pictureUrl`}
                                component="div"
                              />
                              {user &&
                                user.pictures.map((picture, pictureIndex) => (
                                  <img
                                    key={pictureIndex}
                                    src={picture.url}
                                    className="w-1/4 h-1/4"
                                    onClick={() => {
                                      setFieldValue(
                                        `flashcards[${index}].pictureUrl`,
                                        picture.url
                                      );
                                    }}
                                  />
                                ))}
                            </>
                          )}
                        </div>

                        <div>
                          <button type="button" onClick={() => remove(index)}>
                            Remove Flashcard
                          </button>
                        </div>
                      </div>
                    ))}

                    <button
                      type="button"
                      onClick={() =>
                        push({
                          id: uuid(),
                          term: "",
                          definition: "",
                          pictureUrl: "",
                          setId,
                        })
                      }
                    >
                      Add Flashcard
                    </button>
                  </div>
                )}
              />
            </div>

            <button type="submit">Submit</button>
          </Form>
        )}
      </Formik>
    </div>
  );
};

const Edit = () => {
  const [exisitingSet, setExisitingSet] = useState<FlashcardSet>();
  const { id } = useParams();
  const [user, setUser] = useState<User | null>(null);
  const [selectedFlashcard, setSelectedFlashcard] = useState<number>();

  useEffect(() => {
    const fetchData = async () => {
      if (id) {
        const data = await agent.Set.detail(id);
        const userData = await agent.Account.current();
        setExisitingSet(data);
        setUser(userData);
      }
    };

    fetchData();
  }, [id]);

  let initialValues: FlashcardSet = {
    id: "",
    title: "",
    description: "",
    flashcards: [],
  };

  if (exisitingSet) {
    initialValues = {
      id: exisitingSet.id,
      title: exisitingSet.title,
      description: exisitingSet.description,
      flashcards: exisitingSet.flashcards,
    };
  }

  const validationSchema = Yup.object({
    title: Yup.string().required("Title is required"),
    flashcards: Yup.array().of(
      Yup.object().shape({
        term: Yup.string().required("Term is required"),
        definition: Yup.string().required("Definition is required"),
        pictureUrl: Yup.string(),
        setId: Yup.string().required("Set Id is required"),
      })
    ),
  });

  const handleSubmit = async (values: FlashcardSet) => {
    const set = {
      id: values.id,
      title: values.title,
      description: values.description || "",
      Flashcards: values.flashcards?.map((flashcard: Flashcard) => ({
        id: flashcard.id,
        term: flashcard.term,
        definition: flashcard.definition,
        pictureUrl: flashcard.pictureUrl || "",
        setId: flashcard.setId,
      })),
    };

    await agent.Set.update(set);

    router.navigate("/Library");
  };

  return (
    <div>
      <h1>Create</h1>
      {exisitingSet ? (
        <Formik
          initialValues={initialValues}
          enableReinitialize
          validationSchema={validationSchema}
          onSubmit={handleSubmit}
        >
          {({ values, setFieldValue }) => (
            <Form>
              <div>
                <label className="block" htmlFor="title">
                  Title
                </label>
                <Field type="text" id="title" name="title" />
                <ErrorMessage name="title" component="div" />
              </div>

              <div>
                <label className="block" htmlFor="description">
                  Description
                </label>
                <Field type="text" id="description" name="description" />
                <ErrorMessage name="description" component="div" />
              </div>

              <div>
                <h2>Flashcards</h2>
                <FieldArray
                  name="flashcards"
                  render={({ push, remove }: FormikProps) => (
                    <div>
                      {values.flashcards?.map((flashcard: Flashcard, index) => (
                        <div key={flashcard.id} className="">
                          <h3>Flashcard {index + 1}</h3>
                          <div>
                            <label
                              className="block"
                              htmlFor={`flashcards[${index}].term`}
                            >
                              Term
                            </label>
                            <Field
                              type="text"
                              name={`flashcards[${index}].term`}
                            />
                            <ErrorMessage
                              name={`flashcards[${index}].term`}
                              component="div"
                            />
                          </div>

                          <div>
                            <label
                              className="block"
                              htmlFor={`flashcards[${index}].definition`}
                            >
                              Definition
                            </label>
                            <Field
                              type="text"
                              name={`flashcards[${index}].definition`}
                            />
                            <ErrorMessage
                              name={`flashcards[${index}].definition`}
                              component="div"
                            />
                          </div>

                          <div>
                            <p
                              className="cursor-pointer"
                              onClick={() => {
                                setSelectedFlashcard(index);
                              }}
                            >
                              Add Picture
                            </p>
                            {selectedFlashcard === index && (
                              <>
                                <label
                                  className="block"
                                  htmlFor={`flashcards[${index}].pictureUrl`}
                                >
                                  Picture URL
                                </label>
                                <Field
                                  type="text"
                                  name={`flashcards[${index}].pictureUrl`}
                                />
                                <ErrorMessage
                                  name={`flashcards[${index}].pictureUrl`}
                                  component="div"
                                />
                                {user &&
                                  user.pictures.map((picture, pictureIndex) => (
                                    <img
                                      key={pictureIndex}
                                      src={picture.url}
                                      className="w-1/4 h-1/4"
                                      onClick={() => {
                                        setFieldValue(
                                          `flashcards[${index}].pictureUrl`,
                                          picture.url
                                        );
                                      }}
                                    />
                                  ))}
                              </>
                            )}
                          </div>

                          <div>
                            <button type="button" onClick={() => remove(index)}>
                              Remove Flashcard
                            </button>
                          </div>
                        </div>
                      ))}

                      <button
                        type="button"
                        onClick={() =>
                          push({
                            id: uuid(),
                            term: "",
                            definition: "",
                            pictureUrl: "",
                            setId: exisitingSet.id,
                          })
                        }
                      >
                        Add Flashcard
                      </button>
                    </div>
                  )}
                />
              </div>

              <button type="submit">Submit</button>
            </Form>
          )}
        </Formik>
      ) : null}
    </div>
  );
};

const CreateNoId = () => {
  const [user, setUser] = useState<User | null>(null);
  const [selectedFlashcard, setSelectedFlashcard] = useState<number>();

  useEffect(() => {
    const fetchData = async () => {
      const data = await agent.Account.current();
      setUser(data);
    };

    fetchData();
  }, []);

  const initialValues = {
    title: "",
    description: "",
    flashcards: [],
  };

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

  const handleSubmit = async (values: FlashcardSet) => {
    const set = {
      title: values.title,
      description: values.description || "",
      flashcards: values.flashcards?.map((flashcard: Flashcard) => ({
        term: flashcard.term,
        definition: flashcard.definition,
        pictureUrl: flashcard.pictureUrl || "",
      })),
    };

    await agent.Set.create(set);

    router.navigate("/Library");
  };

  return (
    <div>
      <h1>Create</h1>
      <Formik
        initialValues={initialValues}
        validationSchema={validationSchema}
        onSubmit={handleSubmit}
      >
        {({ values, setFieldValue }) => (
          <Form>
            <div>
              <label className="block" htmlFor="title">
                Title
              </label>
              <Field type="text" id="title" name="title" />
              <ErrorMessage name="title" component="div" />
            </div>

            <div>
              <label className="block" htmlFor="description">
                Description
              </label>
              <Field type="text" id="description" name="description" />
            </div>

            <div>
              <h2>Flashcards</h2>
              <FieldArray
                name="flashcards"
                render={({ push, remove }: FormikProps) => (
                  <div>
                    {values.flashcards?.map((flashcard: Flashcard, index) => (
                      <div key={flashcard.id} className="">
                        <h3>Flashcard {index + 1}</h3>
                        <div>
                          <label
                            className="block"
                            htmlFor={`flashcards[${index}].term`}
                          >
                            Term
                          </label>
                          <Field
                            type="text"
                            name={`flashcards[${index}].term`}
                          />
                          <ErrorMessage
                            name={`flashcards[${index}].term`}
                            component="div"
                          />
                        </div>

                        <div>
                          <label
                            className="block"
                            htmlFor={`flashcards[${index}].definition`}
                          >
                            Definition
                          </label>
                          <Field
                            type="text"
                            name={`flashcards[${index}].definition`}
                          />
                          <ErrorMessage
                            name={`flashcards[${index}].definition`}
                            component="div"
                          />
                        </div>

                        <div>
                          <p
                            className="cursor-pointer"
                            onClick={() => {
                              setSelectedFlashcard(index);
                            }}
                          >
                            Add Picture
                          </p>
                          {selectedFlashcard === index && (
                            <>
                              <label
                                className="block"
                                htmlFor={`flashcards[${index}].pictureUrl`}
                              >
                                Picture URL
                              </label>
                              <Field
                                type="text"
                                name={`flashcards[${index}].pictureUrl`}
                              />
                              <ErrorMessage
                                name={`flashcards[${index}].pictureUrl`}
                                component="div"
                              />
                              {user &&
                                user.pictures.map((picture, pictureIndex) => (
                                  <img
                                    key={pictureIndex}
                                    src={picture.url}
                                    className="w-1/4 h-1/4"
                                    onClick={() => {
                                      setFieldValue(
                                        `flashcards[${index}].pictureUrl`,
                                        picture.url
                                      );
                                    }}
                                  />
                                ))}
                            </>
                          )}
                        </div>

                        <div>
                          <button type="button" onClick={() => remove(index)}>
                            Remove Flashcard
                          </button>
                        </div>
                      </div>
                    ))}

                    <button
                      type="button"
                      onClick={() =>
                        push({
                          term: "",
                          definition: "",
                          pictureUrl: "",
                        })
                      }
                    >
                      Add Flashcard
                    </button>
                  </div>
                )}
              />
            </div>

            <button type="submit">Submit</button>
          </Form>
        )}
      </Formik>
    </div>
  );
};

const EditNoId = () => {
  const [exisitingSet, setExisitingSet] = useState<FlashcardSet>();
  const { id } = useParams();
  const [user, setUser] = useState<User | null>(null);
  const [selectedFlashcard, setSelectedFlashcard] = useState<number>();

  useEffect(() => {
    const fetchData = async () => {
      if (id) {
        const data = await agent.Set.detail(id);
        const userData = await agent.Account.current();
        setExisitingSet(data);
        setUser(userData);
      }
    };

    fetchData();
  }, [id]);

  let initialValues: FlashcardSet = {
    id: "",
    title: "",
    description: "",
    flashcards: [],
  };

  if (exisitingSet) {
    initialValues = {
      title: exisitingSet.title,
      description: exisitingSet.description,
      flashcards: exisitingSet.flashcards,
    };
  }

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

  const handleSubmit = async (values: FlashcardSet) => {
    const set = {
      id: exisitingSet?.id,
      title: values.title,
      description: values.description || "",
      Flashcards: values.flashcards?.map((flashcard: Flashcard) => ({
        id: flashcard.id ? flashcard.id : null,
        term: flashcard.term,
        definition: flashcard.definition,
        pictureUrl: flashcard.pictureUrl || "",
      })),
    };

    await agent.Set.update(set);

    router.navigate("/Library");
  };

  return (
    <div>
      <h1>Create</h1>
      {exisitingSet ? (
        <Formik
          initialValues={initialValues}
          enableReinitialize
          validationSchema={validationSchema}
          onSubmit={handleSubmit}
        >
          {({ values, setFieldValue }) => (
            <Form>
              <div>
                <label className="block" htmlFor="title">
                  Title
                </label>
                <Field type="text" id="title" name="title" />
                <ErrorMessage name="title" component="div" />
              </div>

              <div>
                <label className="block" htmlFor="description">
                  Description
                </label>
                <Field type="text" id="description" name="description" />
                <ErrorMessage name="description" component="div" />
              </div>

              <div>
                <h2>Flashcards</h2>
                <FieldArray
                  name="flashcards"
                  render={({ push, remove }: FormikProps) => (
                    <div>
                      {values.flashcards?.map(
                        (_flashcard: Flashcard, index) => (
                          <div key={index} className="">
                            <h3>Flashcard {index + 1}</h3>
                            <div>
                              <label
                                className="block"
                                htmlFor={`flashcards[${index}].term`}
                              >
                                Term
                              </label>
                              <Field
                                type="text"
                                name={`flashcards[${index}].term`}
                              />
                              <ErrorMessage
                                name={`flashcards[${index}].term`}
                                component="div"
                              />
                            </div>

                            <div>
                              <label
                                className="block"
                                htmlFor={`flashcards[${index}].definition`}
                              >
                                Definition
                              </label>
                              <Field
                                type="text"
                                name={`flashcards[${index}].definition`}
                              />
                              <ErrorMessage
                                name={`flashcards[${index}].definition`}
                                component="div"
                              />
                            </div>

                            <div>
                              <p
                                className="cursor-pointer"
                                onClick={() => {
                                  setSelectedFlashcard(index);
                                }}
                              >
                                Add Picture
                              </p>
                              {selectedFlashcard === index && (
                                <>
                                  <label
                                    className="block"
                                    htmlFor={`flashcards[${index}].pictureUrl`}
                                  >
                                    Picture URL
                                  </label>
                                  <Field
                                    type="text"
                                    name={`flashcards[${index}].pictureUrl`}
                                  />
                                  <ErrorMessage
                                    name={`flashcards[${index}].pictureUrl`}
                                    component="div"
                                  />
                                  {user &&
                                    user.pictures.map(
                                      (picture, pictureIndex) => (
                                        <img
                                          key={pictureIndex}
                                          src={picture.url}
                                          className="w-1/4 h-1/4"
                                          onClick={() => {
                                            setFieldValue(
                                              `flashcards[${index}].pictureUrl`,
                                              picture.url
                                            );
                                          }}
                                        />
                                      )
                                    )}
                                </>
                              )}
                            </div>

                            <div>
                              <button
                                type="button"
                                onClick={() => remove(index)}
                              >
                                Remove Flashcard
                              </button>
                            </div>
                          </div>
                        )
                      )}

                      <button
                        type="button"
                        onClick={() =>
                          push({
                            term: "",
                            definition: "",
                            pictureUrl: "",
                          })
                        }
                      >
                        Add Flashcard
                      </button>
                    </div>
                  )}
                />
              </div>

              <button type="submit">Submit</button>
            </Form>
          )}
        </Formik>
      ) : null}
    </div>
  );
};

const validationSchema = Yup.object().shape({
  email: Yup.string()
    .matches(
      /^[^\s@]+@([^\s@.,]+\.)+[^\s@.,]{2,}$/,
      "Please enter a valid email"
    )
    .required("Email is required"),
  password: Yup.string().required("Password is required"),
});

const Login = () => {
  return (
    <div
      className="bg-cover"
      style={{ backgroundImage: `url(${loginbg})`, height: "100vh" }}
    >
      <div className="p-3 w-max">
        <Link to="/">
          <img src={logo} className="h-14 w-auto rounded-xl" />
        </Link>
      </div>
      <div className="drop-shadow-[0px_4px_4px_rgba(0,0,0,0.45)] flex top-40 right-0 pr-40 bg-accent rounded-l-xl ml-[50px]">
        <p
          id="title-text"
          className="text-secondary font-bold text-[36px] [text-shadow:_0_3px_0_rgb(7_0_0_/_40%)] ml-[20%]"
        >
          Welcome
        </p>
      </div>
      {/*login form*/}
      <Formik
        initialValues={{
          email: "",
          password: "",
        }}
        validationSchema={validationSchema}
        onSubmit={async (values: UserFormValues) => {
          const user = await agent.Account.login({
            email: values.email,
            password: values.password,
          });
          localStorage.setItem("token", user.token);
          router.navigate("/Library");
        }}
      >
        {({ errors, touched, isValid, isSubmitting }) => (
          <Form /*className='group'*/>
            <div className="flex justify-center">
              <div
                className="z-10 drop-shadow-[0px_4px_4px_rgba(0,0,0,0.45)] absolute bg-cover bg-no-repeat bg-white px-[25px] pt-[25px] pb-[10px] rounded-2xl m-8 max-w-fit"
                style={{ backgroundImage: `url(${flashcardbg}) ` }}
              >
                <div className="flex flex-wrap">
                  <span className="inline-flex items-center px-3 text-[#584289] bg-[#584289] border border-r-0 border-gray-300 rounded-l-md">
                    <svg
                      className="w-4 h-4 text-secondary"
                      aria-hidden="true"
                      xmlns="http://www.w3.org/2000/svg"
                      fill="currentColor"
                      viewBox="0 0 24 24"
                    >
                      <path d="M12 .02c-6.627 0-12 5.373-12 12s5.373 12 12 12 12-5.373 12-12-5.373-12-12-12zm6.99 6.98l-6.99 5.666-6.991-5.666h13.981zm.01 10h-14v-8.505l7 5.673 7-5.672v8.504z" />
                    </svg>
                  </span>
                  {/*email v*/}
                  <Field
                    name="email"
                    type="email"
                    className="shadow-[inset_0_4px_4px_rgba(0,0,0,0.25)] rounded-none rounded-r-lg bg-accent border border-gray-300 text-[#584289] focus:ring-blue-500 focus:border-blue-500 block flex-1 min-w-0 w-full text-sm p-2.5 placeholder:text-[#584289]"
                    placeholder="email"
                    required
                  />
                  {errors.email && touched.email ? (
                    <div className="popup">
                      <p className="popuptext">{errors.email}</p>
                    </div>
                  ) : null}
                </div>
                <div className="flex pt-[10px]">
                  <span className="inline-flex items-center px-[10px] text-[#584289] bg-[#584289] border border-r-0 border-gray-300 rounded-l-md">
                    <svg
                      className="w-5 h-5 text-secondary"
                      aria-hidden="true"
                      xmlns="http://www.w3.org/2000/svg"
                      fill="currentColor"
                      viewBox="0 0 24 24"
                    >
                      <path d="M12 2C6.48 2 2 6.48 2 12C2 17.52 6.48 22 12 22C17.52 22 22 17.52 22 12C22 6.48 17.52 2 12 2ZM17.38 14.5C17.38 16.7 16.7 17.38 14.5 17.38H9.5C7.3 17.38 6.62 16.7 6.62 14.5V13.5C6.62 11.79 7.03 11 8.25 10.73V10C8.25 9.07 8.25 6.25 12 6.25C15.75 6.25 15.75 9.07 15.75 10V10.73C16.97 11 17.38 11.79 17.38 13.5V14.5Z" />
                      <path
                        xmlns="http://www.w3.org/2000/svg"
                        d="M11.9984 15.0984C12.606 15.0984 13.0984 14.606 13.0984 13.9984C13.0984 13.3909 12.606 12.8984 11.9984 12.8984C11.3909 12.8984 10.8984 13.3909 10.8984 13.9984C10.8984 14.606 11.3909 15.0984 11.9984 15.0984Z"
                      />
                      <path
                        xmlns="http://www.w3.org/2000/svg"
                        d="M12 7.75C10.11 7.75 9.75 8.54 9.75 10V10.62H14.25V10C14.25 8.54 13.89 7.75 12 7.75Z"
                      />
                    </svg>
                  </span>
                  {/*password v*/}
                  <Field
                    name="password"
                    type="password"
                    className="shadow-[inset_0_4px_4px_rgba(0,0,0,0.25)] rounded-none rounded-r-lg bg-accent border border-gray-300 text-[#584289] focus:ring-blue-500 focus:border-blue-500 block flex-1 min-w-0 w-full text-sm p-2.5 placeholder:text-[#584289]"
                    placeholder="password"
                    required
                  />{" "}
                  {errors.password && touched.password ? (
                    <div className="popup">
                      <p className="popuptext">{errors.password}</p>
                    </div>
                  ) : null}
                </div>
                <p className="text-[11px]">Forgot password?</p>
                <div className="text-center">
                  <button
                    type="submit"
                    disabled={!isValid || isSubmitting}
                    className="text-white bg-primary hover:bg-blue-600 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg px-[40px] mb-1 text-[20px] focus:outline-none disabled:pointer-events-none disabled:opacity-50"
                  >
                    {isSubmitting ? <CircularProgress /> : "Submit"}
                  </button>
                </div>
              </div>
            </div>
            {/* blue card */}
            <div className="text-center relative m-auto w-[275px] h-[185px] bg-primary drop-shadow-[0px_4px_4px_rgba(0,0,0,0.65)] rounded-2xl top-[61px] left-[30px]">
              <p className="absolute top-[155px] ml-[10px] ">
                Not Registered?{" "}
                <Link
                  to="/Signup"
                  className="text-slate-200 underline underline-offset-2"
                >
                  Sign Up
                </Link>
              </p>
            </div>
          </Form>
        )}
      </Formik>
    </div>
  );
};

const validationSchemasignup = Yup.object().shape({
  username: Yup.string()
    .required("Username is required")
    .max(30)
    .matches(
      /^(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$/,
      "Username must only contain letters, numbers, periods and underscores"
    ),
  email: Yup.string()
    .matches(
      /^[^\s@]+@([^\s@.,]+\.)+[^\s@.,]{2,}$/,
      "Please enter a valid email"
    )
    .required("Email is required"),
  password: Yup.string()
    .required("Password is required")
    .min(6, "Password must be at least 6 characters long")
    .max(10, "Max 10 characters")
    .matches(
      /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$/,
      "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character"
    ),
  confirmPassword: Yup.string()
    .oneOf([Yup.ref("password"), null], "Passwords must match")
    .required("Confirm password is required"),
});

function Signup() {
  return (
    <div
      className="bg-cover"
      style={{ backgroundImage: `url(${loginbg})`, height: "100vh" }}
    >
      <div className="p-3 w-max">
        <Link to="/">
          <img src={logo} className="h-14 w-auto rounded-xl" />
        </Link>
      </div>
      <div className="drop-shadow-[0px_4px_4px_rgba(0,0,0,0.45)] flex top-40 left-0 pl-40 bg-primary rounded-r-xl mr-[50px]">
        <p
          id="title-text"
          className="text-secondary font-bold text-[36px] [text-shadow:_0_3px_0_rgb(7_0_0_/_40%)]"
        >
          Signup
        </p>
      </div>
      {/*login form*/}
      <div className="flex justify-center">
        <div className="drop-shadow-[0px_4px_4px_rgba(0,0,0,0.45)] absolute bg-accent w-[275px] h-[185px] rounded-2xl m-[160px]">
          <p className="mt-[155px] ml-[10px]">
            Already Registered?{" "}
            <Link
              to="/Login"
              className="text-slate-200 underline underline-offset-2"
            >
              Login
            </Link>
          </p>
        </div>
      </div>
      {/* signup card */}
      <div
        className="z-10 pb-[10px] px-[20px] pt-[20px] text-center relative m-auto w-[275px] bg-cover drop-shadow-[0px_4px_4px_rgba(0,0,0,0.65)] rounded-2xl top-[61px] left-[30px]"
        style={{ backgroundImage: `url(${flashcardbg}) ` }}
      >
        {/*form stuff*/}
        <Formik
          initialValues={{
            username: "",
            email: "",
            password: "",
            confirmPassword: "",
          }}
          validationSchema={validationSchema}
          onSubmit={async (values: UserFormValues) => {
            const user = await agent.Account.register({
              username: values.username,
              email: values.email,
              password: values.password,
            });
            console.log(values);
            localStorage.setItem("token", user.token);
            router.navigate("/Library");
          }}
        >
          {({ errors, touched }) => (
            <Form /*form className='group'*/>
              <div className="flex">
                <span className="inline-flex items-center px-3 text-blue-600 bg-blue-700 border border-r-0 border-gray-300 rounded-l-md">
                  <svg
                    className="w-4 h-4 text-secondary"
                    aria-hidden="true"
                    xmlns="http://www.w3.org/2000/svg"
                    fill="currentColor"
                    viewBox="0 0 20 20"
                  >
                    <path d="M10 0a10 10 0 1 0 10 10A10.011 10.011 0 0 0 10 0Zm0 5a3 3 0 1 1 0 6 3 3 0 0 1 0-6Zm0 13a8.949 8.949 0 0 1-4.951-1.488A3.987 3.987 0 0 1 9 13h2a3.987 3.987 0 0 1 3.951 3.512A8.949 8.949 0 0 1 10 18Z" />
                  </svg>
                </span>
                {/*username v*/}
                <Field
                  name="username"
                  type="text"
                  className="shadow-[inset_0_4px_4px_rgba(0,0,0,0.25)] rounded-none rounded-r-lg bg-primary border border-gray-300 text-sky-800 focus:ring-blue-500 focus:border-blue-500 block flex-1 min-w-0 w-full text-sm p-2.5 placeholder:text-sky-800"
                  placeholder="username"
                  required
                />
                {errors.username && touched.username ? (
                  <div>{errors.username}</div>
                ) : null}
              </div>
              <div className="flex">
                <span className="inline-flex items-center px-3 bg-blue-700 border border-r-0 border-gray-300 rounded-l-md">
                  <svg
                    className="w-4 h-4 text-secondary"
                    aria-hidden="true"
                    xmlns="http://www.w3.org/2000/svg"
                    fill="currentColor"
                    viewBox="0 0 24 24"
                  >
                    <path d="M12 .02c-6.627 0-12 5.373-12 12s5.373 12 12 12 12-5.373 12-12-5.373-12-12-12zm6.99 6.98l-6.99 5.666-6.991-5.666h13.981zm.01 10h-14v-8.505l7 5.673 7-5.672v8.504z" />
                  </svg>
                </span>
                {/*email v*/}
                <Field
                  name="email"
                  type="email"
                  className="shadow-[inset_0_4px_4px_rgba(0,0,0,0.25)] rounded-none rounded-r-lg bg-primary border border-gray-300 text-sky-800 focus:ring-blue-500 focus:border-blue-500 block flex-1 min-w-0 w-full text-sm p-2.5 placeholder:text-sky-800"
                  placeholder="email"
                  pattern="^[^\s@]+@([^\s@.,]+\.)+[^\s@.,]{2,}$"
                  required
                />
                {errors.email && touched.email ? (
                  <div>{errors.email}</div>
                ) : null}
              </div>
              <div className="flex pt-[10px]">
                <span className="inline-flex items-center px-[10px] text-sky-800 bg-blue-700 border border-r-0 border-gray-300 rounded-l-md">
                  <svg
                    className="w-5 h-5 text-secondary"
                    aria-hidden="true"
                    xmlns="http://www.w3.org/2000/svg"
                    fill="currentColor"
                    viewBox="0 0 24 24"
                  >
                    <path d="M12 2C6.48 2 2 6.48 2 12C2 17.52 6.48 22 12 22C17.52 22 22 17.52 22 12C22 6.48 17.52 2 12 2ZM17.38 14.5C17.38 16.7 16.7 17.38 14.5 17.38H9.5C7.3 17.38 6.62 16.7 6.62 14.5V13.5C6.62 11.79 7.03 11 8.25 10.73V10C8.25 9.07 8.25 6.25 12 6.25C15.75 6.25 15.75 9.07 15.75 10V10.73C16.97 11 17.38 11.79 17.38 13.5V14.5Z" />
                    <path
                      xmlns="http://www.w3.org/2000/svg"
                      d="M11.9984 15.0984C12.606 15.0984 13.0984 14.606 13.0984 13.9984C13.0984 13.3909 12.606 12.8984 11.9984 12.8984C11.3909 12.8984 10.8984 13.3909 10.8984 13.9984C10.8984 14.606 11.3909 15.0984 11.9984 15.0984Z"
                    />
                    <path
                      xmlns="http://www.w3.org/2000/svg"
                      d="M12 7.75C10.11 7.75 9.75 8.54 9.75 10V10.62H14.25V10C14.25 8.54 13.89 7.75 12 7.75Z"
                    />
                  </svg>
                </span>
                {/*password v*/}
                <Field
                  name="password"
                  type="password"
                  className="shadow-[inset_0_4px_4px_rgba(0,0,0,0.25)] rounded-none rounded-r-lg bg-primary border border-gray-300 text-sky-800 focus:ring-blue-500 focus:border-blue-500 block flex-1 min-w-0 w-full text-sm p-2.5 placeholder:text-sky-800"
                  placeholder="password"
                  required
                />
                {errors.password && touched.password ? (
                  <div>{errors.password}</div>
                ) : null}
              </div>
              {/*password 2 v*/}
              <div className="flex">
                <span className="inline-flex items-center px-[10px] text-sky-800 bg-blue-700 border border-r-0 border-gray-300 rounded-l-md">
                  <svg
                    className="w-5 h-5 text-secondary"
                    aria-hidden="true"
                    xmlns="http://www.w3.org/2000/svg"
                    fill="currentColor"
                    viewBox="0 0 24 24"
                  >
                    <path d="M12 2C6.48 2 2 6.48 2 12C2 17.52 6.48 22 12 22C17.52 22 22 17.52 22 12C22 6.48 17.52 2 12 2ZM17.38 14.5C17.38 16.7 16.7 17.38 14.5 17.38H9.5C7.3 17.38 6.62 16.7 6.62 14.5V13.5C6.62 11.79 7.03 11 8.25 10.73V10C8.25 9.07 8.25 6.25 12 6.25C15.75 6.25 15.75 9.07 15.75 10V10.73C16.97 11 17.38 11.79 17.38 13.5V14.5Z" />
                    <path
                      xmlns="http://www.w3.org/2000/svg"
                      d="M11.9984 15.0984C12.606 15.0984 13.0984 14.606 13.0984 13.9984C13.0984 13.3909 12.606 12.8984 11.9984 12.8984C11.3909 12.8984 10.8984 13.3909 10.8984 13.9984C10.8984 14.606 11.3909 15.0984 11.9984 15.0984Z"
                    />
                    <path
                      xmlns="http://www.w3.org/2000/svg"
                      d="M12 7.75C10.11 7.75 9.75 8.54 9.75 10V10.62H14.25V10C14.25 8.54 13.89 7.75 12 7.75Z"
                    />
                  </svg>
                </span>
                <Field
                  name="confirmPassword"
                  type="password"
                  className="shadow-[inset_0_4px_4px_rgba(0,0,0,0.25)] rounded-none rounded-r-lg bg-primary border border-gray-300 text-blue-600 focus:ring-blue-500 focus:border-blue-500 block flex-1 min-w-0 w-full text-sm p-2.5 placeholder:text-sky-800"
                  placeholder="confirm password"
                  required
                />
                {errors.confirmPassword && touched.confirmPassword ? (
                  <div>{errors.confirmPassword}</div>
                ) : null}
              </div>

              <div className="text-center">
                <button
                  type="submit"
                  disabled={Object.keys(errors).length > 0}
                  className="mt-[10px] text-white text-[20px] bg-accent hover:bg-violet-600 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg px-[40px] mb-1 text-[15px] focus:outline-none disabled:pointer-events-none disabled:opacity-50"
                >
                  Register
                </button>
              </div>
            </Form>
          )}
        </Formik>
      </div>
    </div>
  );
}
