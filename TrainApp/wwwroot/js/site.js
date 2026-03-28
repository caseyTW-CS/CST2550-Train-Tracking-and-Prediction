// force nav links to truly centre regardless of bootstrap
document.addEventListener('DOMContentLoaded', function () {
    const nav = document.querySelector('.navbar-nav');
    const navbar = document.querySelector('.navbar');
    if (nav && navbar) {
        nav.style.position = 'absolute';
        nav.style.left = '50%';
        nav.style.transform = 'translateX(-50%)';
        nav.style.top = '50%';
        nav.style.marginTop = '-' + (nav.offsetHeight / 2) + 'px';
    }
});