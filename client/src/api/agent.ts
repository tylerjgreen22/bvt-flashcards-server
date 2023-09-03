import axios, { AxiosError, AxiosResponse } from "axios";
import { PaginatedResult } from "../models/pagination";
import { ChangeUserFormValues, User, UserFormValues } from "../models/user";
import { FlashcardSet } from "../models/flashcardSet";
import Picture from "../models/picture";
import { router } from "../routes/Routes";
import { toast } from "react-toastify";

axios.defaults.baseURL = "http://localhost:5000/api/";

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

const sleep = (delay: number) => {
  return new Promise((resolve) => {
    setTimeout(resolve, delay);
  });
};

axios.interceptors.request.use((config) => {
  const token = localStorage.getItem("token");
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

axios.interceptors.response.use(
  async (response) => {
    await sleep(5000);
    const pagination = response.headers["pagination"];
    if (pagination) {
      response.data = new PaginatedResult(
        response.data,
        JSON.parse(pagination)
      );
      return response as AxiosResponse<PaginatedResult<object>>;
    }
    return response;
  },
  (error: AxiosError) => {
    const { data, status } = error.response as AxiosResponse;
    switch (status) {
      case 400:
        toast.error(data);
        break;
      case 401:
        toast.error("unauthorized");
        break;
      case 403:
        toast.error("forbidden");
        break;
      case 404:
        router.navigate("/notFound");
        break;
      case 500:
        console.log(error);
        break;
    }
    return Promise.reject(error);
  }
);

const requests = {
  get: <T>(url: string) => axios.get<T>(url).then(responseBody),
  post: <T>(url: string, body: object) =>
    axios.post<T>(url, body).then(responseBody),
  put: <T>(url: string, body: object) =>
    axios.put<T>(url, body).then(responseBody),
  delete: <T>(url: string) => axios.delete<T>(url).then(responseBody),
};

const Account = {
  current: () => requests.get<User>("account"),
  login: (user: UserFormValues) => requests.post<User>("account/login", user),
  register: (user: UserFormValues) =>
    requests.post<User>("account/register", user),
  username: (user: ChangeUserFormValues) =>
    requests.put<void>("account/username", user),
  password: (user: ChangeUserFormValues) =>
    requests.put<void>("account/password", user),
  delete: (user: ChangeUserFormValues) =>
    requests.post<void>("account/delete", user),
  uploadPicture: (file: Blob) => {
    const formData = new FormData();
    formData.append("File", file);
    return axios.post<Picture>("pictures", formData, {
      headers: { "Content-Type": "multipart/form-data" },
    });
  },
  setProfilePicture: (id: string) =>
    requests.post(`/pictures/${id}/setMain`, {}),
  deletePicture: (id: string) => requests.delete(`/pictures/${id}`),
};

const Set = {
  list: (params: URLSearchParams) =>
    axios
      .get<PaginatedResult<FlashcardSet[]>>("sets", { params })
      .then(responseBody),
  detail: (id: string) => requests.get<FlashcardSet>(`sets/${id}`),
  create: (set: FlashcardSet) => requests.post<void>("sets", set),
  update: (set: FlashcardSet) => requests.put<void>(`sets/${set.id}`, set),
  delete: (id: string) => requests.delete<void>(`sets/${id}`),
};

const agent = {
  Account,
  Set,
};

export default agent;
