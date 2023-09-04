import Picture from "./picture";

// User model
export interface User {
  username: string;
  token: string;
  image: string;
  pictures: Picture[];
}

// Log in / sign up user form values
export interface UserFormValues {
  email: string;
  password: string;
  username?: string;
  confirmPassword?: string;
}

// Change username / password form values
export interface ChangeUserFormValues {
  username?: string;
  password?: string;
  newPassword?: string;
  confirmPassword?: string;
}
