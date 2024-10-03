window.addEventListener('DOMContentLoaded', event => {
    let table = new DataTable('table', {
        responsive: true
    });
});

function ImgError(source) {
    source.src = "/img/static/user.png"
    source.onerror = "";
    return true;
}
