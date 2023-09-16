
function ShowErrorAlert(msg) {
    Swal.fire({
        title: "",
        text: msg,
        icon: "error"
    }).then((result) => {
        if (result.isConfirmed || result.dismiss === Swal.DismissReason.timer) {
            unblockMainView();
        }
    });

    return false;
}

function ShowErrorAlertInPopup(msg) {
    Swal.fire({
        title: "",
        text: msg,
        icon: "error",
    }).then((result) => {
        if (result.isConfirmed || result.dismiss === Swal.DismissReason.timer) {
            unblockPopupView();
        }
    });
    return false;
}

function ConfirmMessage(msg, callback) {
    Swal.fire({
        title: msg,
       
    }).then((result) => {
        if (result) {
            if (callback && typeof callback === "function") {
                blockMainView();
                setTimeout(function () {
                    return callback();
                }, 200);
            }
        }
    });
}

function ShowSuccessAlert(msg, callback) {
    Swal.fire({
        title: "",
        text: msg,
        icon: "success"
    }).then((result) => {
        if (result) {
            if (callback && typeof callback === "function") {
                setTimeout(function () {
                    callback();
                }, 200);
            }
        }
    });
}

function ShowSuccessAlertRedirect(msg, url) {
    Swal.fire({
        title: "",
        text: msg,
        icon: "success"
    }).then((result) => {
        if (result.isConfirmed || result.dismiss === Swal.DismissReason.timer) {
            window.location.href = url;
        }
    });
}


function ShowSuccessPopupAlert(msg, callback) {
    Swal.fire({
        title: "",
        text: msg,
        icon: "success"
    }).then((isConfirm) => {
        if (isConfirm) {
            // Assuming jQuery is available to use the jQuery code
            var modal = document.getElementById('dvModal');
            if (modal) {
                modal.style.display = 'none';
            }
            if (callback && typeof callback === "function") {
                setTimeout(function () {
                    callback();
                }, 200);
            }
        }
    });
}

function ShowSuccessPopupAlertReload(msg) {
    Swal.fire({
        title: "",
        text: msg,
        icon: "success",
        showCancelButton: false,
        confirmButtonText: "Ok",
        onClose: function () {
            // Assuming jQuery is available to use the jQuery code
            if (typeof $ !== 'undefined') {
                $('#dvModal').modal('hide');
            } else {
                // If jQuery is not available, use vanilla JavaScript to hide the modal (if required).
                var modal = document.getElementById('dvModal');
                if (modal) {
                    modal.style.display = 'none';
                }
            }
            window.location.reload();
        }
    });
}


