import { useState } from "react";
import { ChangeUserFormValues } from "../models/user";
import agent from "../api/agent";
import * as Yup from "yup";
import { Formik, ErrorMessage, Field, Form } from "formik";
import { Avatar } from "@mui/material";
import Picture from "../models/picture";
import PictureUploadWidget from "../components/pictureUpload/PictureUploadWidget";
import { useUserContext } from "../context/UserContext";
import CircularProgress from "@mui/material/CircularProgress";

const Profile = () => {
  const { user, setUser } = useUserContext();
  const [addPictureMode, setAddPictureMode] = useState(false);
  const [loading, setLoading] = useState(false);
  const [loadingUsername, setLoadingUsername] = useState(false);
  const [loadingPictures, setLoadingPictures] = useState(false);

  const regexPattern =
    /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+}{"':;?/>.<,])(?!.*\s).{6,10}$/;

  const usernameSchema = Yup.object({
    username: Yup.string().required("The new username is required"),
    password: Yup.string().required("Password is required"),
  });

  const passwordSchema = Yup.object({
    password: Yup.string().required("Password is required"),
    newPassword: Yup.string()
      .required("New password is required")
      .matches(
        regexPattern,
        "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character"
      ),
    confirmPassword: Yup.string()
      .oneOf([Yup.ref("newPassword")], "Passwords must match")
      .required("Confirm password is required"),
  });

  const handleSubmit = async (values: ChangeUserFormValues) => {
    if (values.newPassword) {
      setLoading(true);
      try {
        await agent.Account.password(values);
        const user = await agent.Account.current();
        setUser(user);
        setLoading(false);
      } catch (error) {
        setLoading(false);
      }
    } else {
      setLoadingUsername(true);
      try {
        await agent.Account.username(values);
        const user = await agent.Account.current();
        setUser(user);
        setLoadingUsername(false);
      } catch (error) {
        setLoadingUsername(false);
      }
    }
  };

  const handlePictureUpload = async (file: Blob) => {
    setLoadingPictures(true);

    try {
      await agent.Account.uploadPicture(file);

      const data = await agent.Account.current();
      setUser(data);

      setAddPictureMode(false);

      setLoadingPictures(false);
    } catch (error) {
      setAddPictureMode(false);

      setLoadingPictures(false);
    }
  };

  const handleSetProfilePicture = async (pictureId: string) => {
    setLoadingPictures(true);

    try {
      const result = await agent.Account.setProfilePicture(pictureId);

      if (result) {
        const data = await agent.Account.current();
        setUser(data);
      }

      setLoadingPictures(false);
    } catch (error) {
      setLoadingPictures(false);
    }
  };

  const handleDeletePicture = async (pictureId: string) => {
    setLoadingPictures(true);

    try {
      await agent.Account.deletePicture(pictureId);

      const data = await agent.Account.current();
      setUser(data);

      setLoadingPictures(false);
    } catch (error) {
      setLoadingPictures(false);
    }
  };

  if (user) {
    return (
      <div className="max-w-[1100px] mx-auto mt-6 p-2 mb-32">
        <div className="w-fit mx-auto">
          {user.image ? (
            <Avatar
              src={user.image}
              className="cursor-pointer"
              sx={{ width: 250, height: 250 }}
            />
          ) : (
            <Avatar
              sx={{ bgcolor: "#ff5722", width: 150, height: 150 }}
              className="cursor-pointer"
            >
              <p className="text-6xl">{user.username[0].toUpperCase()}</p>
            </Avatar>
          )}
          <div className="py-2 px-8 bg-accent w-fit rounded-full border-2 border-black mx-auto mt-4">
            <h3 className="text-lg font-bold">{user?.username}</h3>
          </div>
        </div>

        <div className="bg-accent p-6 mt-4 rounded-xl w-3/4 lg:w-1/2 mx-auto">
          {addPictureMode ? (
            <>
              <div className="flex justify-between items-center mb-4">
                <h2 className="text-xl font-bold">Add Picture</h2>
                <button
                  className="text-white bg-purple-900 w-28 rounded-md p-1"
                  onClick={() => setAddPictureMode(!addPictureMode)}
                >
                  Cancel
                </button>
              </div>
              {loadingPictures ? (
                <div className="flex items-center">
                  <CircularProgress size={100} className="mx-auto" />
                </div>
              ) : (
                <PictureUploadWidget uploadPicture={handlePictureUpload} />
              )}
            </>
          ) : (
            <>
              <div className="flex justify-between items-center mb-8">
                <h2 className="text-xl font-bold">Your Pictures</h2>
                <button
                  className="text-white bg-purple-900 w-28 rounded-full p-2"
                  onClick={() => setAddPictureMode(!addPictureMode)}
                >
                  Add Picture
                </button>
              </div>
              <div>
                {user.pictures?.length ? (
                  <div
                    className={
                      loadingPictures
                        ? ""
                        : "flex flex-col gap-2 items-center justify-center md:grid md:grid-cols-3 md:gap-3"
                    }
                  >
                    {loadingPictures ? (
                      <div className="flex items-center">
                        <CircularProgress size={100} className="mx-auto" />
                      </div>
                    ) : (
                      user.pictures.map((picture: Picture) => (
                        <div key={picture.id}>
                          <img src={picture.url} />
                          <div
                            className="flex rounded-md shadow-sm mt-2"
                            role="group"
                          >
                            <button
                              type="button"
                              className="px-4 py-2 text-sm font-medium text-white bg-gray-700  rounded-l-lg"
                              onClick={() =>
                                handleSetProfilePicture(picture.id)
                              }
                            >
                              Profile Picture
                            </button>
                            <button
                              type="button"
                              className="px-4 py-2 text-sm font-medium text-white bg-gray-400  rounded-r-md"
                              onClick={() => handleDeletePicture(picture.id)}
                            >
                              Delete
                            </button>
                          </div>
                        </div>
                      ))
                    )}
                  </div>
                ) : (
                  <div>
                    <p>No pictures yet!</p>
                  </div>
                )}
              </div>
            </>
          )}
        </div>

        <div className="bg-accent p-6 mt-4 rounded-xl w-3/4 lg:w-1/2 mx-auto">
          <h2 className="text-xl font-bold mb-4">Change Username</h2>
          <Formik
            initialValues={{ username: "", password: "", error: null }}
            validationSchema={usernameSchema}
            onSubmit={(values, { resetForm }) => {
              handleSubmit(values);
              resetForm();
            }}
          >
            {({ isValid, isSubmitting, dirty }) => (
              <Form className="flex flex-col gap-4">
                <Field
                  type="text"
                  name="username"
                  className="border-b-2 bg-transparent text-black placeholder-black"
                  placeholder="Enter new username"
                />
                <ErrorMessage name="username" component="div" />
                <Field
                  type="password"
                  name="password"
                  className="border-b-2 bg-transparent text-black placeholder-black"
                  placeholder="Enter new password"
                />
                <ErrorMessage name="password" component="div" />
                <button
                  type="submit"
                  name="usernameBtn"
                  className="text-white bg-purple-900 w-28 rounded-full p-2 mx-auto disabled:bg-gray-900 disabled:text-gray-200"
                  disabled={isSubmitting || !isValid || !dirty}
                >
                  {loadingUsername ? <CircularProgress size={30} /> : "Submit"}
                </button>
              </Form>
            )}
          </Formik>
        </div>

        <div className="flex flex-col gap-4 bg-accent p-6 mt-4 rounded-xl w-3/4 lg:w-1/2 mx-auto">
          <h2 className="text-xl font-bold">Change Password</h2>
          <Formik
            initialValues={{
              password: "",
              newPassword: "",
              confirmPassword: "",
              error: null,
            }}
            validationSchema={passwordSchema}
            onSubmit={(values, { resetForm }) => {
              handleSubmit(values);
              resetForm();
            }}
          >
            {({ isValid, isSubmitting, dirty }) => (
              <Form className="flex flex-col gap-4">
                <Field
                  type="password"
                  name="password"
                  className="border-b-2 bg-transparent text-black placeholder-black"
                  placeholder="Enter password"
                />
                <ErrorMessage name="password" component="div" />
                <Field
                  type="password"
                  name="newPassword"
                  className="border-b-2 bg-transparent text-black placeholder-black"
                  placeholder="Enter new password"
                />
                <ErrorMessage name="newPassword" component="div" />
                <Field
                  type="password"
                  name="confirmPassword"
                  className="border-b-2 bg-transparent text-black placeholder-black"
                  placeholder="Confirm new password"
                />
                <ErrorMessage name="confirmPassword" component="div" />
                <button
                  type="submit"
                  className="text-white bg-purple-900 w-28 rounded-full p-2 mx-auto disabled:bg-gray-900 disabled:text-gray-200"
                  disabled={isSubmitting || !isValid || !dirty}
                >
                  {loading ? <CircularProgress size={30} /> : "Submit"}
                </button>
              </Form>
            )}
          </Formik>
        </div>
      </div>
    );
  } else {
    return null;
  }
};

export default Profile;
