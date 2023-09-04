import { Link } from "react-router-dom";

import flashcardbg from "../assets/flashcard-bg.jpg";
import loginbg from "../assets/login-bg.png";
import logo from "../assets/logo.png";

import { Formik, Form, Field } from "formik";
import * as Yup from "yup";
import "./SignUp.css";
import agent from "../api/agent";
import { UserFormValues } from "../models/user";
import { router } from "./Routes";
import { useUserContext } from "../context/UserContext";
import { useState } from "react";
import CircularProgress from "@mui/material/CircularProgress";

// Validation schema for login, checks that email is valid and both email and password are entered
const validationSchema = Yup.object().shape({
  email: Yup.string()
    .matches(
      /^[^\s@]+@([^\s@.,]+\.)+[^\s@.,]{2,}$/,
      "Please enter a valid email"
    )
    .required("Email is required"),
  password: Yup.string().required("Password is required"),
});

// Log in
const Login = () => {
  const { setUser } = useUserContext();
  const [loading, setLoading] = useState(false);

  return (
    <div
      className="bg-cover overflow-y-hidden"
      style={{ backgroundImage: `url(${loginbg})`, height: "100vh" }}
    >
      {/* Link to home  */}
      <div className="p-3 w-max">
        <Link to="/">
          <img src={logo} className="h-14 w-auto rounded-xl" />
        </Link>
      </div>

      {/* Title  */}
      <div className="drop-shadow-[0px_4px_4px_rgba(0,0,0,0.45)] flex top-40 right-0 pr-40 bg-accent rounded-l-xl ml-[50px]">
        <p
          id="title-text"
          className="text-secondary font-bold text-[36px] [text-shadow:_0_3px_0_rgb(7_0_0_/_40%)] ml-[20%]"
        >
          Welcome
        </p>
      </div>
      {/* Signup card */}
      <div className="flex justify-center mt-16">
        <div className="relative w-[275px] h-[185px] bg-primary drop-shadow-[0px_4px_4px_rgba(0,0,0,0.65)] rounded-2xl -top-[5px] left-[30px]">
          <p className="absolute top-[10px] ml-[10px] ">
            Not Registered?{" "}
            <Link
              to="/Signup"
              className="text-white underline underline-offset-2"
            >
              Sign Up
            </Link>
          </p>
        </div>
      </div>

      {/* Login card */}
      <div
        className="z-10 drop-shadow-[0px_4px_4px_rgba(0,0,0,0.45)] relative m-auto bg-cover bg-no-repeat bg-white px-[25px] pt-[25px] pb-[10px] rounded-2xl -top-[150px] max-w-fit"
        style={{ backgroundImage: `url(${flashcardbg}) ` }}
      >
        {/* Login form */}
        <Formik
          initialValues={{
            email: "",
            password: "",
          }}
          validationSchema={validationSchema}
          // On submit, attempt to login user and set LS token, as well as set user in context. Naivgate to library if successful
          onSubmit={async (values: UserFormValues) => {
            setLoading(true);
            try {
              const user = await agent.Account.login({
                email: values.email,
                password: values.password,
              });
              localStorage.setItem("token", user.token);
              setUser(user);
              router.navigate("/Library");
            } catch (error) {
              console.log(error);
            } finally {
              setLoading(false);
            }
          }}
        >
          {({ errors, touched }) => (
            <Form>
              <div className="flex">
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
                {/* Email v*/}
                <Field
                  name="email"
                  type="email"
                  className="shadow-[inset_0_4px_4px_rgba(0,0,0,0.25)] rounded-none rounded-r-lg bg-accent border border-gray-300 text-[#584289] focus:ring-blue-500 focus:border-blue-500 block flex-1 min-w-0 w-full text-sm p-2.5 placeholder:text-[#584289]"
                  placeholder="email"
                  required
                />
                {errors.email && touched.email ? (
                  <div className="max-[645px]:hidden popup">
                    <p className="max-[645px]:hidden popuptext bg-red-500 rounded-xl text-white">
                      {errors.email}
                    </p>
                  </div>
                ) : null}
              </div>
              {errors.email && touched.email ? (
                <p className="min-[645px]:hidden bg-red-500 rounded-xl text-white text-center">
                  {errors.email}
                </p>
              ) : null}
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
                {/* Password */}
                <Field
                  name="password"
                  type="password"
                  className="shadow-[inset_0_4px_4px_rgba(0,0,0,0.25)] rounded-none rounded-r-lg bg-accent border border-gray-300 text-[#584289] focus:ring-blue-500 focus:border-blue-500 block flex-1 min-w-0 w-full text-sm p-2.5 placeholder:text-[#584289]"
                  placeholder="password"
                  required
                />{" "}
                {errors.password && touched.password ? (
                  <div className="max-[645px]:hidden popup">
                    <p className="max-[645px]:hidden popuptext bg-red-500 rounded-xl text-white">
                      {errors.password}
                    </p>
                  </div>
                ) : null}
              </div>
              {errors.password && touched.password ? (
                <p className="min-[645px]:hidden bg-red-500 rounded-xl text-white text-center">
                  {errors.password}
                </p>
              ) : null}
              {/* Submit  */}
              <div className="text-center pt-4">
                <button
                  type="submit"
                  disabled={Object.keys(errors).length > 0}
                  className="text-white bg-primary hover:bg-blue-600 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg px-[40px] mb-1 text-[20px] focus:outline-none disabled:pointer-events-none disabled:opacity-50"
                >
                  {loading ? <CircularProgress size={30} /> : "Login"}
                </button>
              </div>
            </Form>
          )}
        </Formik>
      </div>
    </div>
  );
};
export default Login;
