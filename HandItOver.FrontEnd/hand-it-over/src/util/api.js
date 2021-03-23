import axios from 'axios';

const apiUrl = 'https://192.168.1.16:5003'

const authScheme = 'Bearer'

function getAuthToken() {
    return localStorage.getItem('authToken');
}

function setAuthToken(newAuthToken) {
    return localStorage.setItem('authToken', newAuthToken);
}

function getRefreshToken() {
    return localStorage.getItem('refreshToken');
}

function setRefreshToken(newRefreshToken) {
    return localStorage.getItem('refreshToken', newRefreshToken);
}

axios.interceptors.request.use(r => {
    if (isAuthorized()) {
        let authToken = getAuthToken();
        r.headers.Authorization = `${authScheme} ${authToken}`;
    }
    return r;
})

axios.interceptors.response.use(r => {
    return r;
}, function (error) {
    console.log(error);
    const originalRequest = error.config;
    if (error.response.status === 401
        && isAuthorized()
        && !originalRequest.isRetry) {
        originalRequest.isRetry = true;

        return axios.post(apiUrl + '/auth/refresh',
            {
                refreshToken: getRefreshToken()
            }).then(r => {
                let data = r.data;
                setAuthToken(data.authToken);
                setRefreshToken(data.refreshToken);
                return axios(originalRequest);
            }).catch(e => {
                deauthorize();
                return Promise.reject(e);
            });
    }
    return Promise.reject(error);
});

function isAuthorized() {
    return getAuthToken() !== null;
}

function authorize(token, refreshToken) {
    setAuthToken(token);
    setRefreshToken(refreshToken);
}

function deauthorize() {
    setAuthToken(null);
    setRefreshToken(null);
}

function sendGet(url, params) {
    return axios.get(apiUrl + url, {
        params: params
    })
}

export default {
    authorize,
    deauthorize,
    sendGet
};