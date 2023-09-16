//function forceNumericOnly() {
//    function isNumericKey(key) {
//        return (
//            key === 8 || // Backspace
//            key === 9 || // Tab
//            key === 13 || // Enter
//            key === 110 || // Numpad decimal point
//            key === 190 || // Decimal point (period)
//            (key >= 35 && key <= 40) || // Home, End, Arrow keys
//            (key >= 48 && key <= 57) || // Numeric keys (0-9)
//            (key >= 96 && key <= 105) // Numpad numeric keys
//        );
//    }

//    //element.addEventListener('keydown', function (event) {
//    //    var key = event.charCode || event.keyCode || 0;
//    //    if (!isNumericKey(key)) {
//    //        event.preventDefault();
//    //    }
//    //});
//}


function ForceDecimal(element) {
    element.addEventListener("keydown", function (event) {
        var key = event.keyCode || event.charCode || 0;
        return (
            key === 8 ||
            key === 9 ||
            key === 13 ||
            //key === 46 ||
            key === 110 ||
            //key === 190 ||
            (key >= 35 && key <= 40) ||
            (key >= 48 && key <= 57) ||
            (key >= 96 && key <= 105));
    });
}

function ForceRichText(element) {
    element.addEventListener("keydown", function (event) {
        var key = event.keyCode || event.charCode || 0;
        return (
            key === 8 ||
            key === 9 ||
            key === 13 ||
            key === 16 ||
            key === 32 ||
            key === 20 ||
            key === 46 ||
            key === 110 ||
            key === 190 ||
            (key >= 35 && key <= 40) ||
            (key >= 48 && key <= 57) ||
            (key >= 65 && key <= 90) ||
            (key >= 96 && key <= 111) ||
            (key >= 186 && key <= 191));
    });
}

function forcePlainText(inputElement) {
    inputElement.addEventListener('input', function () {
        var c = this.selectionStart,
            r = /[^a-z0-9-_ ]/gi, //A-Z
            v = this.value;
        if (r.test(v)) {
            this.value = v.replace(r, '');
            c--;
        }
        this.setSelectionRange(c, c);
    });
}

const blockView = (elementId) => {
    const element = document.getElementById(elementId);
    if (element) {
        element.classList.add('block-view');
    }
};

const unblockView = (elementId) => {
    const element = document.getElementById(elementId);
    if (element) {
        element.classList.remove('block-view');
    }
};

const blockMainView = () => {
    blockView('mainViewDiv');
};

const unblockMainView = () => {
    unblockView('mainViewDiv');
};

const blockPopupView = () => {
    blockView('popupViewDiv');
};

const unblockPopupView = () => {
    unblockView('popupViewDiv');
};

function ErrorMessage(msg, id) {
    Swal.fire("", msg, "error");
    if (id != undefined) {
        document.getElementById(id).focus();
        document.getElementById(id).value = "";
    }
}

function SuccessMessage(msg, id) {
    Swal.fire("", msg, "success");
    if (id != undefined) {
        document.getElementById(id).focus();
        document.getElementById(id).value = "";
    }
}
function InfoMessage(msg, id) {
    Swal.fire("", msg, "info");
    if (id != undefined) {
        document.getElementById(id).focus();
        document.getElementById(id).value = "";
    }
}
function InlineErrorMessage(msg, id) {
    if (id == undefined) {
        alert(msg);
    } else {
        var html = '<div class="alert alert-danger "><a href="#" class="close" data-bs-dismiss="alert" aria-label="close">×</a><i class="fa fa-remove fa-2x"></i>' + msg + '</div>';
        $("#" + id).html(html);
    }
}
function InlineSuccessMessage(msg, id) {
    if (id == undefined) {
        alert(msg);
    } else {
        var html = '<div class="alert alert-success "><a href="#" class="close" data-bs-dismiss="alert" aria-label="close">×</a> <i class="fa fa-check fa-2x"></i><strong> Success!</strong> ' + msg + '</div>';
        $("#" + id).html(html);
    }
}
function ClearInlineError(id) {
    $("#" + id).html("");
}

