import Picture from "./picture";

export interface User {
  username: string;
  token: string;
  image: string;
  pictures: Picture[];
}

export interface UserFormValues {
  email: string;
  password: string;
  username?: string;
  confirmPassword?: string;
}

export interface ChangeUserFormValues {
  username?: string;
  password?: string;
  newPassword?: string;
  confirmPassword?: string;
}
