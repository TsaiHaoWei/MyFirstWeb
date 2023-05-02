const wrapper = document.querySelector('.wrapper');
const loginlink = document.querySelector('.login-link');
const registerlink = document.querySelector('.register-link');
const forgotlink = document.querySelector('.forgot-link');


registerlink.addEventListener('click', () => {  
    wrapper.classList.add('active');
});
//loginlink.addEventListener('click', () => {
//    wrapper.classList.remove('active');
//});
forgotlink.addEventListener('click', () => {
    wrapper.classList.add('forgot');
});

