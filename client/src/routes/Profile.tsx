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

// User profile page
const Profile = () => {
  const { user, setUser } = useUserContext();
  const [addPictureMode, setAddPictureMode] = useState(false);
  const [loading, setLoading] = useState(false);
  const [loadingUsername, setLoadingUsername] = useState(false);
  const [loadingPictures, setLoadingPictures] = useState(false);

  const regexPattern =
    /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+}{"':;?/>.<,])(?!.*\s).{6,10}$/;

  // Check username and password are present for change username
  const usernameSchema = Yup.object({
    username: Yup.string().required("The new username is required"),
    password: Yup.string().required("Password is required"),
  });

  // Check passwords are present and are complex
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

  // On submit, if attemp to change username or password, refetch user information and set context
  const handleSubmit = async (values: ChangeUserFormValues) => {
    if (values.newPassword) {
      setLoading(true);

      try {
        await agent.Account.password(values);
        setUser(await agent.Account.current());
        setLoading(false);
      } catch (error) {
        console.log(error);
      } finally {
        setLoading(false);
      }
    } else {
      setLoadingUsername(true);

      try {
        await agent.Account.username(values);
        setUser(await agent.Account.current());
        setLoadingUsername(false);
      } catch (error) {
        console.log(error);
      } finally {
        setLoadingUsername(false);
      }
    }
  };

  // On picture upload, upload picture to Cloudinary and refetch user, then reset user context. After, turn off picture add mode
  const handlePictureUpload = async (file: Blob) => {
    setLoadingPictures(true);

    try {
      await agent.Account.uploadPicture(file);
      setUser(await agent.Account.current());
    } catch (error) {
      console.log(error);
    } finally {
      setAddPictureMode(false);
      setLoadingPictures(false);
    }
  };

  // Set the users profile picture to the selected picture and refetch user. Reset user context
  const handleSetProfilePicture = async (pictureId: string) => {
    setLoadingPictures(true);

    try {
      await agent.Account.setProfilePicture(pictureId);
      setUser(await agent.Account.current());
    } catch (error) {
      console.log(error);
    } finally {
      setLoadingPictures(false);
    }
  };

  // Delete the picture from the users pictures and refetch and reset user data and context
  const handleDeletePicture = async (pictureId: string) => {
    setLoadingPictures(true);

    try {
      await agent.Account.deletePicture(pictureId);
      setUser(await agent.Account.current());
    } catch (error) {
      console.log(error);
    } finally {
      setLoadingPictures(false);
    }
  };

  if (user) {
    return (
      <div className="max-w-[1100px] mx-auto mt-6 p-2 mb-48 md:mb-0">
        {/* User picture and name  */}
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

        {/* User pictures and add pictures */}
        <div className="bg-accent p-6 mt-4 rounded-xl w-full md:w-3/4 lg:w-1/2 mx-auto">
          {addPictureMode ? (
            <>
              {/* Add picture  */}
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
              {/* User pictures  */}
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

        {/* Change username */}
        <div className="bg-accent p-6 mt-4 rounded-xl w-full md:w-3/4 lg:w-1/2 mx-auto">
          <h2 className="text-xl font-bold mb-4">Change Username</h2>
          {/* Change username form  */}
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
                {/* New username  */}
                <Field
                  type="text"
                  name="username"
                  className="border-b-2 bg-transparent text-black placeholder-black"
                  placeholder="Enter new username"
                />
                <ErrorMessage name="username" component="div" />

                {/* Password  */}
                <Field
                  type="password"
                  name="password"
                  className="border-b-2 bg-transparent text-black placeholder-black"
                  placeholder="Enter new password"
                />
                <ErrorMessage name="password" component="div" />

                {/* Submit  */}
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

        {/* Change password  */}
        <div className="flex flex-col gap-4 bg-accent p-6 mt-4 rounded-xl w-full md:w-3/4 lg:w-1/2 mx-auto">
          <h2 className="text-xl font-bold">Change Password</h2>
          {/* Change password form  */}
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
                {/* Password  */}
                <Field
                  type="password"
                  name="password"
                  className="border-b-2 bg-transparent text-black placeholder-black"
                  placeholder="Enter password"
                />
                <ErrorMessage name="password" component="div" />
                {/* New password  */}
                <Field
                  type="password"
                  name="newPassword"
                  className="border-b-2 bg-transparent text-black placeholder-black"
                  placeholder="Enter new password"
                />
                <ErrorMessage name="newPassword" component="div" />
                {/* Confirm new password  */}
                <Field
                  type="password"
                  name="confirmPassword"
                  className="border-b-2 bg-transparent text-black placeholder-black"
                  placeholder="Confirm new password"
                />
                <ErrorMessage name="confirmPassword" component="div" />
                {/* Submit  */}
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
  }
};

export default Profile;
