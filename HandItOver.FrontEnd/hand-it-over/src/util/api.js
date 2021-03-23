import axios from 'axios';

const apiUrl = 'https://192.168.1.16:5003'

const authScheme = 'Bearer'

function isAuthorized() {
    return getAuth() !== null;
}

function getAuth() {
    return JSON.parse(localStorage.getItem('auth'));
}

function setAuth(authToken, refreshToken, email) {
    localStorage.setItem('auth', JSON.stringify({
        authToken,
        refreshToken,
        email
    }));
}

function removeAuth() {
    localStorage.removeItem('auth');
}

axios.interceptors.request.use(r => {
    if (isAuthorized()) {
        let authToken = getAuth().authToken;
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
                refreshToken: getAuth().refreshToken
            }).then(r => {
                let data = r.data;
                let auth = getAuth();
                setAuth(data.authToken, data.refreshToken, auth.email);
                return axios(originalRequest);
            }).catch(e => {
                removeAuth();
                return Promise.reject(e);
            });
    }
    return Promise.reject(error);
});

function register(email, fullName, password, role){
    return sendPost('/auth/register', null, {
        email, fullName, password, role
    });
}

function login(email, password) {
    return sendPost('/auth/login', null, { email, password })
        .then(r => {
            let data = r.data;
            setAuth(data.token, data.refreshToken, data.email);
        });
}

function logout() {
    let auth = getAuth();
    if (!auth){
        throw new Error("Already logged out.")
    }
    return sendPost('/auth/revoke', null, { refreshToken: auth.refreshToken })
        .then(function() {
            removeAuth();
        })
}

function sendPost(url, params, data) {
    return axios.post(apiUrl + url, data, { params });
}

function sendGet(url, params) {
    return axios.get(apiUrl + url, {
        params: params
    })
}

export default {
    getAuth,
    register,
    login,
    logout,
    sendGet
};