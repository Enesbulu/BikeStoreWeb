import axios from 'axios';

const BASE_URL = 'https://localhost:7212/api/v1';
const api = axios.create({
    baseURL: BASE_URL,
});

api.interceptors.request.use(
    (configs) => {
        const token = localStorage.getItem('token');
        if (token) {
            configs.headers.Authorization = `Bearer ${token}`;
        }

        return configs;
    },
    (error) => {
        return Promise.reject(error);
    }
);

export default api;