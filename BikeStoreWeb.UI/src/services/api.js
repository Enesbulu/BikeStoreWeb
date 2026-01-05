import axios from 'axios';

const BASE_URL = 'https://localhost:7212/api/v1';
const api = axios.create({
    baseURL: BASE_URL,
});

export default api;