function homeService() {
    return new JK_ServiceCore({
        servicePath: "/storesetup/location-categories/",
        detailHistPath: "",
        saveConfirmAlert: "Location would be saved! Are you sure?",
        saveSuccessAlert: "Location was saved successfully",
        deleteConfirmAlert: "Location would be deleted! Are you sure?",
        deleteSuccessAlert: "Location was deleted successfully",
    });
}


function loadView() {
    homeService().displayDashView();
}

function onSavePopup(formId, processType) {

    console.log("Form ID:", formId);
    console.log("Process Type:", processType);

    alert(formId);
    alert(processType);
    const formValidator = new ('.needs-validation');

    return this;
}

// Custom extension method to attach an event listener and call the desired function
Element.prototype.onSavePopupData = function (formId, processType) {
    console.log("ES");
    this.addEventListener("click", function (e) {
        e.preventDefault();
        onSavePopup(formId, processType);
    });
};
