import axios from "axios";

const options = {
  baseURL: import.meta.env.VITE_API_BASE_URL,
  timeout: 1000,
};

console.log("axios options: ", options);

export default axios.create(options);
